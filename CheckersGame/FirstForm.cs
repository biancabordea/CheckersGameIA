﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Threading;

namespace SimpleCheckers
{
    public partial class FirstForm : Form
    {
        Thread th;

        public FirstForm()
        {
            InitializeComponent();
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            this.Close();
            th = new Thread(openNewForm);

            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }
        private void openNewForm()
        {
            Application.Run(new MainForm());
        }
    }
}
