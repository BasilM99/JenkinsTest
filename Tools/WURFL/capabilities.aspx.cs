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
    public partial class capabilities : System.Web.UI.Page
    {
        private string Cname = "Column1";
        public string ID;

        public void listfill()
        {


            var device = WurflManager.Instance.GetDeviceById(ID);
            
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

            capabilities_GridView.DataSource = capabilities;
            capabilities_GridView.DataBind();


        }




        protected void Page_Load(object sender, EventArgs e)
        {
            ID = Request.QueryString["val"];

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