﻿/**************************************************************************
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
        // public bool captureMade { get; set; } // daca s-a efectuat o captura in configuratia anterioara configuratiei curente


        // configuratie tabla de joc
        public Board()
        {
            Size = 8; // tabla este 8x8
            Pieces = new List<Piece>(Size * 3); // fiecare jucator are 3 randuri de piese
            int randuriMax = 0;
            int id = 0;

            // piesele trebuie amplasate doar pe patratele de culoare neagra
            // piesele calculatorului
            for (int i = 1; i < Size; i += 2)
            {
                Pieces.Add(new Piece(i, Size - 1, i, PlayerType.Computer));
                Pieces.Add(new Piece(i - 1, Size - 2, i, PlayerType.Computer));
                Pieces.Add(new Piece(i, Size - 3, i, PlayerType.Computer));
            }

            // tabla jucatorului
            for (int i = 0; i < Size; i+=2)
            {
                Pieces.Add(new Piece(i, 0, i + Size, PlayerType.Human));
                Pieces.Add(new Piece(i + 1, 1, i, PlayerType.Human));
                Pieces.Add(new Piece(i, 2, i, PlayerType.Human));

            }
         

            /* for (int i = 0; i < Size; i++)
                   Pieces.Add(new Piece(i, Size - 1, i, PlayerType.Computer));

               for (int i = 0; i < Size; i++)
                   Pieces.Add(new Piece(i, 0, i + Size, PlayerType.Human));*/
        }

        public Board(Board b)
        {
            Size = b.Size;
            Pieces = new List<Piece>(Size * 2);

            foreach (Piece p in b.Pieces)
                Pieces.Add(new Piece(p.X, p.Y, p.Id, p.Player));
        }

        // public double EvaluationFunction() - completati aceasta metoda in fisierul Rezolvare.cs

        /// <summary>
        /// Creeaza o noua configuratie aplicand mutarea primita ca parametru in configuratia curenta
        /// </summary>
        public Board MakeMove(Move move)
        {
            Board nextBoard = new Board(this); // copy

            nextBoard.Pieces[move.PieceId].X = move.NewX;
            nextBoard.Pieces[move.PieceId].Y = move.NewY;

            // daca exista pe diagonala un oponent, poate fi capturat
            int capturedPieceX = 0, capturedPieceY = 0;
            //se sare fix peste un oponent pe diagonala
            if (System.Math.Abs(move.NewX - nextBoard.Pieces[move.PieceId].X) == 2 && System.Math.Abs(move.NewY - nextBoard.Pieces[move.PieceId].Y) == 2)
            {
                // stanga sus
                if (move.NewX - nextBoard.Pieces[move.PieceId].X < 0 && move.NewY - nextBoard.Pieces[move.PieceId].Y > 0)
                {
                    capturedPieceX = nextBoard.Pieces[move.PieceId].X - 1;
                    capturedPieceY = nextBoard.Pieces[move.PieceId].Y + 1;
                }
                //dreapta sus
                else if (move.NewX - nextBoard.Pieces[move.PieceId].X > 0 && move.NewY - nextBoard.Pieces[move.PieceId].Y > 0)
                {
                    capturedPieceX = nextBoard.Pieces[move.PieceId].X + 1;
                    capturedPieceY = nextBoard.Pieces[move.PieceId].Y + 1;
                }
                //stanga jos
                else if (move.NewX - nextBoard.Pieces[move.PieceId].X < 0 && move.NewY - nextBoard.Pieces[move.PieceId].Y < 0)
                {
                    capturedPieceX = nextBoard.Pieces[move.PieceId].X - 1;
                    capturedPieceY = nextBoard.Pieces[move.PieceId].Y - 1;
                }
                //dreapta jos
                else if (move.NewX - nextBoard.Pieces[move.PieceId].X > 0 && move.NewY - nextBoard.Pieces[move.PieceId].Y < 0)
                {
                    capturedPieceX = nextBoard.Pieces[move.PieceId].X + 1;
                    capturedPieceY = nextBoard.Pieces[move.PieceId].Y - 1;
                }

                //eliminam piesa oponent de pe tabla
                foreach (Piece piece in Pieces)
                {
                    if (piece.X == capturedPieceX && piece.Y == capturedPieceY)
                    {
                        piece.X = -1;
                        piece.Y = -1;
                        // nextBoard.captureMade = true;
                        break;
                    }
                }
            }
            else
            {
                // nextBoard.captureMade = false;
            }

            // dama devine rege cand ajunge pe ultimul rand al adversarului
            if ((nextBoard.Pieces[move.PieceId].Player == PlayerType.Human || nextBoard.Pieces[move.PieceId].Player == PlayerType.Computer) &&
                nextBoard.Pieces[move.PieceId].PieceType != PieceType.King && move.NewY == Size - 1)
            {
                nextBoard.Pieces[move.PieceId].PieceType = PieceType.King;
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
            if (Pieces.Where(p => p.Player == PlayerType.Human && p.Y == Size - 1).Count() == Size)
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
            }

            finished = false;
            winner = PlayerType.None;
        }
    }
}