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

        // configuratie tabla de joc
        public Board()
        {
            Size = 8; // tabla este 8x8
            Pieces = new List<Piece>(Size * 3);
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