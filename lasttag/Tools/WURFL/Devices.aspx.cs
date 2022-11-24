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
using WURFL.Web;


namespace WURFL.Web
{
    public partial class devices : System.Web.UI.Page
    {

        List<Device> list;

        protected void fill()
        {

            if (BrandsList.SelectedIndex != 0)
            {

                list = WurflManager.caps[BrandsList.SelectedValue];

                DevicesList.DataSource = list;
                DevicesList.DataBind();

            }


        }


        public void listfill()
        {

            var all = WurflManager.Instance;

            var keys = WurflManager.caps.Keys.ToList();
            keys.Sort();

            BrandsList.DataSource = keys;
            BrandsList.DataBind();

        }

        protected void Empty()
        {

            var empty = new List<Device>();

            WurflManager.caps["empty"] = empty;

            var list = WurflManager.caps["empty"];



            DevicesList.DataSource = list;
            DevicesList.DataBind();

        }

        protected void changed(object sender, EventArgs e)
        {
            fill();
        }

        protected void Page_Load(object sender, EventArgs e)
        {


            if (!IsPostBack)
                listfill();


        }


        protected void Item_Bound(Object sender, DataListItemEventArgs e)
        {

            HyperLink newHyperLink = new HyperLink();
            var ob = e.Item.DataItem as Device;

            if (ob != null)
            {
                if (ob.MarketingName != "")
                {
                    newHyperLink.Text = ob.Name + "(" + ob.MarketingName + ")";
                    newHyperLink.ToolTip = newHyperLink.Text;
                }
                else
                {
                    newHyperLink.Text = ob.Name;
                    newHyperLink.ToolTip = ob.Name;
                }
                newHyperLink.NavigateUrl = "~/capabilities.aspx?val=" + ob.Id;

            }

            e.Item.Controls.RemoveAt(0);
            e.Item.Controls.Add(newHyperLink);
            e.Item.Width = new Unit("200px");

        }

        protected void Search_Click(object sender, EventArgs e)
        {

            string name = TextBox1.Text.ToLower();


            //s.Name.Contains(name) ||
            list = WurflManager.caps[BrandsList.SelectedValue];
            var newlist = list.Where((Device s) => s.Name.ToLower().Contains(name) || s.MarketingName.ToLower().Contains(name)).ToList();

            if (name != "" && newlist.Count < 1)
            {
                div1.Attributes["style"] = "display: none;";
                sms.Attributes["style"] = "display: inline;";
            }
            else
            {

                div1.Attributes["style"] = "display: inline;";
                sms.Attributes["style"] = "display: none;";
            }

            DevicesList.DataSource = newlist;
            DevicesList.DataBind();


        }
    }
}