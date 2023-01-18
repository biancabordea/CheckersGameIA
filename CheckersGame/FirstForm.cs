using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleCheckers
{
    public partial class FirstForm : Form
    {
        Thread th;
        public FirstForm()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void FirstForm_Load(object sender, EventArgs e)
        {

        }
        private void openNewForm()
        {
            Application.Run(new MainForm());
        }
        private void buttonPlay_Click_1(object sender, EventArgs e)
        {
            this.Close();
            th = new Thread(openNewForm);

            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }
    }
}
