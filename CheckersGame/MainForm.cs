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
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SimpleCheckers
{
    public partial class MainForm : Form
    {
        private Board _board;
        private int _selected; // indexul piesei selectate
        private PlayerType _currentPlayer; // om sau calculator
        private Bitmap _boardImage;

        public MainForm()
        {
            InitializeComponent();

            try
            {
                _boardImage = (Bitmap)Image.FromFile("board.png");
            }
            catch
            {
                MessageBox.Show("Nu se poate incarca board.png");
                Environment.Exit(1);
            }

            _board = new Board();
            _currentPlayer = PlayerType.None;
            _selected = -1; // nicio piesa selectata

            this.ClientSize = new System.Drawing.Size(927, 600);
            this.pictureBoxBoard.Size = new System.Drawing.Size(500, 500);

            pictureBoxBoard.Refresh();
        }

        private void pictureBoxBoard_Paint(object sender, PaintEventArgs e)
        {
            Bitmap board = new Bitmap(_boardImage);
            e.Graphics.DrawImage(board, 0, 0);

            if (_board == null)
                return;

             
            float dy = 500 - 62.5f + 6;
            SolidBrush transparentRed = new SolidBrush(Color.FromArgb(192, 255, 0, 0));
            SolidBrush transparentBlack = new SolidBrush(Color.FromArgb(192, 0, 0, 0));
            SolidBrush transparentYellow = new SolidBrush(Color.FromArgb(192, 255, 255, 0));

            // marcam faptul ca o dama devine regina
            SolidBrush transparentQueen = new SolidBrush(Color.FromArgb(255, 255, 255, 0));

      
            foreach (Piece p in _board.Pieces)
            {
                SolidBrush brush = transparentRed;
                if (p.Player == PlayerType.Human)
                {
                    if (p.Id == _selected)
                        brush = transparentYellow;
                    else
                        brush = transparentBlack;
                }

              //  e.Graphics.FillEllipse(brush, 12 + p.X * 125, dy - p.Y * 125, 100, 100);
                e.Graphics.FillEllipse(brush, (float)(6 + p.X * 62.5), (float)(dy - p.Y * 62.5), 50, 50);

                // daca o dama devine regina, se va marca acest lucru
                if (p.PieceType == PieceType.Queen)
                {
                    brush = transparentQueen;
                    e.Graphics.FillEllipse(brush, (float)(18 + p.X * 62.5), (float)(dy - p.Y * 62.5 + 12), 25, 25);

                }
            }
        }

        private void pictureBoxBoard_MouseUp(object sender, MouseEventArgs e)
        {
            if (_currentPlayer != PlayerType.Human)
                return;

            int mouseX = (int)Math.Truncate((float)e.X / 62.5f);
            int mouseY = (int)(6 - Math.Truncate(e.Y / 62.5f) + 1);

            if (_selected == -1)
            {
                foreach (Piece p in _board.Pieces.Where(a => a.Player == PlayerType.Human))
                {
                    if (p.X == mouseX && p.Y == mouseY)
                    {
                        _selected = p.Id;
                        pictureBoxBoard.Refresh();
                        break;
                    }
                }
            }
            else
            {
                Piece selectedPiece = _board.Pieces[_selected];

                if (selectedPiece.X == mouseX && selectedPiece.Y == mouseY)
                {
                    _selected = -1;
                    pictureBoxBoard.Refresh();
                }
                else
                {
                    Move m = new Move(_selected, mouseX, mouseY);

                    if (selectedPiece.IsValidMove(_board, m))
                    {
                        _selected = -1;
                        Board b = _board.MakeMove(m);
                        AnimateTransition(_board, b);
                        _board = b;
                        pictureBoxBoard.Refresh();
                        _currentPlayer = PlayerType.Computer;

                        CheckFinish();

                        if (_currentPlayer == PlayerType.Computer) // jocul nu s-a terminat
                            ComputerMove();
                    }
                }
            }
        }

        private void ComputerMove()
        {
            // Board nextBoard = Minimax.FindNextBoard(_board);
            ActionsGame action = new ActionsGame();
            action.board = _board;
            action.evaluation = _board.EvaluationFunction();
            Board nextBoard = Minimax.AlphaBetaPruningFunction(action, true, Minimax.maxDepth, Double.NegativeInfinity, Double.PositiveInfinity).board;

            AnimateTransition(_board, nextBoard);
            _board = nextBoard;
            pictureBoxBoard.Refresh();

            _currentPlayer = PlayerType.Human;

            CheckFinish();
        }

        private void CheckFinish()
        {
            bool end; PlayerType winner;
            _board.CheckFinish(out end, out winner);

            if (end)
            {
                if (winner == PlayerType.Computer)
                {
                    MessageBox.Show("Calculatorul a castigat!");
                    _currentPlayer = PlayerType.None;
                }
                else if (winner == PlayerType.Human)
                {
                    MessageBox.Show("Ai castigat!");
                    _currentPlayer = PlayerType.None;
                }
            }
        }

        private void AnimateTransition(Board b1, Board b2)
        {
            Bitmap board = new Bitmap(_boardImage);
            float dy = 500 - 62.5f + 6;
            SolidBrush transparentRed = new SolidBrush(Color.FromArgb(192, 255, 0, 0));
             SolidBrush transparentBlack = new SolidBrush(Color.FromArgb(192, 0, 0, 0));
            SolidBrush transparentQueen = new SolidBrush(Color.FromArgb(255, 255, 255, 0));


            Bitmap final = new Bitmap(500, 500);
            Graphics g = Graphics.FromImage(final);

            int noSteps = 25;

            for (int j = 1; j < noSteps; j++)
            {
                g.DrawImage(board, 0, 0);

                for (int i = 0; i < b1.Pieces.Count; i++)
                {
                    // daca piesa este capturata , nu mai este afisata
                    if (b2.Pieces[i].X == -1 && b2.Pieces[i].Y == -1) 
                    {
                        continue; 
                    }

                    double avx = (j * b2.Pieces[i].X + (noSteps - j) * b1.Pieces[i].X) / (double)noSteps;
                    double avy = (j * b2.Pieces[i].Y + (noSteps - j) * b1.Pieces[i].Y) / (double)noSteps;

                    SolidBrush brush = transparentRed;
                    if (b1.Pieces[i].Player == PlayerType.Human)
                        brush = transparentBlack;

                    //g.FillEllipse(brush, (int)(12 + avx * 125), (int)(dy - avy * 125), 100, 100);
                    g.FillEllipse(brush, (float)(6 + avx * 62.5), (float)(dy - avy * 62.5), 50, 50);

                    if (b1.Pieces[i].PieceType == PieceType.Queen)
                    {
                        brush = transparentQueen;
                        g.FillEllipse(brush, (float)(18 + avx * 62.5), (float)(dy - avy * 62.5 + 12), 25, 25);

                    }
                }

                Graphics pbg = pictureBoxBoard.CreateGraphics();
                pbg.DrawImage(final, 0, 0);
            }
        }

        private void jocNouToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            _board = new Board();
            _currentPlayer = PlayerType.Computer;
            ComputerMove();
        }

        private void despreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            const string copyright =
                "Algoritmul minimax\r\n" +
                "Inteligenta artificiala, Laboratorul 8\r\n" +
                "(c)2016-2017 Florin Leon\r\n" +
                "http://florinleon.byethost24.com/lab_ia.htm";

            MessageBox.Show(copyright, "Despre jocul Dame simple");
        }

        private void iesireToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            Environment.Exit(0);
        }

        private void pictureBoxBoard_Click(object sender, EventArgs e)
        {
            pictureBoxBoard.BringToFront();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
           // pictureBox1.SendToBack();

        }
    }
}