open System
// Github: https://github.com/CarstenKoenig/DOS_2024_Fsharp

Console.WriteLine("Hallo")

printfn "Hallo DevOpenSpace"

let zahl = 1
let mutable veraendlicheZahl = 1
veraendlicheZahl <- 2

let text = "Text..."
let character = 'c'
let wahrheit = true

type Zahl = Int
type Vorname = String
type Nachname = String

let carstenNachname: Nachname = "König"
let carstenVorname: Vorname = carstenNachname

// Unit
let unitWert = ()

// Bottom
// - Exception / Endlosschleife
// let bottom1 : int = raise (new Exception "Test")

// --- Funktion ---
// Idee Funktion f: bekommt Wert x -> Kommt Wert y
// Gegenbeispiel: DateTime.Now
// Warum: f(x) <-> Ersten mit den Wert von x (referentielle Transparenz)

let inkrement n = n + 1
// inkrement 10
// = 10 + 1
// = 11

let dekrement n =
    // body
    printfn "%d" n
    n - 1

let verdopple n = 2 * n

let hallo name = "Hallo " + name

let inkrement2 n =
    let ink x = x + 1
    ink (ink n)

// Lambda-Funktion mit fun
let inkrement3 = fun n -> n + 1

let zehn = 10
let inkrement10 = fun n -> n + zehn

// Funktion: erst verdopple Wert und dann das Ergebnis inkrementiert
let f1 n = inkrement (verdopple n)

let f2 n =
    // Pipe
    n |> verdopple |> inkrement

let f3 n = inkrement <| (verdopple <| n)

// point-free
let f4 = verdopple >> inkrement

let f5 = inkrement << verdopple

// Funktionen mit mehreren Paramtern
// eigentlich: Funktion Eingabe -> Ausgabe

let tuple = (4, "Hallo")

// in imperativen Sprachen üblich: Tupel
let addition (x, y) = x + y

// Currying (Haskell Curry)
// Funktion addition : bekommt Summand1 und liefert neue Funktion
// neue Funktion bekommt Summand2 und liefert dann Summand1+Summand2

let additionCurry2 = fun x -> (fun y -> x + y)
// C#: Func<int, Func<int, int>> additionCurry = x => (y => x + y)
// (additionCurry2 2) 3
// = (fun y -> 2 + y) 3
// = 2 + 3
// = 5

let additionCurry x y = x + y

// "partitielle Anwendung" geschenkt
let inkrementMitPartiellerAnwendung: int -> int = additionCurry 1

let vier = 2 |> additionCurry 2

// if-Expression
// ähnlich b ? t : e
let ifBeispiel =
    // if
    if 4 > 5 then "größer" else "kleiner"

let ifBeispielUnit =
    if 4 > 5 then
        (printfn "kleiner")

// FizzBuzz
// 1 2 Fizz 4 Buzz ... 14 FizzBuzz
// fizzBuzz : n -> String

let fizzBuzz (n: int) : string =
    if n % 15 = 0 then "FizzBuzz"
    elif n % 5 = 0 then "Buzz"
    elif n % 3 = 0 then "Fizz"
    else string n

let fizzBuzz2 (n: int) : string =
    match (n % 3 = 0, n % 5 = 0) with
    | (true, true) -> "FizzBuzz"
    | (true, false) -> "Fizz"
    | (false, true) -> "Buzz"
    | _ -> string n

let tuple2 = (1, "String", 4.5)
let (tInt, tString, tDouble) = tuple2

let matchAusdruck =
    match tuple2 with
    | (2, s, _) -> s
    | (1, s, d) when d > 0.0 -> s + "+"
    | (_, s, _) -> s

let betrag n =
    match n with
    | _ when n < 0 -> -1 * n
    | _ -> n

// Listen
// mehere Elemente alle gleicher Typ
let liste1 = [ 1; 2; 3; 4 ]
let liste2 = [ 1..4 ]
let liste3 = [ 1..2..10 ]
// cons-Liste/Operator
let liste4 = 1 :: (2 :: (3 :: (4 :: [])))

type MyConsList<'a> =
    | EmptyList
    | Cons of 'a * MyConsList<'a>

let rec laenge liste =
    match liste with
    | [] -> 0
    | h :: tl -> 1 + laenge tl

let rec summe liste =
    match liste with
    | [] -> 0
    | h :: tl -> h + summe tl

let rec map (f: 'a -> 'b) (liste: list<'a>) : list<'b> =
    match liste with
    | [] -> []
    | h :: tl -> f h :: map f tl

let rec fold zusammenfassen wertFuerLeer liste =
    match liste with
    | [] -> wertFuerLeer
    | h :: tl -> zusammenfassen h (fold zusammenfassen wertFuerLeer tl)

// coin-change
// Funktion: Betrag in ct -> Liste von Münzen, Summe = Betrag
// 3.15 -> 200 + 100 + 10 + 5

type Coin = int
let muenzen = [ 200; 100; 50; 20; 10; 5; 2; 1 ]

let wechsleBetrag (betrag: int) : List<Coin> =
    let rec wechsle restBetrag ms =
        match ms with
        | _ when restBetrag <= 0 -> []
        | [] -> failwith "sollte nicht vorkommen"
        | muenze :: _ when muenze <= restBetrag -> muenze :: wechsle (restBetrag - muenze) ms
        | _ :: restMuenzen -> wechsle restBetrag restMuenzen

    wechsle betrag muenzen

// Algebraische Datentypen

type Farbe =
    | Rot
    | Gruen
    | Blau

type Datentyp =
    | Konstante
    | EineZahl of int
    | EinTuple of bool * string

    member this.Art =
        match this with
        | Konstante -> 1
        | EineZahl _ -> 2
        | EinTuple _ -> 3

type Name = Name of string

type DU =
    | D1 of int
    | D2 of int * string

type DU3 =
    | D11
    | D21 of string

type DU2 = int * DU3

type Expr =
    | Const of int
    | Add of Expr * Expr
    | Mult of Expr * Expr

// (2+3)*5
let beispiel = Mult(Add(Const 2, Const 3), Const 5)

// eval beispiel = 25
let rec eval (expr: Expr) : int =
    match expr with
    | Const n -> n
    | Add(a, b) -> eval a + eval b
    | Mult(a, b) -> eval a * eval b


let loesungen = WolfZiegeKohl.sucheLoesungen ()
printfn "%A" loesungen

type ITest =
    abstract Test: name: string -> string

(*
interface ITest
{
    public string Test(string name)
}
*)

type Klasse(nummer: int) =
    // noch Ctor/eventuell Felder
    let nummer2 = nummer + 5
    do printfn "Hallo"

    // Member
    new() = Klasse(5)
    member this.Nummer = nummer

    member this.Nummer2 = nummer2
    static member Create() = Klasse()

    member this.Test(name: string) : string = "Hallo " + name

    interface ITest with
        member this.Test(name: string) = this.Test(name)

let test = Klasse(5)
let res = (test :> ITest).Test("x")
