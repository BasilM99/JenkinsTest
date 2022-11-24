using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text.pdf;
using System.Web;
using System.Web.Mvc;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports;
using System.IO;
using iTextSharp.text;

using Noqoush.Framework;
using Noqoush.AdFalcon.Common.UserInfo;
using Noqoush.AdFalcon.Domain.Common.Model.Account;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.Fund;
using Noqoush.AdFalcon.Domain.Common.Repositories.Core;
using Noqoush.AdFalcon.Domain.Common.Model.Core;
using Noqoush.AdFalcon.Services.Interfaces.Services.Reports;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports.Dashboard;
using Noqoush.AdFalcon.Services.Interfaces.Core;

namespace Noqoush.AdFalcon.Web.Controllers.Utilities
{
    public class WriteReportDocumentsHelper
    {
        private static Font _dispalyFont;
        private static Font _fontHeader;
        static readonly object LockObj = new object();

        public static Font GetFont()
        {
            if (_dispalyFont == null)
            {
                lock (LockObj)
                {
                    if (_dispalyFont == null)
                    {
                        var path = Environment.GetFolderPath(Environment.SpecialFolder.Fonts) + "\\arial.ttf";
                        BaseFont arial = BaseFont.CreateFont(path, "Identity-H", BaseFont.EMBEDDED);
                        _dispalyFont = new Font(arial, 8f, Font.NORMAL);
                    }
                }
            }
            return _dispalyFont;
        }
        public static Font GetHeaderFont()
        {
            if (_fontHeader == null)
            {
                lock (LockObj)
                {
                    if (_fontHeader == null)
                    {
                        var path = Environment.GetFolderPath(Environment.SpecialFolder.Fonts) + "\\arial.ttf";
                        BaseFont arial = BaseFont.CreateFont(path, "Identity-H", BaseFont.EMBEDDED);
                        _fontHeader = new Font(arial, 10f, 1, new BaseColor(System.Drawing.Color.Black));
                    }
                }
            }
            return _fontHeader;
        }
        public static PdfPCell GetCell(string value, Font font)
        {
            if (value == null)
            {

                value = string.Empty;
            }
            var cell = new PdfPCell(new Phrase(value, font));
            if (IsUnicode(value))
            {
                cell.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            }
            return cell;
        }
        public static PdfPTable GetTable(int numColumns)
        {
            var table = new PdfPTable(numColumns);
            if (Config.CurrentDirection.Equals("rtl", StringComparison.OrdinalIgnoreCase))
            {
                table.RunDirection = PdfWriter.RUN_DIRECTION_RTL;

            }
            return table;
        }

        private static bool IsUnicode(string content)
        {
            int i = 0;
            for (i = 1; i <= content.Length; i++)
            {
                int code = Convert.ToInt32(Convert.ToChar(content.Substring(i - 1, 1)));
                if (code < 0 || code > 255)
                {
                    return true;
                }
            }
            return false;
        }

        private static void addHeader(StringBuilder strWriter, List<KeyValueDto> labels)
        {

            for (int i = 0; i < labels.Count; i++)
            {
                strWriter.Append(labels[i].Key.ToString());
                strWriter.Append("\t");
                strWriter.Append(labels[i].Value.ToString());
                strWriter.Append("\n");
            }
        }
        private static void addHeader(iTextSharp.text.Document document, List<KeyValueDto> labels)
        {

            Paragraph para = new Paragraph("\n");

            PdfPTable table = WriteReportDocumentsHelper.GetTable(8);
            // table.HorizontalAlignment = 0;
            //table.WidthPercentage = 60;

            iTextSharp.text.Font fontHeader = WriteReportDocumentsHelper.GetHeaderFont();

            for (int i = 0; i < labels.Count; i++)
            {
                iTextSharp.text.Font font = WriteReportDocumentsHelper.GetFont();
                PdfPCell KeyCell = WriteReportDocumentsHelper.GetCell(labels[i].Key.ToString(), fontHeader);
                PdfPCell valueCell = WriteReportDocumentsHelper.GetCell(labels[i].Value.ToString(), fontHeader);


                KeyCell.Border =0;
                //KeyCell.HorizontalAlignment = 0;
                //valueCell.HorizontalAlignment = 0;
                valueCell.Border = 0;
                 valueCell.Colspan = 7;
                table.AddCell(KeyCell);
                table.AddCell(valueCell);




            }
            document.Add(table);
            document.Add(para);
        }
        public ActionResult BuildReportFile<T>(List<T> reportingList, string MatrixColumns, string exportType, List<KeyValueDto> keyValueDtos, string fileName = "") where T : BaseReportResult
        {
            FileStreamResult streamResult = null;

            if (!string.IsNullOrEmpty(MatrixColumns))
            {
                List<int> MatrixColumnsIds = MatrixColumns.Split(',')
                    .Where(x => !string.IsNullOrEmpty(x) && x != ",")
                    .Select(x => Convert.ToInt32(x)).ToList();
                MatrixColumnsIds = FixMetriceColumns(reportingList, MatrixColumnsIds);

                if (MatrixColumnsIds.Count > 0)
                {

                    iTextSharp.text.Font font = GetFont();
                    iTextSharp.text.Font fontHeader = GetHeaderFont();
                    IReportService _reportService = IoC.Instance.Resolve<IReportService>();

                    using (MemoryStream memDocument = new MemoryStream())
                    {
                        switch (exportType.ToLower())
                        {
                            case "pdf":
                                using (iTextSharp.text.Document document = new iTextSharp.text.Document())
                                {
                                    if (MatrixColumnsIds.Count > 10)
                                    {
                                        document.SetPageSize(PageSize.A4.Rotate());

                                        document.SetMargins(1, 1, 40, 40);
                                    }
                                    else
                                    {
                                        document.SetPageSize(PageSize.A4);

                                        document.SetMargins(10, 10, 40, 40);
                                    }


                                    using (PdfWriter writer = PdfWriter.GetInstance(document, memDocument))
                                    {
                                        writer.CloseStream = false;
                                        document.Open();
                                        addHeader(document, keyValueDtos);
                                        PdfPTable table = GetTable(MatrixColumnsIds.Count);

                                        foreach (int id in MatrixColumnsIds)
                                        {
                                            metriceColumnDto Column = _reportService.GetColumn(id);
                                            if (Column != null)
                                            {
                                                PdfPCell Header = GetCell(ResourcesUtilities.GetResource(Column.HeaderResourceKey, Column.HeaderResourceSet), fontHeader);

                                                table.AddCell(Header);
                                            }
                                        }
                                        foreach (object item in reportingList)
                                        {
                                            foreach (int id in MatrixColumnsIds)
                                            {
                                                metriceColumnDto Column = _reportService.GetColumn(id);
                                                if (Column != null)
                                                {

                                                    var valueobj = GenericExtensions.GetPropertyValue(item, Column.AppFieldName);
                                                    string value = "";
                                                    if (valueobj != null)
                                                    {
                                                        value = valueobj.ToString();
                                                    }
                                                    PdfPCell Cell = GetCell(value, font);
                                                    table.AddCell(Cell);

                                                }

                                            }

                                        }



                                        document.Add(table);
                                        document.Close();

                                        writer.Flush();
                                        memDocument.Flush();

                                    }
                                }

                                //prepare output stream
                                HttpContext.Current.Response.ContentType = "application/pdf";
                                HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.pdf", fileName));
                                HttpContext.Current.Response.Buffer = true;
                                HttpContext.Current.Response.Clear();
                                HttpContext.Current.Response.OutputStream.Write(memDocument.GetBuffer(), 0, memDocument.GetBuffer().Length);
                                HttpContext.Current.Response.OutputStream.Flush();
                                HttpContext.Current.Response.End();

                                streamResult = new FileStreamResult(new MemoryStream(), "application/pdf");
                                break;
                            case "excel":
                                StringBuilder strExcelWriter = new StringBuilder();
                                addHeader(strExcelWriter, keyValueDtos);
                                for (int i = 0; i < MatrixColumnsIds.Count; i++)
                                {
                                    metriceColumnDto Column = _reportService.GetColumn(MatrixColumnsIds[i]);
                                    if (Column != null)
                                    {
                                        strExcelWriter.Append(ResourcesUtilities.GetResource(Column.HeaderResourceKey, Column.HeaderResourceSet));

                                        if (i != MatrixColumnsIds.Count - 1)
                                            strExcelWriter.Append("\t");
                                        else
                                            strExcelWriter.Append("\n");
                                    }
                                }


                                foreach (object item in reportingList)
                                {
                                    (item as BaseReportResult).IsExport = true;

                                    for (int i = 0; i < MatrixColumnsIds.Count; i++)
                                    {
                                        metriceColumnDto Column = _reportService.GetColumn(MatrixColumnsIds[i]);
                                        if (Column != null)
                                        {

                                            var valueobj = GenericExtensions.GetPropertyValue(item, Column.AppFieldName);
                                            string value = "";
                                            if (valueobj != null)
                                            {
                                                value = valueobj.ToString();
                                            }

                                            strExcelWriter.Append(value);

                                            if (i != MatrixColumnsIds.Count - 1)
                                                strExcelWriter.Append("\t");
                                            else
                                                strExcelWriter.Append("\n");

                                        }

                                    }
                                }

                                HttpContext.Current.Response.Clear();
                                HttpContext.Current.Response.ClearHeaders();
                                HttpContext.Current.Response.ClearContent();
                                HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                                HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.xls", fileName));

                                HttpContext.Current.Response.HeaderEncoding = Encoding.UTF8;
                                HttpContext.Current.Response.AddHeader("Pragma", "public");
                                HttpContext.Current.Response.ContentEncoding = Encoding.Unicode;
                                // Start the feed with BOM
                                HttpContext.Current.Response.BinaryWrite(Encoding.Unicode.GetPreamble());
                                HttpContext.Current.Response.Write(strExcelWriter.ToString());
                                HttpContext.Current.Response.End();

                                streamResult = new FileStreamResult(new MemoryStream(), "application/vnd.ms-excel");
                                break;
                            case "csv":

                                StringBuilder strCSVWriter = new StringBuilder();
                                addHeader(strCSVWriter, keyValueDtos);

                                for (int i = 0; i < MatrixColumnsIds.Count; i++)
                                {
                                    metriceColumnDto Column = _reportService.GetColumn(MatrixColumnsIds[i]);
                                    if (Column != null)
                                    {
                                        strCSVWriter.Append(ResourcesUtilities.GetResource(Column.HeaderResourceKey, Column.HeaderResourceSet));

                                        if (i != MatrixColumnsIds.Count - 1)
                                            strCSVWriter.Append(",");
                                        else
                                            strCSVWriter.Append("\n");
                                    }
                                }


                                foreach (object item in reportingList)
                                {
                                    (item as BaseReportResult).IsExport = true;

                                    for (int i = 0; i < MatrixColumnsIds.Count; i++)
                                    {
                                        metriceColumnDto Column = _reportService.GetColumn(MatrixColumnsIds[i]);
                                        if (Column != null)
                                        {

                                            var valueobj = GenericExtensions.GetPropertyValue(item, Column.AppFieldName);
                                            string value = "";
                                            if (valueobj != null)
                                            {
                                                value = valueobj.ToString();
                                            }
                                            strCSVWriter.Append(value);

                                            if (i != MatrixColumnsIds.Count - 1)
                                                strCSVWriter.Append(",");
                                            else
                                                strCSVWriter.Append("\n");

                                        }

                                    }
                                }


                                HttpContext.Current.Response.Clear();
                                HttpContext.Current.Response.ClearHeaders();
                                HttpContext.Current.Response.ClearContent();
                                HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.csv", fileName));

                                HttpContext.Current.Response.ContentType = "text/csv";
                                HttpContext.Current.Response.HeaderEncoding = Encoding.UTF8;
                                HttpContext.Current.Response.AddHeader("Pragma", "public");
                                HttpContext.Current.Response.ContentEncoding = Encoding.Unicode;
                                // Start the feed with BOM
                                HttpContext.Current.Response.BinaryWrite(Encoding.Unicode.GetPreamble());
                                HttpContext.Current.Response.Write(strCSVWriter.ToString());
                                HttpContext.Current.Response.End();
                                streamResult = new FileStreamResult(new MemoryStream(), "text/csv");
                                break;
                            default:
                                streamResult = new FileStreamResult(memDocument, "text/plain");
                                break;
                        }
                    }
                }
            }
            return streamResult;
        }



        public ActionResult BuildFundTransactionFile(List<FundTransactionDto> reportingList, string exportType, bool Details, List<KeyValueDto> keyValueDtos, string fileName = "")
        {
            using (MemoryStream memDocument = new MemoryStream())
            {
                FileStreamResult streamResult;


                switch (exportType.ToLower())
                {
                    case "pdf":
                        using (iTextSharp.text.Document document = new iTextSharp.text.Document(PageSize.A4, 10, 10, 40, 40))
                        {

                            using (PdfWriter writer = PdfWriter.GetInstance(document, memDocument))
                            {
                                writer.CloseStream = false;
                                document.Open();
                                addHeader(document, keyValueDtos);
                                iTextSharp.text.Font fontHeader = WriteReportDocumentsHelper.GetHeaderFont();
                                int columnCounter = 0;
                                if (!Details)
                                    columnCounter = 4;
                                else
                                    columnCounter = 5;
                                PdfPTable table = GetTable(columnCounter);

                                PdfPCell TransactionDateHeader = GetCell(ResourcesUtilities.GetResource("TransactionDate", "AccountHistory"), fontHeader);
                                PdfPCell NameHeader = GetCell(ResourcesUtilities.GetResource("Name", "Global"), fontHeader);
                                PdfPCell CountryHeader = GetCell(ResourcesUtilities.GetResource("Country", "Global"), fontHeader);
                                PdfPCell AmountHeader = GetCell(ResourcesUtilities.GetResource("Amount", "AddFund"), fontHeader);
                                PdfPCell VATAmountHeader = GetCell(ResourcesUtilities.GetResource("VATAmount", "Global"), fontHeader);
                                if (Details)
                                    table.AddCell(TransactionDateHeader);
                                table.AddCell(NameHeader);
                                table.AddCell(CountryHeader);
                                table.AddCell(AmountHeader);
                                table.AddCell(VATAmountHeader);


                                foreach (var item in reportingList)
                                {
                                    iTextSharp.text.Font font = WriteReportDocumentsHelper.GetFont();
                                    PdfPCell TransactionDate = GetCell(item.TransactionDate.ToShortDateString(), font);
                                    PdfPCell AccountName = GetCell(item.AccountName, font);
                                    PdfPCell Country = GetCell(item.Country, font);
                                    PdfPCell Amount = GetCell(item.AmountText.ToString(), font);
                                    PdfPCell VATAmount = GetCell(item.VATAmountText.ToString(), font);


                                    if (Details)
                                        table.AddCell(TransactionDate);
                                    table.AddCell(AccountName);
                                    table.AddCell(Country);
                                    table.AddCell(Amount);
                                    table.AddCell(VATAmount);
                                }

                                document.Add(table);
                                document.Close();

                                writer.Flush();
                                memDocument.Flush();
                            }
                        }

                        //prepare output stream
                        HttpContext.Current.Response.ContentType = "application/pdf";
                        //HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=Reports.pdf");
                        HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.pdf", fileName));

                        HttpContext.Current.Response.Buffer = true;
                        HttpContext.Current.Response.Clear();
                        HttpContext.Current.Response.OutputStream.Write(memDocument.GetBuffer(), 0, memDocument.GetBuffer().Length);
                        HttpContext.Current.Response.OutputStream.Flush();
                        HttpContext.Current.Response.End();

                        streamResult = new FileStreamResult(new MemoryStream(), "application/pdf");
                        break;
                    case "excel":
                        StringBuilder strExcelWriter = new StringBuilder();
                        addHeader(strExcelWriter, keyValueDtos);
                        if (Details)
                        {
                            strExcelWriter.Append(ResourcesUtilities.GetResource("TransactionDate", "AccountHistory"));
                            strExcelWriter.Append("\t");
                        }

                        strExcelWriter.Append(ResourcesUtilities.GetResource("Name", "Global"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(ResourcesUtilities.GetResource("Country", "Global"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(ResourcesUtilities.GetResource("Amount", "AddFund"));

                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(ResourcesUtilities.GetResource("VATAmount", "Global"));


                        strExcelWriter.Append("\n");

                        foreach (var item in reportingList)
                        {
                            if (Details)
                            {
                                strExcelWriter.Append(item.TransactionDate.ToShortDateString());
                                strExcelWriter.Append("\t");
                            }

                            strExcelWriter.Append(item.AccountName);
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.Country);
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.AmountText);
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.VATAmountText);
                            strExcelWriter.Append("\n");
                        }


                        HttpContext.Current.Response.Clear();
                        HttpContext.Current.Response.ClearHeaders();
                        HttpContext.Current.Response.ClearContent();
                        HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                        HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.xls", fileName));

                        HttpContext.Current.Response.HeaderEncoding = Encoding.UTF8;
                        HttpContext.Current.Response.AddHeader("Pragma", "public");
                        HttpContext.Current.Response.ContentEncoding = Encoding.Unicode;
                        // Start the feed with BOM
                        HttpContext.Current.Response.BinaryWrite(Encoding.Unicode.GetPreamble());
                        HttpContext.Current.Response.Write(strExcelWriter.ToString());
                        HttpContext.Current.Response.End();

                        streamResult = new FileStreamResult(new MemoryStream(), "application/vnd.ms-excel");
                        break;
                    case "csv":
                        StringBuilder strCSVWriter = new StringBuilder();
                        addHeader(strCSVWriter, keyValueDtos);
                        if (Details)
                        {
                            strCSVWriter.Append(ResourcesUtilities.GetResource("TransactionDate", "AccountHistory"));
                            strCSVWriter.Append(",");
                        }

                        strCSVWriter.Append(ResourcesUtilities.GetResource("Name", "Global"));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(ResourcesUtilities.GetResource("Country", "Global"));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(ResourcesUtilities.GetResource("Amount", "AddFund"));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(ResourcesUtilities.GetResource("VATAmount", "Global"));

                        strCSVWriter.Append("\n");

                        foreach (var item in reportingList)
                        {

                            if (Details)
                            {
                                strCSVWriter.Append(item.TransactionDate.ToShortDateString());
                                strCSVWriter.Append(",");

                            }
                            strCSVWriter.Append(item.AccountName);
                            strCSVWriter.Append(",");

                            strCSVWriter.Append(item.Country);
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(item.AmountText);
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(item.VATAmountText);
                            strCSVWriter.Append("\n");
                        }



                        HttpContext.Current.Response.Clear();
                        HttpContext.Current.Response.ClearHeaders();
                        HttpContext.Current.Response.ClearContent();
                        HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.csv", fileName));

                        HttpContext.Current.Response.ContentType = "text/csv";
                        HttpContext.Current.Response.HeaderEncoding = Encoding.UTF8;
                        HttpContext.Current.Response.AddHeader("Pragma", "public");
                        HttpContext.Current.Response.ContentEncoding = Encoding.Unicode;
                        // Start the feed with BOM
                        HttpContext.Current.Response.BinaryWrite(Encoding.Unicode.GetPreamble());
                        HttpContext.Current.Response.Write(strCSVWriter.ToString());
                        HttpContext.Current.Response.End();
                        streamResult = new FileStreamResult(new MemoryStream(), "text/csv");
                        break;
                    default:
                        streamResult = new FileStreamResult(memDocument, "text/plain");
                        break;
                }

                return streamResult;
            }
        }


        private List<int> FixMetriceColumns<T>(List<T> DataSource, List<int> MetriceColumns) where T : BaseReportResult
        {
            IReportService _reportService = IoC.Instance.Resolve<IReportService>();
            if (MetriceColumns.Count > 0)
            {
                if (typeof(T) == typeof(CampaignCommonReportDto))
                {
                    List<CampaignCommonReportDto> TempDataSource = DataSource as List<CampaignCommonReportDto>;
                    bool ShowName = TempDataSource.Where(x => !string.IsNullOrEmpty(x.Name)).Count() != 0;
                    bool ShowSubName = TempDataSource.Where(x => !string.IsNullOrEmpty(x.SubName)).Count() != 0;
                    bool ShowDateRange = TempDataSource.Where(x => !string.IsNullOrEmpty(x.DateRange)).Count() != 0;

                    if (!ShowName || !ShowSubName || ShowDateRange)
                    {
                        if (!ShowName)
                        {
                            int NameId = _reportService.GetColumnId("Name", false);
                            MetriceColumns = MetriceColumns.Where(x => x != NameId).ToList();
                        }
                        if (!ShowSubName)
                        {
                            int SubNameId = _reportService.GetColumnId("SubName", false);
                            MetriceColumns = MetriceColumns.Where(x => x != SubNameId).ToList();
                        }
                        if (!ShowDateRange)
                        {
                            int DateRangeId = _reportService.GetColumnId("DateRangeProp", false);
                            MetriceColumns = MetriceColumns.Where(x => x != DateRangeId).ToList();
                        }
                    }

                }
                else if (typeof(T) == typeof(AppCommonReportDto))
                {
                    List<AppCommonReportDto> TempDataSource = DataSource as List<AppCommonReportDto>;

                    bool ShowName = TempDataSource.Where(x => !string.IsNullOrEmpty(x.Name)).Count() != 0;
                    bool ShowSubName = TempDataSource.Where(x => !string.IsNullOrEmpty(x.SubName)).Count() != 0;
                    bool ShowDateRange = TempDataSource.Where(x => !string.IsNullOrEmpty(x.DateRange)).Count() != 0;

                    if (!ShowName || !ShowSubName || ShowDateRange)
                    {
                        if (!ShowName)
                        {
                            int NameId = _reportService.GetColumnId("Name", true);
                            MetriceColumns = MetriceColumns.Where(x => x != NameId).ToList();
                        }
                        if (!ShowSubName)
                        {
                            int SubNameId = _reportService.GetColumnId("SubName", true);
                            MetriceColumns = MetriceColumns.Where(x => x != SubNameId).ToList();
                        }
                        if (!ShowDateRange)
                        {
                            int DateRangeId = _reportService.GetColumnId("DateRangeProp", true);
                            MetriceColumns = MetriceColumns.Where(x => x != DateRangeId).ToList();
                        }
                    }

                }
            }
            return MetriceColumns;

        }

    }
}
