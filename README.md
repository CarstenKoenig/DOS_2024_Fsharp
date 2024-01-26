# F# Workshop

Material F# Workshop für den DeveloperOpenSpace 2023/24

## Link

Github: [github.com/CarstenKoenig/DOS_2024_Fsharp](https://github.com/CarstenKoenig/DOS_2024_Fsharp)

---

## Grundlagen

### Werte und Typen

#### `let` mit ein paar Zahlen

Beispiel:

```fsharp
    let antwort = 42;;
    antwort / 2 + 2;;
```

#### Typen und Typ-Synonyme mit `type`

```fsharp
    type Zahl = int
```

#### generische Typen

```fsharp
 type Mit<'a> = 'a
 type 'a Mit  = 'a
 let x : _ Mit = 5
```

#### UNIT

- statt `void` benutzen wir `unit`
- `unit` hat nur einen Wert `()`
- spart die lästige Unterscheidung Funktion/Prozedur

#### BOTTOM

als *nicht-totale* Sprache haben wir immer einen zustäzlichen Wert *bottom*:

```fsharp
 failwith "bottom"
```

---

### Funktionen

#### einfache Definition und Typen

```fsharp
 let plus10 x = x + 10;;
 plus10 5;;
```

#### Applikation hat höchste Priorität

```fsharp
 plus10 5 * 5
```

#### Einrückungsregeln

```fsharp
let plus10 x =
 x + 10
```

#### Funktionen als Werte / innere Funktionen

```fsharp
 let plus10 x =
  let plus5 x =
   x+5
  plus5 x + plus5 x
```

#### Lambdas

```fsharp
  let plus10 =
  fun x -> x + 10
```

#### Komposition / Verkettung

```fsharp
 let f x = x + x
 let g x = string x

 g (f 5) = (g << f) 4 = (f >> g) 4

 4 |> f |> g
 g <| (f <| 4)
```

#### mehrere Argumente / Currying

```fsharp
 let f x y = x + y
 f 3 4;;
```

#### Vorsicht

Applikation assoziiert nach links

```fsharp
 let hi s = "Hallo " + s;;
 hi System.DateTime.Now.ToString();;
```

##### **Merksatz:**

> *Typ-Signaturen* sind **recht**-assoziiert, *Funktions-Applikation* ist **links**-assoziiert

#### partielle Applikation

```fsharp
 let plus10 = f 10
```

Als weiteres Beispiel: `printfn` und co.

#### FizzBuzz

```fsharp
 let fizzBuzz n =
  match n with
  | _ when n % 15 = 0 -> "FizzBuzz"
  | _ when n % 5  = 0 -> "Buzz"
  | _ when n % 3  = 0 -> "Fizz"
  | _                 -> string n
```

---

#### Rekursion

```fsharp
 let rec fact n =
  if n = 1 then 1 else
  n * fact (n-1)
```

### Strukturen und Patternmatching

#### Tupel

```fsharp
 let t3 = (1, "Hallo", 3.5)

 let (i,s,f) = t3
```

geht auch mit Konstanten und catchall

```fsharp
 let (1,s,_) = t3
```

`select case` on steroids:

```fsharp
 let test t =
   match t3 with
   | (1,s,_) -> "mit 1 " + s
   | (0,s,f) -> "mit 0 " + s + " und " + string f
   | _       -> "else/default"
``

#### Listen und Sequenzen

```fsharp
 let l = [1; 2; 3; 4; 5]
 let l = [1..5]
 let s = seq [1; 2; 3; 4]
 let s = seq [1..5]
```

##### Head/Tail/Cons

```fsharp
 let l = 1 :: [2..5]
 let l = 1 :: 2 :: 3 :: 4 :: 5 :: []

 let (x::xs) = l

 let f = function
  | [] -> "Leer"
  | (x::xs) -> sprintf "Head: %a, Tail: %A" x xs
```

##### Concat

```fsharp
 [1..5] = [1;2;3] @ [4;5]
```

Rekursiv definiert:

```fsharp
 let rec concat xs ys =
  match xs with
  | [] -> ys
  | (x::xs) -> x :: concatxs ys
```

##### CoinChange

```fsharp
 let rec findChange remCoins remAmount =
  if remAmount = 0 then [] else
  match remCoins with
   | [] ->
    failwith "ich kann darauf nicht zurückgeben"
   | (c::_) when c <= remAmount ->
    c :: findChange remCoins (remAmount - c)
   | (_::cs) ->
    findChange cs remAmount

 let coinChange (amount : int) : Coin list =
  coins = List.sort coins |> List.rev
  findChange coins amount
```

---

#### DUs und algebraische Datentypen

Wie *Enumerationen*

```fsharp
 type Enumeration = A | B | C
```

jeder *Fall* darf aber Werte enthalten (muss aber nicht)

```fsharp
 type T = A of int | B of string | C

 let a = A 5
 let b = B "Hallo"
 let c = C
```

`A`, `B` und `C` heißen *Daten-Konstruktoren* (es gibt auch *Typ-Konstruktoren* die in F# keine entsprechung haben)

##### Warum *algebraisch*?

Fragen:

- Wieviele Elemente hat `bool * bool`?
- Wieviele Elemente hat `type T = A of bool | B of bool*bool | C`?

##### pattern - matching

```fsharp
 let (A i) = a

 match t with
  | A i -> string i
  | B s -> s
  | C   -> "leer"
```

##### dürfen rekursiv sein

```fsharp
 type Expr =
  | Zahl of int
  | Plus of Expr * Expr
  | Mult of Expr * Expr
```

Eval

```fsharp
let rec eval = function
  | Zahl n -> n
  | Plus (a,b) -> eval a + eval b
  | Mult (a,b) -> eval a * eval b
```

##### Option

Ein oft verwendeter Typ

```fsharp
 type 'a Option = None | Some 'a
```

**der** Ersatz für das lästige `Null`

---

## Mehr zu Listen

Im Prinzip kann das alles als Übung freigegeben werden.

### Länge einer Liste

```fsharp
 let rec laenge = function
  | []      -> 0
  | (_::xs) -> 1 + laenge xs
```

### Summe

```fsharp
 let rec summe = function
  | []      -> 0
  | (x::xs) -> x + summe xs
```

### Produkt

```fsharp
 let rec produkt = function
  | []      -> 1
  | (x::xs) -> x * produkt xs
```

### Map

gegeben: `f : 'a -> 'b` und `'a list`
gesucht: `'b list`

```fsharp
 let rec map f = function
  | []      -> []
  | (x::xs) -> f x :: map f xs
```

### Filter

gegeben: Prädikat `p : 'a -> bool` und `ls : 'a list`
gesucht: `'a list` mit Elementen `l` aus `ls` mit `p l = true`

```fsharp
 let rec filter p = function
  | []      -> []
  | (x::xs) ->
   let xs' = filter p xs
   if p x then x :: xs' else xs'
```

### Muster gesehen?

Muster herausarbeiten...

```fsharp
 let rec foldR f s = function
  | []      -> s
  | (x::xs) -> f x (foldR f s xs)
```

### Left-Fold

Analog zu `foldR`:

```fsharp
 let rec foldL f s = function
  | []      -> s
  | (x::xs) -> foldL f (f s x) xs
```

- als `List.fold` enthalten
- `foldR` heißt `List.foldBack` (ein wenig anders definiert)

**Vorteile:**

- *tail-recursive* (Erklären)
- deswegen vorzuziehen

---

## Sequenzen, Rekursion und Kombinatorik

### Sequenz der Teillisten

```fsharp
 let rec sublists = function
  | [] -> Seq.singleton []
  | (x::xs) -> seq {
   for xs' in sublists xs do
    yield xs'
    yield x::xs'
   }
```

### crossProd

```fsharp
let rec crossProd = function
    | []        -> Seq.singleton []
    | (xs::xss) ->
        let xss' = crossProd xss
        seq {
            for x in xs do
            for xs in xss' do
            yield (x::xs)
        }
```

### Sequenz der Permutationen

```fsharp
 let rec selectOne (xs : 'a list) : ('a * 'a list) seq =
  match xs with
  | [] -> Seq.empty
  | (x::xs) ->
   seq {
    yield (x,xs)
    for (x',xs') in selectOne xs do
     yield (x', x::xs')
   }

 let rec permutationen (xs : 'a list) : ('a list) seq =
  match xs with
  | [] -> Seq.singleton []
  | _ ->
   seq {
    for (x,xs) in selectOne xs do
    for xs' in permutationen xs do
    yield x::xs'
   }
```

### Unfold (?)

```fsharp
 let rec unfold next start =
  seq {
   match next start with
   | None ->
    yield! Seq.empty
   | Some (v, n) ->
    yield v
    yield! unfold next n
  }
```

### Iterate mit Unfold

```fsharp
 let iterate f =
  Seq.unfold (fun s -> Some (s, f s))
```

### Map mit Unfold

```fsharp
 let map f =
  Seq.unfold (function
  | []      -> None
  | (x::xs) -> Some (f x, xs))
```

### Zip mit Unfold

```fsharp
 let zip xs ys =
  Seq.unfold
   (function
    | ([],_) | (_,[]) -> None
    | (x::xs,y::ys)   -> Some ((x,y),(xs,ys))
   )
   (xs,ys)
```

Bemerkungen:

- `List.zip`: beide Listen müssen die gleiche Länge haben
- `Seq.zip`: wie oben: bricht ab, wenn eine Sequenz leer wird
- Umwandlung zwischen `List` und `Seq` mit `Seq.toList`, `Seq.ofList`, ...

---

## Wolf-Ziege-Kohl

[Wikipedia](https://en.wikipedia.org/wiki/Wolf,_goat_and_cabbage_problem)

```fsharp
type Fahrgaeste =
    | Wolf
    | Kohl
    | Ziege

let eat (gast: Fahrgaeste) : Fahrgaeste option =
    match gast with
    | Wolf -> Some Ziege
    | Ziege -> Some Kohl
    | Kohl -> None

type Ufer = Fahrgaeste Set

let istUferSicherFuerAlle (ufer: Ufer) : bool =
    ufer
    |> Set.toSeq
    |> Seq.choose eat
    |> Seq.exists (fun opfer -> Set.contains opfer ufer)
    |> not

type Zustand =
    | FarmerLinks of Ufer * Ufer
    | FarmerRechts of Ufer * Ufer

let istZustandSicherFuerAlle (zustand: Zustand) : bool =
    match zustand with
    | FarmerLinks(_, rechtesUfer) -> 
      istUferSicherFuerAlle rechtesUfer
    | FarmerRechts(linkesUfer, _) -> 
      istUferSicherFuerAlle linkesUfer

let start: Zustand = 
  FarmerLinks(Set.ofList [ Wolf; Kohl; Ziege ], Set.empty)

let ziel: Zustand = 
  FarmerRechts(Set.empty, Set.ofList [ Wolf; Kohl; Ziege ])

let fahreAnsAndereUfer 
  (nimmMit: Fahrgaeste option) 
  (zustand: Zustand) : Zustand =
    let transferiereGast gast (von, nach) =
        if not (Set.contains gast von) then
            (von, nach)
        else
            (Set.remove gast von, Set.add gast nach)

    match (zustand, nimmMit) with
    | FarmerLinks(linkesUfer, rechtesUfer), None -> 
      FarmerRechts(linkesUfer, rechtesUfer)
    | FarmerRechts(linkesUfer, rechtesUfer), 
      None -> FarmerLinks(linkesUfer, rechtesUfer)
    | FarmerLinks(linkesUfer, rechtesUfer), Some gast -> 
      FarmerRechts(transferiereGast gast (linkesUfer, rechtesUfer))
    | FarmerRechts(linkesUfer, rechtesUfer), Some gast ->
        FarmerLinks(
          let (rechts, links) = 
            transferiereGast gast (rechtesUfer, linkesUfer) 
          in 
            (links, rechts))

let moeglicheFolgeZustaende (zustand: Zustand) : Zustand seq =
    let moeglicheGaeste =
        match zustand with
        | FarmerLinks(uferLinks, _) -> uferLinks
        | FarmerRechts(_, uferRechts) -> uferRechts
        |> Set.toList
        |> List.map Some
        |> fun gaeste -> None :: gaeste

    seq {
        for gast in moeglicheGaeste do
            let zustand' = fahreAnsAndereUfer gast zustand

            if istZustandSicherFuerAlle zustand' then
                yield zustand'
    }

// Achtung: DFS - würde nicht unbedingt den "kürzesten" Pfad finden
let sucheLoesungen () : (Zustand list) seq =
    let rec go hierWarIchSchon zustand : (Zustand list) seq =
        seq {
            if Set.contains zustand hierWarIchSchon then
                yield! Seq.empty
            else if zustand = ziel then
                yield [ zustand ]
            else
                for zustand' in moeglicheFolgeZustaende zustand do
                    for weg in go (Set.add zustand hierWarIchSchon) zustand' do
                        yield zustand :: weg
        }

    go Set.empty start

printfn $"Lösungen %A{sucheLoesungen ()}"
```
