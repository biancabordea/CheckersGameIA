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
                diff += piece.Y;

            }
            return 12 - diff;
        }

        /// <summary>
        /// Strategia masoara pentru fiecare piesa cate pozitii trebuie sa mearga pentru a ajunge la finish.
        /// Piesele sunt considerate obstacole si se face algoritmul lui Lee petru fiecare piesa in parte.
        /// La final se face diferenta intre punctele human si pc.
        /// </summary>
        /// <returns> Valoarea asignata tablei de joc </returns>

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

                    if (piece.X == skipX || piece.Y == skipY) // nu sunt inca sigura..trebuie sa verific cand functiile vor
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
            public static Board FindNextBoard(Board currentBoard)
            {
                throw new Exception("Aceasta metoda trebuie implementata");

            }
        }
    
}