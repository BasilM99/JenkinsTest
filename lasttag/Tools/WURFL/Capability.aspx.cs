using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Xml;
using System.Configuration;

namespace WURFL.Web
{
    public partial class _Default : System.Web.UI.Page
    {
       


        private string Cname = "Column1";
      

        public void listfill()
        {
            if (string.IsNullOrEmpty(txtUserAgent.Text))
            {
                Empty();
                return;
            }


            var device = WurflManager.Instance.GetDeviceForRequest(HttpUtility.UrlDecode(txtUserAgent.Text));
            if (device.UserAgent.ToString() != "")
            {
                IDictionary<string, string> caps = device.GetCapabilities();

                XmlDocument xml = new XmlDocument();

                string path = System.Configuration.ConfigurationManager.AppSettings["WurflStructure"]; //@" C:\Users\Alid\Desktop\WURFL\Wurfl_structure.xml";

                xml.Load(path);

                IDictionary<string, string> capabilities = new Dictionary<string, string>();
                XmlNodeList xnList = xml.SelectNodes("/root");
                if (xnList.Count > 0)
                {
                    foreach (XmlNode per in xnList[0].ChildNodes)
                    {
                        capabilities.Add(per.Name, "Capabilities");
                        foreach (XmlNode child in per.ChildNodes)
                        {

                            capabilities.Add(child.Name, caps[child.Name]);
                        }
                    }
                }

                GridView1.DataSource = capabilities;
                GridView1.DataBind();
            }




        }


        protected void Empty()
        {

            IDictionary<string, string> empty = new Dictionary<string, string>();
            empty.Add("", "");
            GridView1.DataSource = empty;
            GridView1.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Empty();
        }

        protected void Load_Click(object sender, EventArgs e)
        {

            listfill();


        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Text = Cname = "Key";
                e.Row.Cells[1].Text = "Value";
             
            }

            if (e.Row.Cells[1].Text == "Capabilities")

            {
                e.Row.BackColor = System.Drawing.Color.Silver;
                e.Row.Height = Unit.Pixel(30);
                e.Row.Cells[0].ForeColor = e.Row.Cells[1].ForeColor = System.Drawing.Color.Brown;
                e.Row.Cells[0].Font.Size = e.Row.Cells[1].Font.Size = FontUnit.Large;
                e.Row.Cells[0].Font.Bold = e.Row.Cells[1].Font.Bold = true;
                e.Row.Cells[1].Text = string.Empty;
               
            }
            

        }

    }
}
