using System;
using System.Collections;
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
    public partial class Form1 : Form
    {
        private string _sourcePath;
        private string _destPath;

        private IList<IPRange> _ipRanges;

        private string _insertScript = @"call insert_ip_targetings(@start_id ,@groupId,{0},{1},'{2}');";
        private string _nextItemScript = @"set @start_id=@start_id+1;";
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSource_Click(object sender, EventArgs e)
        {
            var result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                _sourcePath = openFileDialog1.FileName;
                lblSource.Text = _sourcePath;
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
            _ipRanges = new List<IPRange>();


            //read file text
            var text = System.IO.File.ReadAllText(_sourcePath);
            if (cbxIsCIDR.Checked)
            {
                ParseCidrIPs(text);
            }
            else
            {
                ParseIPs(text);
            }


            GenerateScript();

            MessageBox.Show("done");
        }

        private void ParseIPs(string text)
        {
            var ips = ParseIP(text);

            IPRange lstRange;
            foreach (var ip in ips)
            {
                lstRange = _ipRanges.LastOrDefault();

                if (lstRange == null)
                {
                    lstRange = new IPRange { StartRange = ip, EndRange = ip, Description = "" };
                    _ipRanges.Add(lstRange);
                    continue;
                }

                if (lstRange.EndRange + 1 == ip)
                {
                    lstRange.EndRange = ip;
                }
                else
                {
                    lstRange = new IPRange { StartRange = ip, EndRange = ip, Description = "" };
                    _ipRanges.Add(lstRange);
                }
            }
        }
        private List<long> ParseIP(string ipList)
        {
            var ips = new List<long>();
            //split it to lines
            var ipsStr = ipList.Split('\n').Select(x => x.Trim('\r')).ToList();
            //loop ips
            foreach (var ip in ipsStr)
            {
                //convert ip to int
                var ipValue = Helper.ConvertIPToNumber(ip);
                //store ip value in  list
                ips.Add(ipValue);
            }

            //sort the list
            ips.Sort();
            return ips;
        }

        #region Parse CIDR IP

        public static IPRange ParseCidrIP(string cidrIP)
        {
            return Network2IpRange(cidrIP);
        }

        private static IPRange Network2IpRange(string sNetwork)
        {
            var result = new IPRange();
            uint ip,
                /* ip address */
                 mask,
                /* subnet mask */
                 broadcast,
                /* Broadcast address */
                 network; /* Network address */

            int bits;

            string[] elements = sNetwork.Split(new Char[] { '/' });

            ip = IP2Int(elements[0]);
            bits = Convert.ToInt32(elements[1]);

            mask = ~(0xffffffff >> bits);

            network = ip & mask;
            broadcast = network + ~mask;

            var usableIps = (bits > 30) ? 0 : (broadcast - network - 1);

            if (usableIps <= 0)
            {
                result.StartRange = result.EndRange = 0;
            }
            else
            {
                result.StartRange = network + 1;
                result.EndRange = broadcast - 1;
            }

            return result;
        }

        private static uint IP2Int(string ipNumber)
        {
            uint ip = 0;
            string[] elements = ipNumber.Split(new Char[] { '.' });
            if (elements.Length == 4)
            {
                ip = Convert.ToUInt32(elements[0]) << 24;
                ip += Convert.ToUInt32(elements[1]) << 16;
                ip += Convert.ToUInt32(elements[2]) << 8;
                ip += Convert.ToUInt32(elements[3]);
            }
            return ip;
        }

        #endregion

        private void ParseCidrIPs(string ipList)
        {
            //split it to lines
            var ipsStr = ipList.Split('\n').Select(x => x.Trim('\r')).ToList();
            //loop ips
            foreach (var ip in ipsStr)
            {
                IPNetwork ipnetwork = IPNetwork.Parse(ip);
                var startRange = Helper.ConvertIPToNumber(ipnetwork.FirstUsable.ToString());
                var endRange = Helper.ConvertIPToNumber(ipnetwork.LastUsable.ToString());
               // _ipRanges.Add(new IPRange { StartRange = startRange, EndRange = endRange, Description = "" });
                var correctr = new IPRange {StartRange = startRange, EndRange = endRange, Description = ""};
                var testr = ParseCidrIP(ip);

                if (correctr.StartRange != testr.StartRange || correctr.EndRange != testr.EndRange)
                {
                    
                }

            }
        }

        private void GenerateScript()
        {
            var sp = new StringBuilder();
            sp.AppendLine("set @start_id =1000;set @groupId =0;");

            foreach (var ipRange in _ipRanges)
            {
                sp.AppendLine(string.Format(_insertScript, ipRange.StartRange, ipRange.EndRange, ipRange.Description));
                sp.AppendLine(_nextItemScript);
            }

            System.IO.File.WriteAllText(_destPath, sp.ToString());
        }

    }
}
