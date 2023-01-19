using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleCheckers;


namespace CheckersGameUnitTest
{
    [TestClass]
    public class BoardUT
    {
        // construire tabla de joc initiala
        readonly Board board = new Board();

        //verificam corectitudinea asezarii pieselor PC-ului pe tabla
        [TestMethod]
        public void VerifyInitialBoard_PcPieces_Success()
        {
            Assert.AreEqual(0, board.Pieces[0].X);
            Assert.AreEqual(6, board.Pieces[0].Y);
            Assert.AreEqual(1, board.Pieces[1].X);
            Assert.AreEqual(7, board.Pieces[1].Y);
            Assert.AreEqual(2, board.Pieces[2].X);
            Assert.AreEqual(6, board.Pieces[2].Y);

            Assert.AreEqual(1, board.Pieces[8].X);
            Assert.AreEqual(5, board.Pieces[8].Y);
        }

        //verificam corectitudinea asezarii pieselor jucatorului pe tabla
        [TestMethod]
        public void VerifyInitialBoard_PlayerPieces_Success()
        {
            Assert.AreEqual(0, board.Pieces[12].X);
            Assert.AreEqual(0, board.Pieces[12].Y);
            Assert.AreEqual(1, board.Pieces[13].X);
            Assert.AreEqual(1, board.Pieces[13].Y);
            Assert.AreEqual(2, board.Pieces[14].X);
            Assert.AreEqual(0, board.Pieces[14].Y);

            Assert.AreEqual(0, board.Pieces[20].X);
            Assert.AreEqual(2, board.Pieces[20].Y);
        }

        [TestMethod]
        public void MakeMove_VerifyPieceMoves_Success()
        {
            Board currentBoard = new Board(board);
            currentBoard = currentBoard.MakeMove(new Move(23, 7, 3));

            Assert.AreNotEqual(board, currentBoard);
        }

        //PC are regina
        [TestMethod]
        public void MakeMove_CheckersToQueenPc_Success()
        {
            Board currentBoard = new Board(board);
            currentBoard = currentBoard.MakeMove(new Move(8, 0, 4));
            currentBoard = currentBoard.MakeMove(new Move(21, 1, 3));
            currentBoard = currentBoard.MakeMove(new Move(8, 2, 2));
            currentBoard = currentBoard.MakeMove(new Move(22, 3, 3));
            currentBoard = currentBoard.MakeMove(new Move(0, 1, 5));
            currentBoard = currentBoard.MakeMove(new Move(15, 4, 2));
            currentBoard = currentBoard.MakeMove(new Move(0, 0, 4));
            currentBoard = currentBoard.MakeMove(new Move(15, 5, 3));
            currentBoard = currentBoard.MakeMove(new Move(1, 0, 6));
            currentBoard = currentBoard.MakeMove(new Move(14, 3, 1));
            currentBoard = currentBoard.MakeMove(new Move(1, 1, 5));
            currentBoard = currentBoard.MakeMove(new Move(14, 4, 2));
            currentBoard = currentBoard.MakeMove(new Move(8, 3, 1));
            currentBoard = currentBoard.MakeMove(new Move(23, 7, 3));
            // 8 devine regina
            currentBoard = currentBoard.MakeMove(new Move(8, 2, 0));

            Assert.AreEqual(8, currentBoard.Pieces[8].Id);
            Assert.AreEqual(PieceType.Queen, currentBoard.Pieces[8].PieceType);
        }

        // jucatorul are regina
        [TestMethod]
        public void MakeMove_CheckersToQueenPlayer_Success()
        {
            Board currentBoard = new Board(board);
            currentBoard = currentBoard.MakeMove(new Move(8, 0, 4));
            currentBoard = currentBoard.MakeMove(new Move(20, 1, 3));
            currentBoard = currentBoard.MakeMove(new Move(0, 1, 5));
            currentBoard = currentBoard.MakeMove(new Move(13, 0, 2));
            currentBoard = currentBoard.MakeMove(new Move(9, 4, 4));
            currentBoard = currentBoard.MakeMove(new Move(12, 1, 1));
            currentBoard = currentBoard.MakeMove(new Move(9, 5, 3));
            currentBoard = currentBoard.MakeMove(new Move(23, 4, 4));
            currentBoard = currentBoard.MakeMove(new Move(0, 2, 4));
            currentBoard = currentBoard.MakeMove(new Move(23, 3, 5));
            currentBoard = currentBoard.MakeMove(new Move(2, 1, 5));
            currentBoard = currentBoard.MakeMove(new Move(23, 2, 6));
            currentBoard = currentBoard.MakeMove(new Move(1, 0, 6));

            //23 devine regina
            currentBoard = currentBoard.MakeMove(new Move(23, 1, 7));

            Assert.AreEqual(23, currentBoard.Pieces[23].Id);
            Assert.AreEqual(PieceType.Queen, currentBoard.Pieces[23].PieceType);
        }

        [TestMethod]
        public void EvaluationFunction_Success()
        {
            Board currentBoard = new Board(board);
            currentBoard = currentBoard.MakeMove(new Move(8, 0, 4));
            currentBoard = currentBoard.MakeMove(new Move(21, 1, 3));
            currentBoard = currentBoard.MakeMove(new Move(8, 2, 2));
            currentBoard = currentBoard.MakeMove(new Move(22, 3, 3));
            currentBoard = currentBoard.MakeMove(new Move(0, 1, 5));
            currentBoard = currentBoard.MakeMove(new Move(15, 4, 2));
            currentBoard = currentBoard.MakeMove(new Move(0, 0, 4));
            currentBoard = currentBoard.MakeMove(new Move(15, 5, 3));
            currentBoard = currentBoard.MakeMove(new Move(1, 0, 6));
            currentBoard = currentBoard.MakeMove(new Move(14, 3, 1));
            currentBoard = currentBoard.MakeMove(new Move(1, 1, 5));
            currentBoard = currentBoard.MakeMove(new Move(14, 4, 2));
            currentBoard = currentBoard.MakeMove(new Move(8, 3, 1));
            currentBoard = currentBoard.MakeMove(new Move(23, 7, 3));
            // 8 devine regina
            currentBoard = currentBoard.MakeMove(new Move(8, 2, 0));
            Assert.AreEqual(11, currentBoard.EvaluationFunction());
        }

    }
}
