Imports System
Imports Microsoft.VisualBasic


Module Module1

    Const NO_KEY = 0
    Const CURSOR_LEFT = 1
    Const CURSOR_RIGHT = 2
    Const UNKNOWN_KEY = 99
    Const PAUSE_TASTE = 3

    Const SPALTE_MAX = 79
    Const ZEILE_MAX = 24
    Const A_MIN = 1
    Const A_MAX_Start = 2
    Const G_MIN = 1
    Const G_MAX = 9
    Const P_MIN = 0
    Const P_MAX = SPALTE_MAX

    Const BEWEGUNG_SPIELFIGUR = 10

    Const MENUE_SPALTE = 5
    Const MENUE_ZEILE = 3
    'Const INHALT_SPALTE = 36
    'Const INHALT_ZEILE = 3

    'Globale Variablen:
    Dim v_spielername As String
    Dim v_schwierigkeit As String
    Dim v_sound_an As Boolean

    'Liste für alle Spieler
    Dim v_spielerNamen As New List(Of String)
    Dim v_spielerScores As New List(Of Integer)
    Dim v_spielerDatum As New List(Of String)

    'Speicherdatei
    Dim v_datei_spieler As String = "spieler.txt"

    Function Tastatur_Abfrage() As Integer
        Dim cki As New ConsoleKeyInfo()
        If Console.KeyAvailable = False Then
            Return NO_KEY
        Else
            cki = Console.ReadKey(True)
            If cki.Key = ConsoleKey.LeftArrow Then
                Return CURSOR_LEFT
            ElseIf cki.Key = ConsoleKey.RightArrow Then
                Return CURSOR_RIGHT
            ElseIf cki.Key = ConsoleKey.P Then
                Return PAUSE_TASTE
            Else
                Return UNKNOWN_KEY
            End If
        End If
    End Function

    Sub ZeilenErzeugung(ByRef Zeile() As Char, ByVal a_max As Integer)
        'Deklarieren der Variablen
        Dim a As Integer    'Anzahl der Hindernisblocks
        Dim X As Single
        Dim i As Integer
        Dim g As Integer    'Größe des Hindernisblocks
        Dim p As Integer    'Position des Hindernisblocks


        'Zeilenvektor mit Leerzeichen füllen:
        For i = 0 To SPALTE_MAX
            Zeile(i) = " "
        Next

        'Anzahl A der HIndernisblocks zufällig ermitteln:
        Randomize()
        X = VBMath.Rnd

        a = (a_max - A_MIN) * X + A_MIN
        'Console.WriteLine(A)

        'Für jeden der A Hindernisblocks:
        For i = 1 To a

            'Größe G des Hindernisblocks zufällig ermitteln:
            Randomize()
            X = VBMath.Rnd

            g = (G_MAX - G_MIN) * X + G_MIN
            'console.WriteLine("G: " & G)

            'Startposition P des Hindernisblocks zufällig ermitteln:
            Randomize()
            X = VBMath.Rnd

            p = (P_MAX - P_MIN) * X + P_MIN
            'Console.WriteLine("P: " & P)

            'Für jedes der G Einzelhindernisse:
            For j = 1 To g

                'Prüfen ob Hinderniss innerhalb des Wertebereichs ist
                If p + j - 1 <= SPALTE_MAX Then

                    'Hinderniss an Position P+j-1 in den Zeilenvektor eintragen
                    Zeile(p + j - 1) = "X"
                End If
            Next
        Next
    End Sub

    Sub GameOver(ByVal punkte As Integer)

        Dim game_over_text As ConsoleKeyInfo
        Console.BackgroundColor = ConsoleColor.DarkBlue

        Console.Clear()

        Console.SetCursorPosition(0, ZEILE_MAX / 2 - 5)

        Console.WriteLine(" _____                  _____             ")
        Console.WriteLine("|   __|___ _____ ___   |     |_ _ ___ ___ ")
        Console.WriteLine("|  |  | .'|     | -_|  |  |  | | | -_|  _|")
        Console.WriteLine("|_____|__,|_|_|_|___|  |_____|\_/|___|_|  ")

        Console.WriteLine()
        Console.WriteLine("Du hast leider verloren," & v_spielername & "!")
        Console.WriteLine("Dein Punktestand: " & punkte)
        'Highscore aktualisieren:
        Dim index As Integer = v_spielerNamen.IndexOf(v_spielername)

        If index >= 0 Then
            If punkte > 0 And punkte > v_spielerScores(index) Then
                v_spielerScores(index) = punkte
                v_spielerDatum(index) = DateTime.Now.ToString("dd.MM.yyyy")
                SpeichereSpieler()
            End If
        End If

        Console.WriteLine("Schwierigkeit: " & v_schwierigkeit)
        Console.WriteLine()
        Console.WriteLine("[1] Nochmal spielen")
        Console.WriteLine("[2] Zum Hauptmenü")

        'Benutzereingabe einlesen
        game_over_text = Console.ReadKey(True)
        If game_over_text.KeyChar = "1" Then
            Spielablauf()
            Exit Sub
        ElseIf game_over_text.KeyChar = "2" Then
            Hauptmenue()
            Exit Sub
        End If
    End Sub

    Sub Startbildschirm()
        Console.BackgroundColor = ConsoleColor.DarkCyan
        Console.ForegroundColor = ConsoleColor.White

        Console.Clear()

        Console.SetCursorPosition(0, 0)

        Console.WriteLine(" _______  __   __  _______  _______  ______    ")
        Console.WriteLine("|       ||  | |  ||       ||       ||    _ |   ")
        Console.WriteLine("|  _____||  | |  ||    _  ||    ___||   | ||   ")
        Console.WriteLine("| |_____ |  |_|  ||   |_| ||   |___ |   |_||_  ")
        Console.WriteLine("|_____  ||       ||    ___||    ___||    __  | ")
        Console.WriteLine(" _____| ||       ||   |    |   |___ |   |  | | ")
        Console.WriteLine("|_______||_______||___|    |_______||___|  |_| ")


        Console.WriteLine(" __   __  _______  ______    ___   _______ ")
        Console.WriteLine("|  |_|  ||   _   ||    _ |  |   | |       |")
        Console.WriteLine("|       ||  |_|  ||   | ||  |   | |   _   |")
        Console.WriteLine("|       ||       ||   |_||_ |   | |  | |  |")
        Console.WriteLine("|       ||       ||    __  ||   | |  |_|  |")
        Console.WriteLine("| ||_|| ||   _   ||   |  | ||   | |       |")
        Console.WriteLine("|_|   |_||__| |__||___|  |_||___| |_______|")

        Console.WriteLine(" _______  __   __  ______    __   __  ___   __   __  _______  ___     ")
        Console.WriteLine("|       ||  | |  ||    _ |  |  | |  ||   | |  | |  ||   _   ||   |    ")
        Console.WriteLine("|  _____||  | |  ||   | ||  |  |_|  ||   | |  |_|  ||  |_|  ||   |    ")
        Console.WriteLine("| |_____ |  |_|  ||   |_||_ |       ||   | |       ||       ||   |    ")
        Console.WriteLine("|_____  ||       ||    __  ||       ||   | |       ||       ||   |___ ")
        Console.WriteLine(" _____| ||       ||   |  | | |     | |   |  |     | |   _   ||       |")
        Console.WriteLine("|_______||_______||___|  |_|  |___|  |___|   |___|  |__| |__||_______|")

        Console.ReadLine()
    End Sub


    Sub SpielerAuswahl()
        Dim spieler_auswahl As ConsoleKeyInfo
        Do
            Console.Clear()

            Console.SetCursorPosition(SPALTE_MAX / 2, 0)

            Console.WriteLine("___________SPIELERAUSWAHL____________")
            Console.WriteLine()

            Console.WriteLine("[1] Neuer Spieler")
            Console.WriteLine()

            Console.WriteLine("[2] Vorhandenen Spieler wählen")
            Console.WriteLine()

            'Benutzereingabe einlesen:
            spieler_auswahl = Console.ReadKey(True)

            'Benutzereingabe prüfen und entsprechend reagieren:
            If spieler_auswahl.KeyChar = "1" Then
                Console.Clear()

                Console.WriteLine("Geben Sie Ihren Namen ein:")
                Dim name As String = Console.ReadLine()

                'Prüfen auf existenz des Namens
                If v_spielerNamen.Contains(name) Then
                    Console.WriteLine("Dieser Spieler existiert bereits.")
                    Console.ReadKey(True)
                    Continue Do
                End If

                'Neuer Spieler anlegen:
                v_spielerNamen.Add(name)
                v_spielerScores.Add(0)
                v_spielerDatum.Add(DateTime.Now.ToString("dd.MM.yyyy"))

                v_spielername = name
                Console.WriteLine()
                Console.WriteLine("Willkommen, " & v_spielername & "!")
                Console.WriteLine()
                Console.WriteLine("Zum Fortfahren Enter drücken")
                Console.ReadKey(True)

                Exit Do

            ElseIf spieler_auswahl.KeyChar = "2" Then
                Console.Clear()

                If v_spielerNamen.Count = 0 Then
                    Console.WriteLine("Es sind keine Spieler vorhanden. Bitte legen Sie einen neuen Spieler an.")
                    Console.ReadKey(True)
                    Continue Do
                End If

                Console.WriteLine("Hinterlegte Spieler:")
                Console.WriteLine()

                For i = 0 To v_spielerNamen.Count - 1
                    Console.WriteLine("[" & i + 1 & "] " & v_spielerNamen(i))
                Next

                Console.WriteLine()
                Console.WriteLine("Geben Sie die Nummer des gewünschten Spielers ein:")

                Dim eingabe As String = Console.ReadLine()
                Dim nr As Integer

                If Integer.TryParse(eingabe, nr) Then
                    nr -= 1
                    If nr >= 0 And nr < v_spielerNamen.Count Then
                        v_spielername = v_spielerNamen(nr)
                    Else
                        Console.WriteLine("Ungültige Nummer. Bitte wählen Sie eine gültige Nummer.")
                        Console.ReadKey(True)
                        Continue Do
                    End If
                Else
                    Console.WriteLine("Ungültige Eingabe. Bitte wählen Sie eine gültige Option.")
                    Console.ReadKey(True)
                    Continue Do
                End If

                Console.WriteLine("Zum Fortfahren Enter drücken")
                Console.ReadKey(True)

                Exit Do
            End If
        Loop
    End Sub

    Sub Hauptmenue()
        Dim menü_auswahl As ConsoleKeyInfo
        Dim schwierigkeit_auswahl As ConsoleKeyInfo
        Do
            Console.Clear()

            'Titel des Spiels ausgeben:
            Console.SetCursorPosition(SPALTE_MAX / 2 - 5, 0)
            Console.WriteLine("___________SUPER MARIO SURVIVAL__________")

            'Menueoptionen ausgeben:
            Console.SetCursorPosition(MENUE_SPALTE, MENUE_ZEILE)
            Console.WriteLine(" ---------------------------- ")
            Console.SetCursorPosition(MENUE_SPALTE, MENUE_ZEILE + 1)
            Console.WriteLine("|           MENÜ             |")
            Console.SetCursorPosition(MENUE_SPALTE, MENUE_ZEILE + 2)
            Console.WriteLine(" ---------------------------- ")

            Console.SetCursorPosition(MENUE_SPALTE + 2, MENUE_ZEILE + 4)
            Console.WriteLine("1. Spiel starten")

            Console.SetCursorPosition(MENUE_SPALTE + 2, MENUE_ZEILE + 5)
            Console.WriteLine("2. Anleitung")

            Console.SetCursorPosition(MENUE_SPALTE + 2, MENUE_ZEILE + 6)
            Console.WriteLine("3. Highscores")

            Console.SetCursorPosition(MENUE_SPALTE + 2, MENUE_ZEILE + 7)
            Console.WriteLine("4. Einstellungen")

            Console.SetCursorPosition(MENUE_SPALTE + 2, MENUE_ZEILE + 8)
            Console.WriteLine("5. Charakter wechseln")

            Console.SetCursorPosition(MENUE_SPALTE + 2, MENUE_ZEILE + 9)
            Console.WriteLine("6. Spiel verlassen")

            Console.SetCursorPosition(MENUE_SPALTE, MENUE_ZEILE + 11)
            Console.WriteLine(" ---------------------------- ")

            'Benutzereingabe einlesen
            menü_auswahl = Console.ReadKey(True)

            If menü_auswahl.KeyChar = "1" Then
                Do
                    'Schwerigkeitliste anzeigen
                    Console.SetCursorPosition(0, 18)

                    Console.WriteLine("SCHWIERIGKEITSAUSWAHL:")
                    Console.WriteLine()

                    Console.WriteLine("[1] Easy")
                    Console.WriteLine("[2] Medium")
                    Console.WriteLine("[3] Hard")

                    'Benutzereingabe einlesen
                    schwierigkeit_auswahl = Console.ReadKey(True)

                    'Auswahl der Schwierigkeit prüfen und in Variable speichern
                    If schwierigkeit_auswahl.KeyChar = "1" Then
                        v_schwierigkeit = "Easy"
                        Exit Do

                    ElseIf schwierigkeit_auswahl.KeyChar = "2" Then
                        v_schwierigkeit = "Medium"
                        Exit Do

                    ElseIf schwierigkeit_auswahl.KeyChar = "3" Then
                        v_schwierigkeit = "Hard"
                        Exit Do

                    Else Console.WriteLine("Ungültige Eingabe. Bitte wählen Sie eine gültige Schwierigkeit.")
                        Console.ReadKey(True)
                    End If
                Loop

                'Spiel starten:
                Spielablauf()
            End If

            'Anleitung aufrufen
            If menü_auswahl.KeyChar = "2" Then
                Anleitung()
            End If

            'Highscore liste aufrufen:
            If menü_auswahl.KeyChar = "3" Then
                Highscores()
            End If

            'Einstellungen aufrufen:
            If menü_auswahl.KeyChar = "4" Then
                Einstellungen()
            End If

            'Charakter wechseln
            If menü_auswahl.KeyChar = "5" Then
                SpielerAuswahl()
            End If

            'Spiel verlassen
            If menü_auswahl.KeyChar = "6" Then
                Console.Clear()
                Console.WriteLine("Danke fürs Spielen! Auf Wiedersehen!")
                Console.ReadLine()
                End
            End If
        Loop
    End Sub

    Sub Anleitung()
        Console.SetCursorPosition(0, 18)

        Console.WriteLine("__ANLEITUNG__")
        Console.WriteLine()

        Console.WriteLine("Ziel:")
        Console.WriteLine("::::::::::")
        Console.WriteLine()

        Console.WriteLine("Steuerung:")
        Console.WriteLine("::::::::::")
        Console.WriteLine()

        Console.WriteLine("Zum Zurueckkehren Enter drücken")
        Console.ReadLine()
    End Sub

    Sub Highscores()
        Console.Clear()
        Console.WriteLine("HIGHSCORES:")
        Console.WriteLine()

        Console.SetCursorPosition(0, 18)

        If v_spielerNamen.Count = 0 Then
            Console.WriteLine("Es sind noch keine Highscores vorhanden.")
            Console.ReadLine()
            Exit Sub
        End If
        'Sortieren mit bubble Sort:

        For i = 0 To v_spielerScores.Count - 2
            For j = 0 To v_spielerScores.Count - 2 - i
                If v_spielerScores(j) < v_spielerScores(j + 1) Then
                    'Punkte tauschen
                    Dim tempScore As Integer = v_spielerScores(j)
                    v_spielerScores(j) = v_spielerScores(j + 1)
                    v_spielerScores(j + 1) = tempScore
                    'Namen tauschen
                    Dim tempName As String = v_spielerNamen(j)
                    v_spielerNamen(j) = v_spielerNamen(j + 1)
                    v_spielerNamen(j + 1) = tempName
                    'Datum tauschen
                    Dim tempDatum As String = v_spielerDatum(j)
                    v_spielerDatum(j) = v_spielerDatum(j + 1)
                    v_spielerDatum(j + 1) = tempDatum
                End If
            Next
        Next
        For i = 0 To v_spielerNamen.Count - 1
            If v_spielerScores(i) > 0 Then
                Console.WriteLine(v_spielerNamen(i) & " - " & v_spielerScores(i) & " Punkte - " & v_spielerDatum(i))
            End If
        Next
        Console.WriteLine()
        Console.WriteLine("Zum Zurueckkehren Enter drücken")
        Console.ReadLine()
    End Sub

    Sub Einstellungen()

        Dim einstellungs_auswahl As ConsoleKeyInfo

        Console.SetCursorPosition(0, 18)

        Console.WriteLine("EINSTELLUNGEN:")
        Console.WriteLine()

        'Anzeigen ob Sound an oder aus
        If v_sound_an = True Then
            Console.WriteLine("Aktueller Status: SOUND AKTIV")

        Else Console.WriteLine("Aktueller Status: SOUND AUS")
        End If
        Console.WriteLine()

        'Menü zur Soundeinstellung
        Console.WriteLine("[1] Sound AN")
        Console.WriteLine("[2] Sound AUS")
        Console.WriteLine()

        'Benutzereingabe einlesen
        einstellungs_auswahl = Console.ReadKey(True)

        'Auswahl prüfen und entsprechend reagieren
        If einstellungs_auswahl.KeyChar = "1" Then
            v_sound_an = True
            Console.WriteLine("Sound ist AN.")
        ElseIf einstellungs_auswahl.KeyChar = "2" Then
            v_sound_an = False
            Console.WriteLine("Sound ist AUS.")
        Else Console.WriteLine("Ungültige Eingabe. Bitte wählen Sie eine gültige Option.")
        End If

        Console.WriteLine()
        Console.WriteLine("Zum Zurueckkehren Enter drücken")
        Console.ReadLine()
    End Sub

    Sub Spielablauf()
        Dim leben As Integer
        Dim spielfeld(ZEILE_MAX, SPALTE_MAX) As Char
        Dim zeile(SPALTE_MAX) As Char
        Dim z As Integer
        Dim s As Integer
        Dim taste As Integer
        Dim spielfigur_spalte As Integer
        Dim wartezeit As Single
        Dim a_max As Single
        Dim punkte As Integer
        Dim k As Integer

        'Startwerte setzen
        leben = 5
        punkte = 0
        spielfigur_spalte = SPALTE_MAX \ 2
        wartezeit = 200
        a_max = A_MAX_Start

        'Schwierigkeitseinstellung anpassen:
        If v_schwierigkeit = "Medium" Then
            wartezeit = wartezeit - 20
            a_max = a_max + 1
        ElseIf v_schwierigkeit = "Hard" Then
            wartezeit = wartezeit - 40
            a_max = a_max + 2
        End If

        'Countdown vor Spielbeginn:
        Console.Clear()
        Console.SetCursorPosition(SPALTE_MAX / 2 - 5, ZEILE_MAX / 2)
        Console.WriteLine("READY?")
        Console.ReadLine()
        Console.Clear()

        Console.SetCursorPosition(SPALTE_MAX / 2 - 5, ZEILE_MAX / 2)
        Console.WriteLine("3")
        Threading.Thread.Sleep(1000)
        Console.Clear()

        Console.SetCursorPosition(SPALTE_MAX / 2 - 5, ZEILE_MAX / 2)
        Console.WriteLine("2")
        Threading.Thread.Sleep(1000)
        Console.Clear()

        Console.SetCursorPosition(SPALTE_MAX / 2 - 5, ZEILE_MAX / 2)
        Console.WriteLine("1")
        Threading.Thread.Sleep(1000)
        Console.Clear()

        Console.SetCursorPosition(SPALTE_MAX / 2 - 5, ZEILE_MAX / 2)
        Console.WriteLine("GO!")
        Threading.Thread.Sleep(1000)
        Console.Clear()

        'Hauptschleife des Spiels
        Do
            'neue Zeile erzeugen
            ZeilenErzeugung(Zeile, a_max)

            'Alle Zeilen des Spielfelds um eine Zeile nach unten verschieben
            'Rückwärtschleife über Zeilen
            For z = ZEILE_MAX To 1 Step -1
                'Vorwärtschleife über Spalten
                For s = 0 To SPALTE_MAX
                    'Eine Zelle nach unten kopieren
                    spielfeld(z, s) = spielfeld(z - 1, s)

                Next
            Next
            'Neue Zeile am oberen Rand des Spielfelds einfügen
            For s = 0 To SPALTE_MAX
                spielfeld(0, s) = Zeile(s)
            Next

            'Player auf der Konsole ausgeben
            Console.SetCursorPosition(0, 0)
            Console.Write("Player: " & v_spielername)

            'Informationen zur Schwierigkeitausgeben:
            If v_schwierigkeit = "Easy" Then
                Console.ForegroundColor = ConsoleColor.Green
            ElseIf v_schwierigkeit = "Medium" Then
                Console.ForegroundColor = ConsoleColor.Yellow
            ElseIf v_schwierigkeit = "Hard" Then
                Console.ForegroundColor = ConsoleColor.Red
            End If
            Console.Write("   Schwierigkeit: " & v_schwierigkeit)
            Console.ForegroundColor = ConsoleColor.White

            'Informationen zum Punktestand ausgeben:
            Console.Write("   Punkte:" & punkte)

            'Spielfeld auf der Konsole ausgeben:
            For z = 0 To ZEILE_MAX - 2
                For s = 0 To SPALTE_MAX
                    Console.Write(spielfeld(z, s))
                Next
                Console.WriteLine()
            Next

            'Zählschleife für schnelle Bewegung:
            For i = 1 To BEWEGUNG_SPIELFIGUR
                'Tastatur abfragen:
                taste = Tastatur_Abfrage()
                'Console.WriteLine(taste)

                'alte Spielfigur löschen:
                Console.SetCursorPosition(spielfigur_spalte, ZEILE_MAX - 1)  'Zeile und Spalte anders als bei Matrix
                Console.Write(" ")

                'Position der Spielfigur berechnen:
                If taste = CURSOR_LEFT Then
                    spielfigur_spalte = spielfigur_spalte - 1
                    If spielfigur_spalte < 0 Then
                        spielfigur_spalte = 0
                    End If
                End If

                If taste = CURSOR_RIGHT Then
                    spielfigur_spalte = spielfigur_spalte + 1
                    If spielfigur_spalte > SPALTE_MAX Then
                        spielfigur_spalte = SPALTE_MAX
                    End If
                End If

                'Spielfigur auf der Konsole ausgeben: 
                Console.SetCursorPosition(spielfigur_spalte, ZEILE_MAX - 1)  'Zeile und Spalte anders als bei Matrix
                Console.Write("☻")

                'Kollision prüfen:
                If spielfeld(ZEILE_MAX - 2, spielfigur_spalte) = "X" Then
                    'Kollision erkannt:
                    leben = leben - 1

                    'Sound abspielen:
                    Console.Beep()

                    'Bildschirm rot aufblitzen lassen:
                    Console.BackgroundColor = ConsoleColor.Red
                    Console.Clear()
                    Threading.Thread.Sleep(10)
                    Console.Clear()
                    Console.BackgroundColor = ConsoleColor.DarkCyan

                    'Hindernis löschen:
                    spielfeld(22, spielfigur_spalte) = " "
                End If

                'Anzeige der Leben-Anzahl:
                Console.SetCursorPosition(0, ZEILE_MAX - 1)
                Console.Write("Leben: ")
                For k = 1 To leben
                    Console.Write("♥")
                Next

                'Warten
                Threading.Thread.Sleep(wartezeit / BEWEGUNG_SPIELFIGUR)
            Next

            'Pausefunktion:
            If taste = PAUSE_TASTE Then
                Console.SetCursorPosition(SPALTE_MAX / 2 - 3, ZEILE_MAX / 2)
                Console.WriteLine("PAUSIERT")

                Console.SetCursorPosition(0, ZEILE_MAX - 1)
                Console.Write("Zum Fortfahren P drücken")

                ''Tastaturpuffer leeren:
                'Do
                '    taste = Tastatur_Abfrage()
                'Loop Until taste = NO_KEY

                'Auf P warten:
                Do
                    taste = Tastatur_Abfrage()
                Loop Until taste = PAUSE_TASTE

                'Pausetext löschen:
                Console.SetCursorPosition(SPALTE_MAX / 2 - 3, ZEILE_MAX / 2)
                Console.WriteLine("         ")
                Console.SetCursorPosition(0, ZEILE_MAX - 1)
                Console.Write("                          ")
            End If

            'Tastaturpuffer leeren:
            Do
                taste = Tastatur_Abfrage()
            Loop Until taste = NO_KEY

            'Wartezeit verringern:
            wartezeit = wartezeit * 0.99
            If wartezeit < 0 Then wartezeit = 0
            'Console.SetCursorPosition(15, ZEILE_MAX)
            'Console.Write("Wartezeit: " & wartezeit)

            'Hindernísdichte erhöhen:
            a_max = a_max * 1.03
            'Console.SetCursorPosition(38, ZEILE_MAX)
            'Console.Write("Hindernisdichte: " & a_max)

            'Punkte erhöhen:
            punkte = punkte + 1

        Loop Until leben <= 0

        GameOver(punkte)
    End Sub

    Sub LadeSpieler()
        If IO.File.Exists(v_datei_spieler) = False Then Exit Sub

        Dim zeilen() As String = IO.File.ReadAllLines(v_datei_spieler)

        For Each z In zeilen
            Dim teile() As String = z.Split(";"c)
            If teile.Length = 3 Then
                v_spielerNamen.Add(teile(0))
                v_spielerScores.Add(CInt(teile(1)))
                v_spielerDatum.Add(teile(2))
            End If
        Next
    End Sub

    Sub SpeichereSpieler()
        Dim liste As New List(Of String)

        For i = 0 To v_spielerNamen.Count - 1
            liste.Add(v_spielerNamen(i) & ";" & v_spielerScores(i) & ";" & v_spielerDatum(i))
        Next

        IO.File.WriteAllLines(v_datei_spieler, liste.ToArray())
    End Sub
    Sub Main()

        'Konsolenfenster einstellen:


        'Cursor unsichtbar machen:
        Console.CursorVisible = False

        'Sound an:
        v_sound_an = True

        LadeSpieler()

        Startbildschirm()
        SpielerAuswahl()
        Hauptmenue()

    End Sub
End Module