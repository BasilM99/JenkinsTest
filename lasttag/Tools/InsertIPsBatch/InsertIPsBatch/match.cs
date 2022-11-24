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
    public partial class Match : Form
    {
        private string mainSourceFilePath;
        private string sourceFilePath;
        private string foundFilePath;
        private string notFoundFilePath;
        public Match()
        {
            InitializeComponent();
        }

        #region Events
        private void btnMainSource_Click(object sender, EventArgs e)
        {
            var result = ofdMainSource.ShowDialog();
            if (result == DialogResult.OK)
            {
                mainSourceFilePath = ofdMainSource.FileName;
                lblMainSource.Text = mainSourceFilePath;
            }
        }

        private void btnSource_Click(object sender, EventArgs e)
        {
            var result = ofdSource.ShowDialog();
            if (result == DialogResult.OK)
            {
                sourceFilePath = ofdSource.FileName;
                lblSource.Text = sourceFilePath;
            }
        }

        private void btnFound_Click(object sender, EventArgs e)
        {
            var result = sfdFound.ShowDialog();
            if (result == DialogResult.OK)
            {
                foundFilePath = sfdFound.FileName;
                lblFound.Text = foundFilePath;
            }
        }

        private void btnNotFound_Click(object sender, EventArgs e)
        {
            var result = sfdNotFound.ShowDialog();
            if (result == DialogResult.OK)
            {
                notFoundFilePath = sfdNotFound.FileName;
                lblNotFound.Text = notFoundFilePath;
            }
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {

            var found = new List<IP>();
            var notFound = new List<IP>();

            //read first source
            var mainIPs = Helper.ReadFile(mainSourceFilePath);
            //read second source
            var ips = Helper.ReadFile(sourceFilePath);

            pgbProcess.Maximum = ips.Count;
            pgbProcess.Value = 0;
            //loop thro the first source  and check if the ip is found in the second
            foreach (var ip in ips)
            {
                if (mainIPs.Any(x => x.ValueInt == ip.ValueInt))
                {
                    found.Add(ip);
                }
                else
                {
                    notFound.Add(ip);
                }
                pgbProcess.Value++;
                Application.DoEvents();
            }

            Helper.WriteFile(foundFilePath, found);
            Helper.WriteFile(notFoundFilePath, notFound);
            pgbProcess.Value = 0;
            MessageBox.Show("done");
        }
        #endregion



    }
}
