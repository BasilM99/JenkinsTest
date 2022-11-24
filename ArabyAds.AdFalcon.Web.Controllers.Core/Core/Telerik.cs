using System;
using System.Collections.Generic;
using System.Text;


   
    namespace Telerik.Web.Mvc
{
        public class GridModel
        {
            public GridModel() { }
        public GridModel(IEnumerable<object> Dataob) {

            Data = Dataob;
        }
        public IEnumerable<object> Data { get; set; }
            public int Total { get; set; }
         
            public object Errors { get; set; }
        }

    
    }


namespace Telerik.Web.Mvc.UI
{
    public class DropDownItem
    {
        public DropDownItem() { }

      
        public object Text { get; set; }

        public object Value { get; set; }
    }


}


namespace Telerik.Web.Mvc.Extensions
{
    public class GridColumnSettings
    {
        public GridColumnSettings() { }


        public object Text { get; set; }

        public object Value { get; set; }



         public object Title { get; set; }
        public object Member { get; set; }

        public object Format { get; set; }
    }


}

namespace System.Web.UI
{
    public class GridColumnSettingswe
    {
        public GridColumnSettingswe() { }


        public object Text { get; set; }

        public object Value { get; set; }
    }


}
namespace   System.Web.Security{

    public class GridColumnSettingswes
    {
        public GridColumnSettingswes() { }


        public object Text { get; set; }

        public object Value { get; set; }
    }
}