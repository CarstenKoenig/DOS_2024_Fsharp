module WolfZiegeKohl

// linkes Ufer, rechts Ufer
// links: Farmer + Kohl, Ziege, Wolf
// Boot: Platz für Farmer + Ziege oder Kohl oder Wolf
// alle ans linke Ufer bringen ohne das wer gefressen wird
type Fahrgast =
    | Wolf
    | Ziege
    | Kohl

// option - Some wert | None

let frisst (gast: Fahrgast) : Fahrgast option =
    match gast with
    | Wolf -> Some Ziege
    | Ziege -> Some Kohl
    | Kohl -> None

type Ufer = Set<Fahrgast>

let istUferSicherFuerAlle (ufer: Ufer) : bool =
    ufer
    |> Set.toSeq
    |> Seq.choose frisst
    |> Seq.exists (fun futter -> Set.contains futter ufer)
    |> not

type Zustand =
    | FarmerLinks of Ufer * Ufer
    | FarmerRechts of Ufer * Ufer

let istSicherFuerAlle zustand =
    match zustand with
    | FarmerLinks(_, rechte) -> istUferSicherFuerAlle rechte
    | FarmerRechts(links, _) -> istUferSicherFuerAlle links

let start: Zustand = FarmerLinks(Set.ofList [ Wolf; Ziege; Kohl ], Set.empty)

let ziel: Zustand = FarmerRechts(Set.empty, Set.ofList [ Wolf; Ziege; Kohl ])

let fahreAnsAndereUfer (nimmMit: Fahrgast option) (zustand: Zustand) =
    let transfiereGast gast vonUfer nachUfer =
        if Set.contains gast vonUfer then
            (Set.remove gast vonUfer, Set.add gast nachUfer)
        else
            (vonUfer, nachUfer)

    match (zustand, nimmMit) with
    | (FarmerLinks(links, rechts), None) -> FarmerRechts(links, rechts)
    | (FarmerRechts(links, rechts), None) -> FarmerLinks(links, rechts)
    | (FarmerLinks(links, rechts), Some gast) ->
        let (links', rechts') = transfiereGast gast links rechts
        FarmerRechts(links', rechts')
    | (FarmerRechts(links, rechts), Some gast) ->
        let (rechts', links') = transfiereGast gast rechts links
        FarmerLinks(links', rechts')

let moeglicheFolgeZustaende (zustand: Zustand) : Zustand seq =
    let moeglicheGaeste =
        match zustand with
        | FarmerLinks(links, _) -> links
        | FarmerRechts(_, rechts) -> rechts
        |> Set.toList
        |> List.map Some
        |> fun gaeste -> None :: gaeste

    moeglicheGaeste
    |> List.map (fun nimmMit -> fahreAnsAndereUfer nimmMit zustand)
    |> List.filter istSicherFuerAlle
    |> List.toSeq

let sucheLoesungen () : List<Zustand> seq =
    let rec go hierWarIchSchon zustand =
        seq {
            if Set.contains zustand hierWarIchSchon then
                yield! Seq.empty
            elif zustand = ziel then
                yield [ ziel ]
            else
                for zustand' in moeglicheFolgeZustaende zustand do
                    for weg in go (Set.add zustand hierWarIchSchon) zustand' do
                        yield zustand :: weg
        }

    go Set.empty start
