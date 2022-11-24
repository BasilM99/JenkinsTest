using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Noqoush.AdFalcon.AdServer.Integration;
using Noqoush.Framework.EventBroker;

namespace TestAPI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Noqoush.Framework.EventBroker.EventBroker.Instance.Raise(new EventArgsBase() { InstanceId =txtId.Text, EventName = comboBox1.SelectedItem.ToString() });
            Noqoush.Framework.EventBroker.EventBroker.Instance.Flush();
        }
    }
}
