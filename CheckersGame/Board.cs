/**************************************************************************
 *                                                                        *
 *  Copyright:   (c) 2016-2020, Florin Leon                               *
 *  E-mail:      florin.leon@academic.tuiasi.ro                           *
 *  Website:     http://florinleon.byethost24.com/lab_ia.html             *
 *  Description: Game playing. Minimax algorithm                          *
 *               (Artificial Intelligence lab 7)                          *
 *                                                                        *
 *  This code and information is provided "as is" without warranty of     *
 *  any kind, either expressed or implied, including but not limited      *
 *  to the implied warranties of merchantability or fitness for a         *
 *  particular purpose. You are free to use this source code in your      *
 *  applications as long as the original copyright notice is included.    *
 *                                                                        *
 **************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleCheckers
{
    /// <summary>
    /// Reprezinta o configuratie a jocului (o tabla de joc) la un moment dat
    /// </summary>
    public partial class Board
    {
        public int Size { get; set; } // dimensiunea tablei de joc
        public List<Piece> Pieces { get; set; } // lista de piese, atat ale omului cat si ale calculatorului
        public bool capturePieces { get; set; } // daca s-a efectuat o captura in configuratia anterioara configuratiei curente


        // configuratie tabla de joc
        public Board()
        {
            Size = 8; // tabla este 8x8
            Pieces = new List<Piece>(Size * 3); // fiecare jucator are 3 randuri de piese
            int id = 0;

            // piese calculator
            for (int i = 0; i < Size; i++)
            {
                Pieces.Add(new Piece(i, Size - 1 - (i + 1) % 2, id++, PlayerType.Computer, PieceType.Checker));
            }
            for (int i = 1; i < Size; i += 2) 
            {
                Pieces.Add(new Piece(i, Size - 3, id++, PlayerType.Computer, PieceType.Checker));
            }

            // piese om
            for (int i = 0; i < Size; i++)
            {
                Pieces.Add(new Piece(i, (i + 2) % 2, id++, PlayerType.Human, PieceType.Checker));

            }
            for (int i = 0; i < Size; i += 2) 
            {
                Pieces.Add(new Piece(i, 2, id++, PlayerType.Human, PieceType.Checker));
            }

            /* for (int i = 0; i < Size; i++)
                   Pieces.Add(new Piece(i, Size - 1, i, PlayerType.Computer));

               for (int i = 0; i < Size; i++)
                   Pieces.Add(new Piece(i, 0, i + Size, PlayerType.Human));*/
        }

        public Board(Board b)
        {
            Size = b.Size;
            capturePieces = b.capturePieces;
            Pieces = new List<Piece>(Size * 2);

            foreach (Piece p in b.Pieces)
                Pieces.Add(new Piece(p.X, p.Y, p.Id, p.Player, p.PieceType));
        }

        // public double EvaluationFunction() - completati aceasta metoda in fisierul Rezolvare.cs

        /// <summary>
        /// Creeaza o noua configuratie aplicand mutarea primita ca parametru in configuratia curenta
        /// </summary>
        public Board MakeMove(Move move)
        {
            Board nextBoard = new Board(this); // copy

           

            // daca exista pe diagonala un oponent, poate fi capturat
            int capturedPieceX = -1, capturedPieceY = -1;
            //se sare fix peste un oponent pe diagonala
            if (System.Math.Abs(move.NewX - nextBoard.Pieces[move.PieceId].X) == 2 && System.Math.Abs(move.NewY - nextBoard.Pieces[move.PieceId].Y) == 2)
            {
                // dreapta-sus
                if (move.NewX - nextBoard.Pieces[move.PieceId].X > 0 && move.NewY - nextBoard.Pieces[move.PieceId].Y > 0) 
                {
                    capturedPieceX = move.NewX - 1;
                    capturedPieceY = move.NewY - 1;
                }
                // stanga-sus
                else if (move.NewX - nextBoard.Pieces[move.PieceId].X < 0 && move.NewY - nextBoard.Pieces[move.PieceId].Y > 0) 
                {
                    capturedPieceX = move.NewX + 1;
                    capturedPieceY = move.NewY - 1;
                }
                // dreapta-jos
                else if (move.NewX - nextBoard.Pieces[move.PieceId].X > 0 && move.NewY - nextBoard.Pieces[move.PieceId].Y < 0) 
                {
                    capturedPieceX = move.NewX - 1;
                    capturedPieceY = move.NewY + 1;
                }
                // stanga-jos
                else if (move.NewX - nextBoard.Pieces[move.PieceId].X < 0 && move.NewY - nextBoard.Pieces[move.PieceId].Y < 0) 
                {
                    capturedPieceX = move.NewX + 1;
                    capturedPieceY = move.NewY + 1;
                }
                for (int i = 0; i < nextBoard.Pieces.Count; ++i)
                {
                    Piece piece = nextBoard.Pieces.ElementAt<Piece>(i);
                    if (piece.X == capturedPieceX && piece.Y == capturedPieceY)
                    {
                        piece.X = -1;
                        piece.Y = -1;
                        nextBoard.capturePieces = true;
                        break;
                    }
                }
            }
            else
            {
                nextBoard.capturePieces = false;
            }

            nextBoard.Pieces[move.PieceId].X = move.NewX;
            nextBoard.Pieces[move.PieceId].Y = move.NewY;

            // dama devine rege cand ajunge pe ultimul rand al adversarului
            if ((nextBoard.Pieces[move.PieceId].Player == PlayerType.Human || nextBoard.Pieces[move.PieceId].Player == PlayerType.Computer) &&
                nextBoard.Pieces[move.PieceId].PieceType != PieceType.Queen && move.NewY == Size - 1)
            {
                nextBoard.Pieces[move.PieceId].PieceType = PieceType.Queen;
            }


            return nextBoard;
        }

       
        /// <summary>
        /// Verifica daca configuratia curenta este castigatoare
        /// </summary>
        /// <param name="finished">Este true daca cineva a castigat si false altfel</param>
        /// <param name="winner">Cine a castigat: omul sau calculatorul</param>
        public void CheckFinish(out bool finished, out PlayerType winner)
        {
            /* if (Pieces.Where(p => p.Player == PlayerType.Human && p.Y == Size - 1).Count() == Size)
             {
                 finished = true;
                 winner = PlayerType.Human;
                 return;
             }

             if (Pieces.Where(p => p.Player == PlayerType.Computer && p.Y == 0).Count() == Size)
             {
                 finished = true;
                 winner = PlayerType.Computer;
                 return;
             }*/
            if (Pieces.Where(p => p.Player == PlayerType.Human && p.X == -1 && p.Y == -1).Count() == Size * 3 / 2)
            {
                finished = true;
                winner = PlayerType.Computer;
                return;
            }

            if (Pieces.Where(p => p.Player == PlayerType.Computer && p.X == -1 && p.Y == -1).Count() == Size * 3 / 2)
            {
                finished = true;
                winner = PlayerType.Human;
                return;
            }

            finished = false;
            winner = PlayerType.None;
        }
    }
}