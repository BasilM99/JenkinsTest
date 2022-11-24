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
    public partial class marge : Form
    {
        public marge()
        {
            InitializeComponent();
        }
        private string _sourcePath;
        private string _source2Path;
        private string _destPath;


        private void btnSource_Click(object sender, EventArgs e)
        {
            var result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                _sourcePath = openFileDialog1.FileName;
                lblSource.Text = _sourcePath;
            }
        }
        private void btnSource2_Click(object sender, EventArgs e)
        {
            var result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                _source2Path = openFileDialog1.FileName;
                lblsource2.Text = _sourcePath;
            }
        }
        private void btnDest_Click(object sender, EventArgs e)
        {
            var result = saveFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                _destPath = saveFileDialog1.FileName;
                lblDest.Text = _destPath;
            }
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            //TODO:use list Union for merge  with excluding duplicates  
            var ips1 = Helper.ReadFile(_sourcePath);
            var ips2 = Helper.ReadFile(_source2Path);
            var result = new Dictionary<string, IP>();

            foreach (var ip in ips1)
            {
                if (!result.ContainsKey(ip.Value))
                {
                    result.Add(ip.Value, ip);
                }
            }

            foreach (var ip in ips2)
            {
                if (!result.ContainsKey(ip.Value))
                {
                    result.Add(ip.Value, ip);
                }
            }
            Helper.WriteFile(_destPath, result.Select(x => x.Value).ToList());
            MessageBox.Show("done");
        }


    }
}
