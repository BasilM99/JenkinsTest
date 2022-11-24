using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Web.Controllers.Utilities
{
    public class GoogleDataTableColumn
    {
        public string Id { get; set; }

        public string Label { get; set; }

        public string Type { get; set; }
    }

    
    public static class GoogleControlsHelper
    {
        /// <summary>
        /// Convert .Net Dictionary to Google DataTable structure, this DataTable will be 
        /// user as data source for
        /// on of Google controls
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rows"></param>
        /// <param name="columnsDefintion"></param>
        /// <returns></returns>
        public static string ConvertIListToDataTable<T>(IList<T> rows, List<GoogleDataTableColumn> columnsDefintion)
            where T: class
        {
            StringBuilder dataTable = new StringBuilder("{");

            if (columnsDefintion == null || columnsDefintion.Count == 0) { throw new ArgumentException("columnsDefintion parameter can not be null or empty"); }

            StringBuilder columnsBuilder = new StringBuilder("");

            if (columnsDefintion != null && columnsDefintion.Count() != 0)
            {

                var columnsCounter = 0;
                var columnsTotalCounter = columnsDefintion.Count;

                // Build column string
                //Example: {id: 'startDate', label: 'Start Date'}
                foreach (var item in columnsDefintion)
                {
                    columnsBuilder.Append("{");
                    columnsBuilder.Append(string.Format("\"id\":\"{0}\",", item.Id));
                    columnsBuilder.Append(string.Format("\"label\":\"{0}\",", item.Label));
                    columnsBuilder.Append(string.Format("\"type\":\"{0}\"", item.Type));

                    columnsBuilder.Append("}");

                    if (columnsCounter != columnsTotalCounter - 1) { columnsBuilder.Append(","); }

                    columnsCounter++;
                }

            }

            dataTable.Append(string.Format("\"cols\": [{0}],", columnsBuilder.ToString()));

            StringBuilder rowsBuilder = new StringBuilder("");

            if (rows != null && rows.Count() != 0)
            {
                var rowsCounter = 0;
                var rowsTotalCounter = rows.Count;

                // Get the public properties in this object to write them in the json data
                IList<PropertyInfo> objectProperties = GetAnonymousObjectPropertyNames(rows.First());

                int columnsPerRowCount = objectProperties.Count <= columnsDefintion.Count ? objectProperties.Count : columnsDefintion.Count;

                // Build row string
                // Example: {c:[{v: 'Bob'}, {v: new Date(2007, 5, 1)}]}
                foreach (var item in rows)
                {
                    rowsBuilder.Append("{\"c\":[");


                    for (int i = 0; i < columnsPerRowCount; i++)
                    {
                        var objectProperty = objectProperties[i];

                        if (objectProperty.PropertyType == typeof(string))
                        {
                            rowsBuilder.Append(string.Format("{{\"v\":\"{0}\"}}", objectProperty.GetValue(item)));
                        }
                        else
                        {
                            rowsBuilder.Append(string.Format("{{\"v\":{0}}}", objectProperty.GetValue(item)));
                        }

                        if (i != columnsPerRowCount - 1) { rowsBuilder.Append(","); }
                    }
                    
                    rowsBuilder.Append("]}");

                    if (rowsCounter != rowsTotalCounter - 1) { rowsBuilder.Append(","); }

                    rowsCounter++;
                }
            }

            dataTable.Append(string.Format("\"rows\": [{0}]", rowsBuilder.ToString()));


            // Close the Json object
             dataTable.Append("}");

             return dataTable.ToString();
        }

        #region Private Method


        private static IList<PropertyInfo> GetAnonymousObjectPropertyNames(object anaymousObject)
        {
            return anaymousObject.GetType().GetProperties();
        }

        #endregion
    }
}
