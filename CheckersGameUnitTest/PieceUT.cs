using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleCheckers;

namespace CheckersGameUnitTest
{
    [TestClass]
    public class PieceUT
    {
        readonly Board board = new Board();

        [TestMethod]
        public void IsValidMove_Success()
        {
            Move move = new Move(8, 0, 4);
            Assert.AreEqual(true, board.Pieces[8].IsValidMove(board, move));
        }

        [TestMethod]
        public void IsValidMove_NotDiagonally_Fail()
        {
            Move move = new Move(8, 1, 4);
            Assert.AreNotEqual(true, board.Pieces[8].IsValidMove(board, move));
        }

        [TestMethod]
        public void IsValidMove_MoreThanOneSpace_Fail()
        {
            Move move = new Move(8, 3, 3);
            Assert.AreEqual(false, board.Pieces[8].IsValidMove(board, move));
        }

        [TestMethod]
        public void IsValidMove_OccupiedSpace_Fail()
        {
            Move move = new Move(8, 2, 6);
            Assert.AreEqual(false, board.Pieces[8].IsValidMove(board, move));
        }
    }
}
