using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StegoCrypto
{
    public partial class Form_Menu : Form
    {
        public Form_Menu()
        {
            InitializeComponent();
        }

        private void FLSB_Click(object sender, EventArgs e)
        {
            Form_LSB_MRC4 FLSB = new Form_LSB_MRC4();
            this.Hide();
            FLSB.ShowDialog();
            this.Close();
        }

        private void FLSBMR_Click(object sender, EventArgs e)
        {
            Form_LSBMR_MRC4 FLSBMR = new Form_LSBMR_MRC4();
            this.Hide();
            FLSBMR.ShowDialog();
            this.Close();
        }
    }
}
