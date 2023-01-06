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

            for (int x = -1; x <= 1; ++x)
            {
                for (int y = -1; y <= 1; ++y)
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

            // verificam ca distanta dintre pozitia curenta si noua mutare este 1
            int xDiff = Math.Abs(move.NewX - X);
            int yDiff = Math.Abs(move.NewY - Y);
            if (xDiff > 1 && yDiff > 1)
            {
                return false;
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
                foreach (Piece piece in currentBoard.Pieces)
                {
                    return false;
                }
            }

                return true;
            
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