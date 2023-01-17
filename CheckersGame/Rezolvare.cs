using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleCheckers
{
    public partial class Board
    {
        /// <summary>
        /// Calculeaza functia de evaluare statica pentru configuratia (tabla) curenta
        /// </summary>
        public double EvaluationFunction()
        {
            // cu cat functia aceasta este mai "inteligenta", cu atat calculatorul va juca mai bine

            // consideram tabla de joc impartita in doua, sus sunt piesele computerului, jos cele ale omului

            // functia de evaluare depinde de computer
            // daca functia are o valoare mare, inseamna ca este un avantaj pentru computer
            // o valoare mai mica a functiei inseamna un avantaj pentru om

            // la fiecare mutare, i se va adauga computerului o pondere, a carei valoare se retine in variabila declarata
            // cu cat o piesa a computerului este mai aproape de linia finala de pe jumatatea omului (deci sanse mai mari), ponderea va avea o valoare mai mare
            // daca exista un dezavantaj pentru computer, ponderea adaugata va fi mai mica

            // dupa cum spuneam anterior, considerand tabla de joc impartita in jumatate, daca se afla computerul in jumatatea de tabla corespunzatoare omului
            // ponderea primita va fi mai mare pentru ca acesta se afla in avantaj si se apropie de linia finala
            // pe de alta parte, daca omul avanseaza spre calculator iar computerul se afla ramas in jumatatea lui, ponderea atribuita este mai mica

            // de mentionat este faptul ca ponderile sunt alese aleator, avand in vedere sa fie atribuite in mod corespunzator:
            // avantaj = pondere mai mare, dezavantaj = pondere mai mica

            // tinand cont de aceste ponderi, se poate stabili cine este in avantaj (pe baza valorii curente a functiei de evaluare statica) si are sanse sa castige

            // de asemenea, toate mutarile si calculele vor fi facute luand in considerare pozitia pe baza coordonatelor X si Y ale pieselor

            int pondere = 0; // initailizam cu 0 pentru ca este inceputul jocului
            // piesele nu sunt mutate, deci nu exista avantaj sau dezavantaj pentru niciuna din parti

            foreach (Piece piece in Pieces)
            {
                if (piece.Player == PlayerType.Human)
                {
                    continue;
                }
                // in mod general functia scade din numarul total de piese din joc (in cazul nostru 56)
                // numarul de piese ale unui jucator si cele ale oponentului

                // pentru inceput stabilim o valoare (pondere) pentru piese
                // avem in vedere: dama, care poate fi atat in jumatea computerului, cat si in cea a jucatorului
                // pentru ca functia reprezinta cat de bine joaca calculatorul, dama din perimetrul acestuia va fi cu o valoare mai mare
                // decat dama din perimetrul omului

                // jocul contine si regi, piese care sunt importante si mai puternice decat damele
                // in acest caz, indiferent de pozitia in care se afla, regii vor avea aceeasi valoare, una mai mare decat damele
                if (piece.PieceType == PieceType.Queen)
                {
                    pondere = pondere + 10;
                }

                if (piece.PieceType == PieceType.Checker) // dame
                {
                    // daca dama computerului se afla pe un rand in jumatatea de tabla a omului, are avantaj deci primeste 7
                    // altfel, daca se afla in jumatatea lui de tabla, primeste doar 5
                    pondere += (piece.Y < Size / 2) ? 7 : 5;
                }

                // pentru ca jocul este unul dinamic, trebuie tratate toate situatiile in care se pot afla piesele
                // altfel spus, trebuie sa avem in vedere fiecare miscare care poate fi facuta de o piesa
                // fiecare mutare, cu cat se apropie mai mult de zona de incoronare, este evaluata cu 2

                // PRIMA MISCARE
                // prima data, dama pleaca din perimetrul ei de joc si merge spre centru
                // tabla fiind 8 x 8, consideram centrul ca fiind 4
                // totodata, numerotarea liniilor se considera de la 0, in cazul nostru cifrele de la 0 la 7 reprezinta cele 8 linii
                // in acest moment tratam cazul de plecare, deci nu vrem sa ajungem in centru (4) si punem conditie ca dama sa se afla in perimetrul 0 - 3
                if (piece.X > 0 && piece.X < Size - 1 && piece.Y > Size - 3 && piece.Y < Size - 1)
                {
                    pondere = pondere + 2;
                    // am ales valoarea 2 pentru ca nu vorbim despre un avantaj, este o simpla mutare, fara sa se ajunga in centru, deci nu a avansat prea mult spre linia finala
                }

                // A DOUA MISCARE
                // daca dama a parasit pozitia initiala, aceasta a avansat catre centru si il poate controla
                // centrul tablei de joc este considerat intre liniile 2 - 5 (excludem cele 2 linii din fiecare capat)
                if (piece.X > 2 && piece.X < Size - 2 && piece.Y > 2 && piece.Y < Size - 3)
                {
                    pondere = pondere + 4; // se avanseaza catre liniile finale, exista un avantaj, deci o astfel de miscare aduce o pondere mai mare
                }

                // A TREIA MISCARE
                // dupa miscarile din centru, damele se apropie de linia finala, deci se afla in avantaj
                // acest caz trebuie tratat avand in vedere ca piesa sa fie dama, pentru ca doar ele pot fi incoronate
                // si in functie de coordonatele X si Y trebuie stabilit daca se afla in permetrul apropiat zonei de incoronare
                if (piece.PieceType == PieceType.Checker && piece.X > 0 && piece.X < Size - 1 && piece.Y > 0 && piece.Y < 3)
                {
                    pondere = pondere + 6;
                }

                // A PATRA MISCARE
                // daca damele se afla in zona de incoronare si pot face o mutare spre aceasta
                // adica din penultimul rand pot muta spre ultimul si devin incoronate, ponderea se dubleaza
                if (piece.PieceType == PieceType.Checker && piece.Y == 0) // daca e dama ajunsa pe ultimul rand
                {
                    pondere = pondere + 12;
                }

                // am terminat tratarea celor 4 cazuri de miscari pe care le pot face damele
                // urmeaza sa valorificam configuratia jocului in urma miscarilor care au avut loc pana in acel moment
                // si tratam cazurile de capturare in functie de pozitiile pieselor
                // daca computerul se va afla intr-un pericol (poate fi capturata de om), acesta este in dezavantaj, functia trebuie sa scada
                // deci atribuim o pondere cu -, care sa fie mai mare decat suma posibila adunata in urma mutarilor anterioare
                // noi o vom considera -35, astfel evidentiem dezavantajul computerului

                // daca s-a intrat in oricare dintre cele 4 bucle posibile, inseamna ca piesa a fost capturata
                // evidentiem dezavantajul computerului printr-o decrementare majora
                // cu break iesim din bucla foreach respectiva pentru ca inseamna ca piesa a fost capturata acolo

                // prin human din foreach ma refer la piesa omului

                // piesa a computerului pe cale de capturare de catre om aflata in partea de jos a tablei
                // dama JOS-DREAPTA
                if (piece.X < Size - 1 && piece.Y > 0) // perimetrul pentru dreapta jos
                {
                    foreach (Piece human in Pieces)
                    {
                        // cautam in perimetrul tablei din dreapta jos in caz de se afla o dama de la om care pune o piesa a computerului in pericol
                        if (piece.X < Size - 1 && piece.Y > 0)
                        {
                            if (human.Player == PlayerType.Human && human.X == piece.X + 1 && human.Y == piece.Y - 1)
                            {
                                pondere = pondere - 35; // capturat de om
                                break;
                            }
                        }
                    }
                }

                // rege SUS-DREAPTA
                if (piece.X < Size - 1 && piece.Y < Size - 1)
                {
                    foreach (Piece human in Pieces)
                    {
                        // cautam in perimetrul tablei din dreapta sus in caz de se afla un rege de la om care pune o piesa a computerului in pericol
                        if (piece.X < Size - 1 && piece.Y < Size - 1)
                        {
                            if (human.PieceType == PieceType.Queen && human.Player == PlayerType.Human && human.X == piece.X + 1 && piece.Y == piece.Y + 1)
                            {
                                pondere = pondere - 35; // capturat de om
                                break;
                            }
                        }
                    }
                }

                // dama JOS-STANGA
                if (piece.X > 0 && piece.Y > 0)
                {
                    foreach (Piece human in Pieces)
                    {
                        // cautam in perimetrul tablei din stanga jos in caz ca se afla un rege de la om care pune o piesa a computerului in pericol
                        if (human.Player == PlayerType.Human && human.X == piece.X - 1 && piece.Y == piece.Y - 1)
                        {
                            pondere = pondere - 35; // capturat de om
                            break;
                        }
                    }
                }

                // rege SUS-STANGA
                if (piece.X > 0 && piece.Y < Size - 1)
                {
                    foreach (Piece human in Pieces)
                    {
                        // cautam in perimetrul tablei din stanga sus in caz ca se afla un rege de la om care pune o piesa a computerului in pericol
                        if (human.PieceType == PieceType.Queen && human.Player == PlayerType.Human && human.X == piece.X - 1 && human.Y == piece.Y + 1)
                        {
                            pondere = pondere - 35; // capturat de om
                            break;
                        }
                    }
                }

                if (capturePieces)
                {
                    pondere += 2000;
                }

                // ne apropiem de finalul jocului, iar dupa toate miscarile, atat omul cat si calculatorul detin un numar de regi (adica au ajuns la linia din capatul jumatatii celuilalt de tabla)
                // contorizam numarul de regi computerului
                int noOfComputerQueens = Pieces.Count(p => p.Player == PlayerType.Computer && p.PieceType == PieceType.Queen);
                // contorizam numarul de regi omului
                int noOfHumanQueens = Pieces.Count(p => p.Player != PlayerType.Computer && p.PieceType == PieceType.Queen);

                // ne intereseaza numarul de regi ai computerului pentru ca sa stabilim in ce forma se afla spre sfarsitul jocului si cum sa continue
                if (noOfComputerQueens == 0)
                {
                    return pondere;
                    // in acest moment returnam valoarea functiei de evaluare asa cum este pentru a vedea scorul mic al computerului
                    // fiind aproape sfarsitul jocului, iar el daca nu are regi, inseamna ca este intr-un dezavantaj considerabil
                    // trebuie sa incerce sa faca miscari ca sa isi imbunatateasca pozitia

                    // totodata, atunci cand o piesa devine rege ea se poate misca in toate directiile
                    // daca nu are regi, calculatorul are si optiuni limitate de actiune iar sansele de castig scad

                }

                // daca se ajunge in acest punct, inseamna ca exista regi ai computerului si trebuie sa actioneze
                // fiind finalul jocului, computerul trebuie sa faca ceva ca sa poata castiga
                // in acest moment il intereseaza ce miscari inteligente poate face in continuare ca sa aiba sanse de castig
                // mutarile depind de piesele omului, deci trebuie calculata distanta pana la ele

                // vom declara o lista in care sa retinem distantele de la regi la piesele adversarului
                // pentru cazul nostru, cea mai potrivita metoda de calcul este bazata pe distanta Manhattan

                // pentru 2 puncte A(x1, y1) si B(x2, y2) situate intr-un sistem xOy se va folosi distanta Manhattan 
                // care se calculeaza astfel: abs(x1- x2) + abs(y1 – y2)

                // de ce Manhattan? pentru ca este cea mai utilizata si totodata cea mai eficienta metoda
                // in cazul tablelor de sah sau a altora asemenea unde se pot face miscari doar in anumite directii
                // aceasta corespunde numarului minim de miscari pe care trebuie sa le faca o piesa pentru a ajunge de la un punct la altul
                // presupunand ca se poate misca doar orizontal sau vertical

                List<int> ManhattanDistances =
                    Pieces.Where(p => p.Player == PlayerType.Computer && p.PieceType == PieceType.Queen)
                        .Select(p => Pieces.Where(h => h.Player == PlayerType.Human)
                        .Sum(h => Math.Abs(p.X - h.X) + Math.Abs(p.Y - h.Y))).ToList();

                // pe linia cu Where filtram piesele pentru a retine doar regii computerului
                // cu .Select folosim regii pe care i-am filtrat anterior ca sa calculam 
                // distanta Manhattan dintre ei si toate celelalte piese ale omului ramase
                // rezultatele sunt adaugate in lista cu ToList()

                // pe baza distantelor rezultate aplicam strategia de joc cu algoritmul MinMax

                // CAZUL 1 - cand computerul are mai multi regi decat jucatorul, deci este in avantaj
                // STRATEGIE - ataca si captureaza cat mai mult

                // acesta trebuie sa-si foloseasca avantajul pentru a pune presiune asupra omului
                // minimizarea distantei presupune ca regii sunt mai aproape de piesele omului, deci este foarte probabil sa le poata captura
                // minimizarea distantei inseamna maximizarea sanselor de atac asupra pieselor omului, reducand numarul acestora
                if (noOfComputerQueens >= noOfHumanQueens)
                {
                    pondere = -pondere - ManhattanDistances.Min();
                }

                // CAZUL 2 - cand computerul are mai putini regi decat jucatorul, deci este in dezavantaj
                // STRATEGIE - evita atacul si se concentreaza pe protejarea pieselor

                // in acest caz computerul trebuie sa-si maximizeze distanta dintre piesele lui si cele ale omului
                // pentru a evita sa fie capturate, deci scopul este sa reduca sansa de atac a omului
                // daca se indeparteaza, isi protejeaza piesele deci jocul va dura mai mult pentru ca in acest timp
                // computerul cauta oportunitati de a-si recastiga pozitia si sa castige jocul
                if (noOfComputerQueens < noOfHumanQueens)
                {
                    pondere = pondere + ManhattanDistances.Max();
                }

            }
            return pondere;

        }



    }

    //=============================================================================================================================

    public partial class Piece
    {
        /// <summary>
        /// Returneaza lista tuturor mutarilor permise pentru piesa curenta (this)
        /// in configuratia (tabla de joc) primita ca parametru
        /// </summary>
        public List<Move> ValidMoves(Board currentBoard)
        {
            List<Move> validMoves = new List<Move>();

            for (int x = -2; x <= 2; ++x)
            {
                for (int y = -2; y <= 2; ++y)
                {
                    Move move = new Move(Id, X + x, Y + y);
                    if (IsValidMove(currentBoard, move))
                    {
                        validMoves.Add(move);
                    }
                }
            }
            return validMoves;
        }

        /// <summary>
        /// Testeaza daca o mutare este valida intr-o anumita configuratie
        /// </summary>
        public bool IsValidMove(Board currentBoard, Move move)
        {

            int xDiff = Math.Abs(move.NewX - X);
            int yDiff = Math.Abs(move.NewY - Y);

            // verificam ca mutarea - distanta dintre pozitia curenta si noua mutare -  sa nu fie mai mare de un 1 
            if (!(xDiff == 2 && yDiff == 2))
            {
                // verificam ca distanta dintre pozitia curenta si noua mutare este 1
                if (xDiff > 1 && yDiff > 1)
                {
                    return false;
                }

                // verificam ca pionul se va misca doar pe diagonala
                if (Y != move.NewY - (move.NewX - X) && Y != move.NewY + (move.NewX - X))
                {
                    return false;
                }
            }

            // verificam daca piesa este mutata sau daca nu depaseste careul
            if (move.NewX < 0 || move.NewY < 0 || move.NewX >= currentBoard.Size
                 || move.NewY >= currentBoard.Size)
            {
                return false;
            }

            // verificam daca in care se afla deja o piesa
            foreach (Piece piece in currentBoard.Pieces)
            {
                if (piece.X == move.NewX && piece.Y == move.NewY)
                {
                    return false;
                }
            }

            // verificam daca pionul este dama, deoarece acest pion nu se poate intoarce
            if (currentBoard.Pieces[move.PieceId].PieceType == PieceType.Checker)
            {
                if ((currentBoard.Pieces[move.PieceId].Player == PlayerType.Human && move.NewY < currentBoard.Pieces[move.PieceId].Y) ||
                        (currentBoard.Pieces[move.PieceId].Player == PlayerType.Computer && move.NewY > currentBoard.Pieces[move.PieceId].Y))
                {
                    return false;
                }

            }


            // mutare pe diagonala peste 2 casete, cand unul dintre pioni fura pionul celuilalt
            if (xDiff == 2 && yDiff == 2)
            {
                bool skip = false;
                int skipX = -1, skipY = -1; // piesa peste care sa se sara - care este mancata

                if (move.NewX - X > 0 && move.NewY - Y > 0) // dreapta-sus
                {
                    skipX = X + 1;
                    skipY = Y + 1;
                }
                else if (move.NewX - X < 0 && move.NewY - Y > 0) // stanga-sus
                {
                    skipX = X - 1;
                    skipY = Y + 1;
                }
                else if (move.NewX - X > 0 && move.NewY - Y < 0) // dreapta-jos
                {
                    skipX = X + 1;
                    skipY = Y - 1;
                }
                else if (move.NewX - X < 0 && move.NewY - Y < 0) // stanga-jos
                {
                    skipX = X - 1;
                    skipY = Y - 1;
                }

                //verificam daca piesa este oponent
                foreach (Piece piece in currentBoard.Pieces)
                {
                    
                     if (piece.X == skipX && piece.Y == skipY)
                     {
                        if (!isOpponent(piece.Player))
                        {
                            return false; // daca piesa nu este a adversarului, atunci mutarea nu este valida
                        }
                        skip = true;
                        break; // in cazul in mutarea este valida, nu se va mai cauta
                     }
                }
                if (!skip)
                {
                    return false;
                }
            }

            return true;

        }

        private bool isOpponent(PlayerType player)
        {
            bool isOpponent = true;

            if ((player == PlayerType.Computer && Player == PlayerType.Computer) || (player == PlayerType.Human && Player == PlayerType.Human)) // daca ambele piese sunt acele calculatorului/persoanei in acelasi timp
            {
                isOpponent = false;
            }
            return isOpponent;
        }
    }


    //=============================================================================================================================

    public partial class Minimax
    {
        /// <summary>
        /// Primeste o configuratie ca parametru, cauta mutarea optima si returneaza configuratia
        /// care rezulta prin aplicarea acestei mutari optime
        /// </summary>
        /* public static Board FindNextBoard(Board currentBoard)
         {
             throw new Exception("Aceasta metoda trebuie implementata");
         }
        */

        public static ActionsGame AlphaBetaPruningFunction(ActionsGame node, bool maxLevel, int depth, double alfa, double beta)
        {
            PlayerType winner;
            bool finished;
            node.board.CheckFinish(out finished, out winner);
            // daca jocul s-a terminat/ s-a ajuns intr-un nod terminal/s-a ajuns la adancimea max 
            if (depth <= 0 || finished) 
            {
                return node;
            }
            // jucatorul maximizant este calc si cel minimizant este persoana
            ActionsGame result = new ActionsGame();

            // in cazul in care jucatorul este persoana
            if (!maxLevel)
            {
                result.evaluation = Double.PositiveInfinity;
                List<Board> possibleConifg = TakePossibleConfig(PlayerType.Human, node);

                // aplicare retezare alfa beta pentru fiecare configuratie dintre cele posibile
                foreach (Board board in possibleConifg)
                {
                    // creeam cate o configuratie din cele posibile
                    ActionsGame action = new ActionsGame();
                    action.board = board;
                    action.evaluation = board.EvaluationFunction();

                    ActionsGame nextAction = AlphaBetaPruningFunction(action, false, depth - 1, alfa, beta);
                    // val optima
                    if (nextAction.evaluation < result.evaluation) // consideram valoarea maxima a functiei de evaluare
                    {
                        result = nextAction; // se trece la urmatoarea actiune
                        result.board = board;
                    }

                    if (alfa >= beta) // nu se mai incearca alte actiuni
                    {
                        break;
                    }

                    beta = Math.Min(beta, result.evaluation);
                }
            }
            else // in cazul in care jucatorul este computer
            {
                result.evaluation = Double.NegativeInfinity;
                List<Board> possibleConifg = TakePossibleConfig(PlayerType.Computer, node);

                // aplicare retezare alfa beta pentru fiecare configuratie dintre cele posibile
                foreach (Board board in possibleConifg)
                {
                    // creeam cate o configuratie din cele posibile
                    ActionsGame action = new ActionsGame();
                    action.board = board;
                    action.evaluation = board.EvaluationFunction();

                    ActionsGame nextAction = AlphaBetaPruningFunction(action, false, depth - 1, alfa, beta);
                    // val optima
                    if (nextAction.evaluation > result.evaluation) // consideram valoarea maxima a functiei de evaluare
                    {
                        result = nextAction; // se trece la urmatoarea actiune
                        result.board = board;
                    }

                    if (alfa >= beta) // nu se mai incearca alte actiuni
                    {
                        break;
                    }

                    alfa = Math.Max(alfa, result.evaluation);
                }
            }
            return result;
        }

        /// <summary>
        /// Functie care va parcurge fiecare piesa de pe tabla si va returna o lista cu configuratiile posibile
        /// </summary>
        private static List<Board> TakePossibleConfig(PlayerType player, ActionsGame config)
        {
            List<Board> candidates = new List<Board>();

            // pentru fiecare piesa de pe tabla
            foreach (Piece piece in config.board.Pieces)
            {
                // se verifica tipul jucatorului pentru a putea ajuta urmatorul pas in functia principala
                if (piece.Player == player)
                {
                    // pentru mutarile valide ale piesei curente, se va realiza o configuratia posibila
                    foreach (Move move in piece.ValidMoves(config.board))
                    {
                        candidates.Add(config.board.MakeMove(move));
                    }
                }
            }
            return candidates;
        }

    }

}