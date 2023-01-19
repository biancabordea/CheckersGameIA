using System;
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

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            const string copyright =
                "Joc Dame\r\n" +
                "Proiect Inteligenta Artificiala\r\n" +
                "Autori:\r\n" +
                "           - Bordea Bianca\r\n" +
                "           - Dumbrava Petronela\r\n" +                
                "           - Ghiorghiu Andreea\r\n" +
                "Grupa: 1411B\r\n\n" +
                "Regulile jocului: \r\n" +
                " - la începutul jocului toate piesele sunt dame \r\n" +
                " - damele se pot muta doar înainte pe diagonală, o singură poziție \r\n" +
                " - damele se pot muta două poziții (pătrate) doar dacă capturează o piesă a adversarului" +
                " (acesta trebuie să se afle la o distanță de o poziție, tot diagonal înainte) \r\n" +
                " - o damă devine regină în momentul în care ajunge pe ultima linie, din capătul opus al tablei, raportat la poziția inițială. \r\n" +
                " - o regină se poate deplasa în orice direcție, dar tot diagonal înainte \r\n" +
                "\r\n" +
                "";

            MessageBox.Show(copyright, "Despre jocul Dame simple");
        }
    }
}
