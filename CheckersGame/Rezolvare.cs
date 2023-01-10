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
            //throw new Exception("Aceasta metoda trebuie implementata");
            int diff = 0;
            foreach (Piece piece in Pieces)
            {
                if (piece.Player == PlayerType.Human)
                {
                    continue;
                }
                diff += piece.Y;

            }

            return 56 - diff;

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
            //throw new Exception("Aceasta metoda trebuie implementata");
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
            //throw new Exception("Aceasta metoda trebuie implementata");
            // nu se muta nicio piesa
            //nu muta in afara tablei
            // nu muta in careu ocupat
            // sare mai multe careuri, cu exceptia cand poate manca un adversat pe diagonala


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

            
            int xDiff = Math.Abs(move.NewX - X);
            int yDiff = Math.Abs(move.NewY - Y);
            // mutarea nu are voie in acest stadiu sa fie mai mare de 1 patrat
            if (xDiff == 2 && yDiff == 2)
            {
                // verificam ca distanta dintre pozitia curenta si noua mutare este 1
                if (xDiff > 1 && yDiff > 1)
                {
                    return false;
                }

                // pionul se va misca doar pe diagonala
                if (Y != move.NewY - (move.NewX - X) && Y != move.NewY + (move.NewX - X))
                {
                    return false;
                }
            }


            // verificam daca pionul este dama, deoarece acest pion nu se poate intoarce
            if (currentBoard.Pieces[move.PieceId].PieceType == PieceType.Checker)
            {
                if (currentBoard.Pieces[move.PieceId].Player == PlayerType.Human && move.NewY < currentBoard.Pieces[move.PieceId].Y)
                {
                    return false;
                }
                else if (currentBoard.Pieces[move.PieceId].Player == PlayerType.Computer && move.NewY > currentBoard.Pieces[move.PieceId].Y)
                {
                    return false;
                }
            }


            // mutare pe diagonala peste 2 casete, cand unul dintre pioni fura poinul celuilalt
            if (xDiff == 2 && yDiff == 2)
            {
                bool skip = false;

                int skipX = -1 , skipY = -1; // piesa peste care sa se sara

                if (move.NewX - X > 0 && move.NewY - Y > 0) // dreapta-sus
                {
                    skipX = X + 1;
                    skipY = Y + 1;
                }
                else if (move.NewX - X > 0 && move.NewY - Y < 0) //dreapta-jos
                {
                    skipX = X + 1;
                    skipY = Y - 1;
                }
                else if (move.NewX - X < 0 && move.NewY - Y < 0) //stanga-jos
                {
                    skipX = X - 1;
                    skipY = Y - 1;
                }
                else if (move.NewX - X < 0 && move.NewY - Y > 0) // stanga-sus
                {
                    skipX = X - 1;
                    skipY = Y + 1;
                }
                

                foreach (Piece piece in currentBoard.Pieces)
                { 

                    if (piece.X == skipX || piece.Y == skipY) 
                    {
                        if (!isOpponent(piece.Player)) // daca pe caseta nu este o piesa oponent
                        {
                            return false;
                        }
                        skip = false;
                        break; // mutarea este valida

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

            public static ActionsGame AlphaBetaPruningFunction(ActionsGame node, bool maxLevel,  int depth, double alfa, double beta)
            {
                PlayerType winner;
                bool finished;
                node.board.CheckFinish(out finished, out winner);
                // daca jocul s-a terminat/ s-a ajuns intr-un nod terminal/s-a ajuns la adancimea max 
                if (depth <= 0 || finished) // am ajuns pe nod terminal, la limita de adancime impusa, sau jocul s-a incheiat
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
                    if ( piece.Player == player )
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