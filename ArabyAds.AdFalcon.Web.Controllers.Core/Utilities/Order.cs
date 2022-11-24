using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Web.Controllers.Core.Utilities
{
    public class Order
    {
        public static void GetOrderSetting(string order, out string orderColumn, out string orderType)
        {
            Framework.ApplicationContext.Instance.Logger.Warn("func: GetOrderSetting : order = " + order);

            if (string.IsNullOrEmpty(order))
            {
                orderColumn = "CountryName";
                orderType = "asc";
            }
            else
            {
                orderColumn = order.Split('-')[0];
                orderType = order.Split('-')[1];
            }
            //TODO:Osaleh to add support for Grid sorting and remove this temp solution 
            if (orderColumn.EndsWith("Text"))
            {
                orderColumn = orderColumn.Replace("Text", "");
            }
            if (orderColumn == "Ctr")
            {
                orderColumn = "CTR";
            }
            if (orderColumn == "DateRange")
            {
                orderColumn = "Date";
            }
            Framework.ApplicationContext.Instance.Logger.Warn("end func: GetOrderSetting : order = " + order);

        }

    }
}
