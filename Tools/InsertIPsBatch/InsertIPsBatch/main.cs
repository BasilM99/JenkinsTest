using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InsertIPsBatch
{
    public partial class main : Form
    {
        public main()
        {
            InitializeComponent();
        }

        private void btnGenerateTargetingScript_Click(object sender, EventArgs e)
        {
            var form = new Form1();
            form.ShowDialog();
        }

        private void btnMatch_Click(object sender, EventArgs e)
        {
            var form = new Match();
            form.ShowDialog();
        }

        private void btnMatch_Click_1(object sender, EventArgs e)
        {
            var form = new marge();
            form.ShowDialog();
        }
    }
}
