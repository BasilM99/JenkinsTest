using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text.pdf;

using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports;
using System.IO;
using iTextSharp.text;
using Noqoush.Framework;
using Noqoush.AdFalcon.Domain.Common.Model.Account;
using Noqoush.AdFalcon.Services.Interfaces.Services.Reports;
using Noqoush.AdFalcon.Domain.Common.Model.Core;
using Noqoush.AdFalcon.Services.Interfaces.Core;

namespace Noqoush.AdFalcon.AdFalconPortalMaintenanceJob.ReportSchedule
{

    public class WriteReportDocumentsHelper
    {
        private Noqoush.Framework.Resources.IResourceService _ResourceService;
        private Noqoush.AdFalcon.Services.Interfaces.Services.Account.IAccountService _AccountService;

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

        public string BuildAppReportFile(List<AppCommonReportDto> reportingList, string exportType, ReportSchedulerDto dto, ref string emailBoday)
        {
            BaseAppSiteResultDto Totalsamp = new BaseAppSiteResultDto();
            _ResourceService = SchedulerHelper.ResourceService;
            _AccountService = SchedulerHelper.AccountService;
            Totalsamp.IsExport = true;
            string completePath = string.Empty;
            using (MemoryStream memDocument = new MemoryStream())
            {

                bool showDateRange = true;
                if (reportingList.Where(p => string.IsNullOrEmpty(p.DateRange)).Count() == reportingList.Count)
                {
                    showDateRange = false;
                }
                bool showNameColumn, showSubNameColumn;
                showNameColumn = showSubNameColumn = true;

                if (reportingList.Where(p => string.IsNullOrEmpty(p.Name)).Count() == reportingList.Count)
                {
                    showNameColumn = false;
                }

                if (reportingList.Where(p => string.IsNullOrEmpty(p.SubName)).Count() == reportingList.Count)
                {
                    showSubNameColumn = false;
                }



                switch (exportType.ToLower())
                {
                    case "pdf":
                        using (iTextSharp.text.Document document = new iTextSharp.text.Document(PageSize.A4, 10, 10, 40, 40))
                        {

                            using (PdfWriter writer = PdfWriter.GetInstance(document, memDocument))
                            {
                                writer.CloseStream = false;
                                document.Open();

                                iTextSharp.text.Font fontHeader = WriteReportDocumentsHelper.GetHeaderFont();

                                int columnCounter = 7;
                                if (showDateRange)
                                { columnCounter++; }
                                if (showNameColumn)
                                    columnCounter++;
                                if (showSubNameColumn)
                                    columnCounter++;
                                if (_AccountService.GetAccountRole(dto.AccountId) == (int)AccountRole.DSP)
                                {
                                    columnCounter += 2;
                                }

                                PdfPTable table = GetTable(columnCounter);

                                PdfPCell dateRangeHeader = GetCell(_ResourceService.GetResource("DateRange", "Report"), fontHeader);

                                PdfPCell adRequestsHeader = GetCell(_ResourceService.GetResource("AdRequests", "AppChart"), fontHeader);
                                PdfPCell adImpressHeader = GetCell(_ResourceService.GetResource("AdImpress", "AppChart"), fontHeader);
                                PdfPCell adClicksHeader = GetCell(_ResourceService.GetResource("AdClicks", "AppChart"), fontHeader);
                                PdfPCell fillRateHeader = GetCell(_ResourceService.GetResource("FillRate", "AppChart"), fontHeader);
                                PdfPCell CTRHeader = GetCell(_ResourceService.GetResource("CTR", "AppChart"), fontHeader);
                                PdfPCell eCPMHeader = GetCell(_ResourceService.GetResource("eCPM", "AppChart"), fontHeader);
                                PdfPCell revenueHeader = GetCell(_ResourceService.GetResource("Revenue", "AppChart"), fontHeader);

                                if (showDateRange)
                                    table.AddCell(dateRangeHeader);

                                if (showNameColumn)
                                {
                                    PdfPCell nameHeader = GetCell(_ResourceService.GetResource("Name", "AppReport"), fontHeader);
                                    table.AddCell(nameHeader);
                                }

                                if (showSubNameColumn)
                                {
                                    PdfPCell campainNameHeader = GetCell(_ResourceService.GetResource("AppSite", "AppChart"), fontHeader);
                                    table.AddCell(campainNameHeader);
                                }

                                table.AddCell(adRequestsHeader);
                                table.AddCell(adImpressHeader);
                                table.AddCell(adClicksHeader);
                                table.AddCell(fillRateHeader);
                                table.AddCell(CTRHeader);
                                table.AddCell(eCPMHeader);
                                table.AddCell(revenueHeader);

                                if (_AccountService.GetAccountRole(dto.AccountId) == (int)AccountRole.DSP)
                                {
                                    PdfPCell WinRateText = GetCell(_ResourceService.GetResource("WinRateText", "PMPDeal"), fontHeader);
                                    table.AddCell(WinRateText);
                                    PdfPCell DisplayRateText = GetCell(_ResourceService.GetResource("DisplayRateText", "PMPDeal"), fontHeader);
                                    table.AddCell(DisplayRateText);
                                }


                                foreach (var item in reportingList)
                                {
                                    iTextSharp.text.Font font = WriteReportDocumentsHelper.GetFont();

                                    PdfPCell dateRangeName = GetCell(item.DateRange, font);

                                    PdfPCell adRequests = GetCell(item.AdRequests.ToString(), font);
                                    PdfPCell adImpress = GetCell(item.AdImpress.ToString(), font);
                                    PdfPCell adClicks = GetCell(item.Clicks.ToString(), font);
                                    PdfPCell fillRate = GetCell(item.FillRateText, font);
                                    PdfPCell ctr = GetCell(item.CtrText, font);
                                    PdfPCell eCPM = GetCell(item.eCPMText, font);
                                    PdfPCell revenue = GetCell(item.RevenueText, font);
                                    PdfPCell WinRateText = GetCell(item.WinRate, font);
                                    PdfPCell DisplayRateText = GetCell(item.DisplayRate, font);
                                    if (showDateRange)
                                        table.AddCell(dateRangeName);

                                    if (showNameColumn)
                                    {
                                        table.AddCell(GetCell(item.Name, GetFont()));
                                    }

                                    if (showSubNameColumn)
                                    {
                                        table.AddCell(GetCell(item.SubName, GetFont()));
                                    }

                                    table.AddCell(adRequests);
                                    table.AddCell(adImpress);
                                    table.AddCell(adClicks);
                                    table.AddCell(fillRate);
                                    table.AddCell(ctr);
                                    table.AddCell(eCPM);
                                    table.AddCell(revenue);
                                    if (_AccountService.GetAccountRole(dto.AccountId) == (int)AccountRole.DSP)
                                    {
                                        table.AddCell(WinRateText);
                                        table.AddCell(DisplayRateText);
                                    }

                                }

                                document.Add(table);
                                document.Close();

                                writer.Flush();
                                memDocument.Flush();
                            }
                        }
                        //completePath = System.Configuration.ConfigurationManager.AppSettings["ReportFolderCreation"] + DateTime.UtcNow.Ticks.ToString() + ".pdf";

                        completePath = System.Configuration.ConfigurationManager.AppSettings["ReportFolderCreation"] + "AdFalcon_stats" + "_" + DateTime.UtcNow.Ticks.ToString() + ".pdf";


                        using (FileStream file = new FileStream(completePath, FileMode.Create, System.IO.FileAccess.Write))
                        {
                            byte[] bytes = new byte[memDocument.Length];
                            memDocument.Read(bytes, 0, (int)memDocument.Length);
                            file.Write(bytes, 0, bytes.Length);
                            memDocument.Close();
                        }

                        break;
                    case "excel":
                        StringBuilder strExcelWriter = new StringBuilder();
                        if (showDateRange)
                        {
                            strExcelWriter.Append(_ResourceService.GetResource("DateRange", "Report"));
                            strExcelWriter.Append("\t");
                        }

                        if (showNameColumn)
                        {
                            strExcelWriter.Append(_ResourceService.GetResource("Name", "AppReport"));
                            strExcelWriter.Append("\t");
                        }

                        if (showSubNameColumn)
                        {
                            strExcelWriter.Append(_ResourceService.GetResource("AppSite", "AppChart"));
                            strExcelWriter.Append("\t");
                        }

                        strExcelWriter.Append(_ResourceService.GetResource("AdRequests", "AppChart"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(_ResourceService.GetResource("AdImpress", "AppChart"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(_ResourceService.GetResource("AdClicks", "AppChart"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(_ResourceService.GetResource("FillRate", "AppChart"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(_ResourceService.GetResource("CTR", "AppChart"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(_ResourceService.GetResource("eCPM", "AppChart"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(_ResourceService.GetResource("Revenue", "AppChart"));

                        if (_AccountService.GetAccountRole(dto.AccountId) == (int)AccountRole.DSP)
                        {
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(_ResourceService.GetResource("WinRateText", "PMPDeal"));
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(_ResourceService.GetResource("DisplayRateText", "PMPDeal"));
                        }
                        strExcelWriter.Append("\n");

                        foreach (var item in reportingList)
                        {
                            item.IsExport = true;
                            if (showDateRange)
                            {
                                strExcelWriter.Append(item.DateRange);
                                strExcelWriter.Append("\t");
                            }
                            if (showNameColumn)
                            {
                                strExcelWriter.Append(item.Name);
                                strExcelWriter.Append("\t");
                            }

                            if (showSubNameColumn)
                            {
                                strExcelWriter.Append(item.SubName);
                                strExcelWriter.Append("\t");
                            }

                            strExcelWriter.Append(item.AdRequests);
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.AdImpress);
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.Clicks);
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.FillRate);
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.CtrText);
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.eCPMText);
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.RevenueText);

                            if (_AccountService.GetAccountRole(dto.AccountId) == (int)AccountRole.DSP)
                            {
                                strExcelWriter.Append("\t");
                                strExcelWriter.Append(item.WinRate);
                                strExcelWriter.Append("\t");
                                strExcelWriter.Append(item.DisplayRate);
                            }


                            strExcelWriter.Append("\n");
                        }


                        // completePath = System.Configuration.ConfigurationManager.AppSettings["ReportFolderCreation"] + DateTime.UtcNow.Ticks.ToString() + ".xls";


                        completePath = System.Configuration.ConfigurationManager.AppSettings["ReportFolderCreation"] + "AdFalcon_stats" + "_" + DateTime.UtcNow.Ticks.ToString() + ".xls";
                        using (FileStream file = new FileStream(completePath, FileMode.Create, System.IO.FileAccess.Write))
                        {
                            byte[] bytes = new byte[memDocument.Length];
                            memDocument.Read(bytes, 0, (int)memDocument.Length);
                            file.Write(bytes, 0, bytes.Length);
                            memDocument.Close();
                        }

                        break;
                    case "csv":
                        StringBuilder strCSVWriter = new StringBuilder();
                        strCSVWriter.Append("\n");
                        strCSVWriter.Append("\n");
                        var ReportTitle = _ResourceService.GetResource("ReportTitle", "Report", SchedulerHelper.GetCultureStr());

                        dto.ReportTitle = dto.ReportTitle.Replace(" ", "_").Replace("-", "_").Replace(":", "_").Replace("___", "_").Replace("__", "_");
                        emailBoday = emailBoday + "<b>" + ReportTitle + _ResourceService.GetResource("SemiColonSubject", "Report", SchedulerHelper.GetCultureStr()) + "</b>" + dto.ReportTitle + "<br/>";
                        strCSVWriter.Append(ReportTitle);
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(dto.ReportTitle);
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append("\n");
                        var DateTimeGenerated = _ResourceService.GetResource("DateTimeGenerated", "Report", SchedulerHelper.GetCultureStr());
                        var stringDate = String.Format(System.Configuration.ConfigurationManager.AppSettings["LongDateFormatService"], DateTime.Now);
                        emailBoday = emailBoday + "<b>" + DateTimeGenerated + _ResourceService.GetResource("SemiColonSubject", "Report", SchedulerHelper.GetCultureStr()) + "</b>" + stringDate + "<br/>";
                        strCSVWriter.Append(DateTimeGenerated);
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(stringDate);
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append("\n");
                        var TimeZoneGenerated = _ResourceService.GetResource("TimeZoneGenerated", "Report", SchedulerHelper.GetCultureStr());
                        var GMT = _ResourceService.GetResource("UTC", "Global");
                        emailBoday = emailBoday + "<b>" + TimeZoneGenerated + _ResourceService.GetResource("SemiColonSubject", "Report", SchedulerHelper.GetCultureStr()) + "</b>" + GMT + "<br/>";
                        strCSVWriter.Append(TimeZoneGenerated);
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(GMT);
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append("\n");

                        var ReportID = _ResourceService.GetResource("ReportID", "Report", SchedulerHelper.GetCultureStr());
                        emailBoday = emailBoday + "<b>" + ReportID + _ResourceService.GetResource("SemiColonSubject", "Report", SchedulerHelper.GetCultureStr()) + "</b>" + dto.ReportId + "<br/>";
                        strCSVWriter.Append(ReportID);
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(dto.ReportId);
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append("\n");

                        var DateRange = _ResourceService.GetResource("DateRange", "Report", SchedulerHelper.GetCultureStr());
                        emailBoday = emailBoday + "<b>" + DateRange + _ResourceService.GetResource("SemiColonSubject", "Report", SchedulerHelper.GetCultureStr()) + "</b>" + dto.ReportDto.FromDateString + _ResourceService.GetResource("ToSubject", "Report", SchedulerHelper.GetCultureStr()) + dto.ReportDto.ToDateString + "<br/>";
                        strCSVWriter.Append(DateRange);
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(dto.ReportDto.FromDateString + _ResourceService.GetResource("ToSubject", "Report", SchedulerHelper.GetCultureStr()) + dto.ReportDto.ToDateString);
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append("\n");
                        strCSVWriter.Append("\n");
                        strCSVWriter.Append("\n");
                        strCSVWriter.Append("\n");



                        strCSVWriter.Append(_ResourceService.GetResource("ReportFields", "Report", SchedulerHelper.GetCultureStr()));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append("\n");
                        if (showDateRange)
                        {
                            strCSVWriter.Append(_ResourceService.GetResource("DateRange", "Report", SchedulerHelper.GetCultureStr()));
                            strCSVWriter.Append(",");
                        }
                        if (showNameColumn)
                        {
                            strCSVWriter.Append(_ResourceService.GetResource("Name", "AppReport", SchedulerHelper.GetCultureStr()));
                            strCSVWriter.Append(",");
                        }

                        if (showSubNameColumn)
                        {
                            strCSVWriter.Append(_ResourceService.GetResource("AppSite", "AppChart", SchedulerHelper.GetCultureStr()));
                            strCSVWriter.Append(",");
                        }


                        strCSVWriter.Append(_ResourceService.GetResource("AdRequests", "AppChart", SchedulerHelper.GetCultureStr()));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(_ResourceService.GetResource("AdImpress", "AppChart", SchedulerHelper.GetCultureStr()));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(_ResourceService.GetResource("AdClicks", "AppChart", SchedulerHelper.GetCultureStr()));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(_ResourceService.GetResource("FillRate", "AppChart", SchedulerHelper.GetCultureStr()));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(_ResourceService.GetResource("CTR", "AppChart", SchedulerHelper.GetCultureStr()));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(_ResourceService.GetResource("eCPM", "AppChart", SchedulerHelper.GetCultureStr()));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(_ResourceService.GetResource("Revenue", "AppChart", SchedulerHelper.GetCultureStr()));

                        if (_AccountService.GetAccountRole(dto.AccountId) == (int)AccountRole.DSP)
                        {
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(_ResourceService.GetResource("WinRateText", "PMPDeal", SchedulerHelper.GetCultureStr()));
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(_ResourceService.GetResource("DisplayRateText", "PMPDeal", SchedulerHelper.GetCultureStr()));
                        }

                        strCSVWriter.Append("\n");
                        long AdRequests = 0;
                        long AdImpress = 0;
                        long Clicks = 0;
                        decimal Revenue = 0;
                        decimal eCPMv = 0;
                        foreach (var item in reportingList)
                        {
                            item.IsExport = true;
                            if (showDateRange)
                            {
                                strCSVWriter.Append(item.DateRange);
                                strCSVWriter.Append(",");
                            }
                            if (showNameColumn)
                            {
                                strCSVWriter.Append(item.Name);
                                strCSVWriter.Append(",");
                            }

                            if (showSubNameColumn)
                            {
                                strCSVWriter.Append(item.SubName);
                                strCSVWriter.Append(",");
                            }

                            AdRequests = AdRequests + item.AdRequests;
                            AdImpress = AdImpress + item.AdImpress;
                            Clicks = Clicks + item.Clicks;
                            Revenue = Revenue + item.Revenue;
                            eCPMv = eCPMv + item.eCPM;
                            strCSVWriter.Append(item.AdRequests);
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(item.AdImpress);
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(item.Clicks);
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(item.FillRateText);
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(item.CtrText);
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(item.eCPMText);
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(item.RevenueText);
                            if (_AccountService.GetAccountRole(dto.AccountId) == (int)AccountRole.DSP)
                            {
                                strCSVWriter.Append(",");
                                strCSVWriter.Append(item.WinRate);
                                strCSVWriter.Append(",");
                                strCSVWriter.Append(item.DisplayRate);
                            }


                            strCSVWriter.Append("\n");
                        }
                        if (reportingList != null && reportingList.Count > 1)
                        {
                            //completePath = System.Configuration.ConfigurationManager.AppSettings["ReportFolderCreation"] + DateTime.UtcNow.Ticks.ToString() + ".csv";
                            strCSVWriter.Append(_ResourceService.GetResource("GrandTotal", "Report", SchedulerHelper.GetCultureStr()));
                            strCSVWriter.Append(",");
                            Totalsamp.AdRequests = AdRequests;
                            Totalsamp.AdImpress = AdImpress;
                            Totalsamp.Clicks = Clicks;
                            Totalsamp.Revenue = Revenue;
                            // Totalsamp.eCPM = eCPMv;
                            if (showNameColumn && showDateRange)
                            {
                                strCSVWriter.Append("---");
                                strCSVWriter.Append(",");
                            }

                            if (showSubNameColumn && showDateRange)
                            {
                                strCSVWriter.Append("---");
                                strCSVWriter.Append(",");
                            }

                            strCSVWriter.Append(AdRequests);
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(AdImpress);
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(Clicks);
                            strCSVWriter.Append(",");

                            strCSVWriter.Append(Totalsamp.FillRateText);
                            strCSVWriter.Append(",");

                            strCSVWriter.Append(Totalsamp.CtrText);
                            strCSVWriter.Append(",");

                            strCSVWriter.Append(Totalsamp.eCPMText);
                            strCSVWriter.Append(",");
                            //strCSVWriter.Append(_ResourceService.GetResource("AvgCPC", "AdChart"));
                            //strCSVWriter.Append(",");
                            strCSVWriter.Append(Totalsamp.RevenueText);

                            if (_AccountService.GetAccountRole(dto.AccountId) == (int)AccountRole.DSP)
                            {
                                strCSVWriter.Append(",");
                                strCSVWriter.Append(Totalsamp.WinRate);
                                strCSVWriter.Append(",");
                                strCSVWriter.Append(Totalsamp.DisplayRate);
                            }

                            strCSVWriter.Append("\n");
                        }

                        completePath = System.Configuration.ConfigurationManager.AppSettings["ReportFolderCreation"] + dto.FileName + ".csv";
                        using (FileStream file = new FileStream(completePath, FileMode.Create, System.IO.FileAccess.Write))
                        {

                        }
                        File.WriteAllText(completePath, strCSVWriter.ToString(), Encoding.UTF8);

                        break;
                    default:

                        break;
                }

                return completePath;
            }
        }

        public string BuildCampaignReportFile(List<CampaignCommonReportDto> reportingList, string exportType, ReportSchedulerDto dto, ref string emailBoday, bool showCI = false)
        {
            CampaignCommonReportDto Totalsamp = new CampaignCommonReportDto();
            decimal CTR = 0;
            Totalsamp.IsExport = true;
            _ResourceService = SchedulerHelper.ResourceService;
            string completePath = string.Empty;
            using (MemoryStream memDocument = new MemoryStream())
            {
                bool showDateRange = true;

                bool showNameColumn, showSubNameColumn;
                showNameColumn = showSubNameColumn = true;
                if (reportingList.Where(p => string.IsNullOrEmpty(p.DateRange)).Count() == reportingList.Count)
                {
                    showDateRange = false;
                }

                if (reportingList.Where(p => string.IsNullOrEmpty(p.Name)).Count() == reportingList.Count)
                {
                    showNameColumn = false;
                }

                if (reportingList.Where(p => string.IsNullOrEmpty(p.SubName)).Count() == reportingList.Count)
                {
                    showSubNameColumn = false;
                }

                switch (exportType.ToLower())
                {
                    case "pdf":
                        using (iTextSharp.text.Document document = new iTextSharp.text.Document(PageSize.A4, 10, 10, 40, 40))
                        {

                            using (PdfWriter writer = PdfWriter.GetInstance(document, memDocument))
                            {
                                writer.CloseStream = false;
                                document.Open();

                                iTextSharp.text.Font fontHeader = WriteReportDocumentsHelper.GetHeaderFont();

                                int columnCounter = 5;
                                if (showDateRange)
                                    columnCounter++;
                                if (showNameColumn)
                                    columnCounter++;
                                if (showSubNameColumn)
                                    columnCounter++;
                                if (showCI)
                                    columnCounter += 2;

                                PdfPTable table = GetTable(columnCounter);

                                PdfPCell dateRangeHeader = GetCell(_ResourceService.GetResource("DateRange", "Report"), fontHeader);

                                PdfPCell impressHeader = GetCell(_ResourceService.GetResource("Impress", "AdChart"), fontHeader);
                                PdfPCell clicksHeader = GetCell(_ResourceService.GetResource("Clicks", "AdChart"), fontHeader);
                                PdfPCell ctrHeader = GetCell(_ResourceService.GetResource("CTR", "AdChart"), fontHeader);
                                PdfPCell avgCPCHeader = GetCell(_ResourceService.GetResource("AvgCPC", "AdChart"), fontHeader);
                                PdfPCell spendHeader = GetCell(_ResourceService.GetResource("BillableCost", "Global"), fontHeader);

                                if (showDateRange)
                                { table.AddCell(dateRangeHeader); }

                                if (showNameColumn)
                                {
                                    PdfPCell nameHeader = GetCell(_ResourceService.GetResource("Name", "CampaignReport"), fontHeader);
                                    table.AddCell(nameHeader);
                                }

                                if (showSubNameColumn)
                                {
                                    PdfPCell campainNameHeader = GetCell(_ResourceService.GetResource("CampaignName", "AdChart"), fontHeader);
                                    table.AddCell(campainNameHeader);
                                }

                                table.AddCell(impressHeader);
                                if (showCI)
                                {
                                    PdfPCell UniqueImpressions = GetCell(_ResourceService.GetResource("UniqueImp", "Report"), fontHeader);
                                    table.AddCell(UniqueImpressions);

                                }
                                table.AddCell(clicksHeader);
                                if (showCI)
                                {
                                    PdfPCell UniqueClicks = GetCell(_ResourceService.GetResource("UniqueClicks", "Report"), fontHeader);
                                    table.AddCell(UniqueClicks);
                                }
                                table.AddCell(ctrHeader);
                                table.AddCell(avgCPCHeader);
                                table.AddCell(spendHeader);


                                foreach (var item in reportingList)
                                {
                                    iTextSharp.text.Font font = WriteReportDocumentsHelper.GetFont();

                                    PdfPCell dateRangeName = GetCell(item.DateRange, font);

                                    PdfPCell impress = GetCell(item.Impress.ToString(), font);
                                    PdfPCell clicks = GetCell(item.Clicks.ToString(), font);
                                    PdfPCell ctr = GetCell(item.CtrText, font);
                                    PdfPCell avgCPC = GetCell(item.AvgCPCText, font);
                                    PdfPCell spend = GetCell(item.BillableCostText, font);
                                    if (showDateRange)
                                    { table.AddCell(dateRangeName); }

                                    if (showNameColumn)
                                    {
                                        var name = GetCell(item.Name, font);
                                        table.AddCell(name);
                                    }

                                    if (showSubNameColumn)
                                    {
                                        table.AddCell(GetCell(item.SubName, font));
                                    }

                                    table.AddCell(impress);
                                    if (showCI)
                                    {
                                        table.AddCell(GetCell(item.UniqueImp.ToString(), font));

                                    }
                                    table.AddCell(clicks);
                                    if (showCI)
                                    {
                                        table.AddCell(GetCell(item.UniqueClicks.ToString(), font));

                                    }
                                    table.AddCell(ctr);
                                    table.AddCell(avgCPC);
                                    table.AddCell(spend);


                                }

                                document.Add(table);
                                document.Close();

                                writer.Flush();
                                memDocument.Flush();
                            }
                        }

                        //prepare output stream
                        //  completePath = System.Configuration.ConfigurationManager.AppSettings["ReportFolderCreation"] + DateTime.UtcNow.Ticks.ToString() + ".pdf";

                        completePath = System.Configuration.ConfigurationManager.AppSettings["ReportFolderCreation"] + dto.FileName + ".pdf";
                        using (FileStream file = new FileStream(completePath, FileMode.Create, System.IO.FileAccess.Write))
                        {
                            byte[] bytes = new byte[memDocument.Length];
                            memDocument.Read(bytes, 0, (int)memDocument.Length);
                            file.Write(bytes, 0, bytes.Length);
                            memDocument.Close();
                        }



                        break;
                    case "excel":
                        StringBuilder strExcelWriter = new StringBuilder();
                        if (showDateRange)
                        {
                            strExcelWriter.Append(_ResourceService.GetResource("DateRange", "Report"));
                            strExcelWriter.Append("\t");
                        }
                        if (showNameColumn)
                        {
                            strExcelWriter.Append(_ResourceService.GetResource("Name", "CampaignReport"));
                            strExcelWriter.Append("\t");
                        }

                        if (showSubNameColumn)
                        {
                            strExcelWriter.Append(_ResourceService.GetResource("CampaignName", "AdChart"));
                            strExcelWriter.Append("\t");
                        }

                        strExcelWriter.Append(_ResourceService.GetResource("Impress", "AdChart"));
                        strExcelWriter.Append("\t");
                        if (showCI)
                        {
                            strExcelWriter.Append(_ResourceService.GetResource("UniqueImp", "Report"));
                            strExcelWriter.Append("\t");
                        }
                        strExcelWriter.Append(_ResourceService.GetResource("Clicks", "AdChart"));
                        strExcelWriter.Append("\t");
                        if (showCI)
                        {
                            strExcelWriter.Append(_ResourceService.GetResource("UniqueClicks", "Report"));
                            strExcelWriter.Append("\t");
                        }
                        strExcelWriter.Append(_ResourceService.GetResource("CTR", "AdChart"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(_ResourceService.GetResource("AvgCPC", "AdChart"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(_ResourceService.GetResource("BillableCost", "Global"));

                        strExcelWriter.Append("\n");

                        foreach (var item in reportingList)
                        {
                            item.IsExport = true;
                            if (showDateRange)
                            {
                                strExcelWriter.Append(item.DateRange);
                                strExcelWriter.Append("\t");
                            }
                            if (showNameColumn)
                            {
                                strExcelWriter.Append(item.Name);
                                strExcelWriter.Append("\t");
                            }

                            if (showSubNameColumn)
                            {
                                strExcelWriter.Append(item.SubName);
                                strExcelWriter.Append("\t");
                            }

                            strExcelWriter.Append(item.Impress);
                            strExcelWriter.Append("\t");
                            if (showCI)
                            {
                                strExcelWriter.Append(item.UniqueImp);
                                strExcelWriter.Append("\t");
                            }
                            strExcelWriter.Append(item.Clicks);
                            strExcelWriter.Append("\t");
                            if (showCI)
                            {
                                strExcelWriter.Append(item.UniqueClicks);
                                strExcelWriter.Append("\t");
                            }
                            strExcelWriter.Append(item.CtrText);
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.AvgCPCText);
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.BillableCostText);

                            strExcelWriter.Append("\n");
                        }



                        //completePath = System.Configuration.ConfigurationManager.AppSettings["ReportFolderCreation"] + DateTime.UtcNow.Ticks.ToString() + ".xls";

                        completePath = System.Configuration.ConfigurationManager.AppSettings["ReportFolderCreation"] + "AdFalcon_stats" + "_" + DateTime.UtcNow.Ticks.ToString() + ".xls";
                        using (FileStream file = new FileStream(completePath, FileMode.Create, System.IO.FileAccess.Write))
                        {
                            byte[] bytes = new byte[memDocument.Length];
                            memDocument.Read(bytes, 0, (int)memDocument.Length);
                            file.Write(bytes, 0, bytes.Length);
                            memDocument.Close();
                        }


                        break;
                    case "csv":
                        StringBuilder strCSVWriter = new StringBuilder();
                        strCSVWriter.Append("\n");
                        strCSVWriter.Append("\n");
                        var ReportTitle = _ResourceService.GetResource("ReportTitle", "Report", SchedulerHelper.GetCultureStr());
                        dto.ReportTitle = dto.ReportTitle.Replace(" ", "_").Replace("-", "_").Replace(":", "_").Replace("___", "_").Replace("__", "_"); ;
                        emailBoday = emailBoday + "<b>" + ReportTitle + _ResourceService.GetResource("SemiColonSubject", "Report", SchedulerHelper.GetCultureStr()) + "</b>" + dto.ReportTitle + "<br/>";
                        strCSVWriter.Append(ReportTitle);
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(dto.ReportTitle);
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append("\n");
                        var DateTimeGenerated = _ResourceService.GetResource("DateTimeGenerated", "Report", SchedulerHelper.GetCultureStr());
                        var stringDate = String.Format(System.Configuration.ConfigurationManager.AppSettings["LongDateFormatService"], DateTime.Now);
                        emailBoday = emailBoday + "<b>" + DateTimeGenerated + _ResourceService.GetResource("SemiColonSubject", "Report", SchedulerHelper.GetCultureStr()) + "</b>" + stringDate + "<br/>";
                        strCSVWriter.Append(DateTimeGenerated);
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(stringDate);
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append("\n");
                        var TimeZoneGenerated = _ResourceService.GetResource("TimeZoneGenerated", "Report", SchedulerHelper.GetCultureStr());
                        var GMT = _ResourceService.GetResource("UTC", "Global");
                        emailBoday = emailBoday + "<b>" + TimeZoneGenerated + _ResourceService.GetResource("SemiColonSubject", "Report", SchedulerHelper.GetCultureStr()) + "</b>" + GMT + "<br/>";
                        strCSVWriter.Append(TimeZoneGenerated);
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(GMT);
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append("\n");

                        var ReportID = _ResourceService.GetResource("ReportID", "Report", SchedulerHelper.GetCultureStr());
                        emailBoday = emailBoday + "<b>" + ReportID + _ResourceService.GetResource("SemiColonSubject", "Report", SchedulerHelper.GetCultureStr()) + "</b>" + dto.ReportId + "<br/>";
                        strCSVWriter.Append(ReportID);
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(dto.ReportId);
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append("\n");

                        var DateRange = _ResourceService.GetResource("DateRange", "Report", SchedulerHelper.GetCultureStr());
                        emailBoday = emailBoday + "<b>" + DateRange + _ResourceService.GetResource("SemiColonSubject", "Report", SchedulerHelper.GetCultureStr()) + "</b>" + dto.ReportDto.FromDateString + _ResourceService.GetResource("ToSubject", "Report", SchedulerHelper.GetCultureStr()) + dto.ReportDto.ToDateString + "<br/>";
                        strCSVWriter.Append(DateRange);
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(dto.ReportDto.FromDateString + _ResourceService.GetResource("ToSubject", "Report", SchedulerHelper.GetCultureStr()) + dto.ReportDto.ToDateString);
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append("\n");
                        strCSVWriter.Append("\n");
                        strCSVWriter.Append("\n");
                        strCSVWriter.Append("\n");




                        strCSVWriter.Append(_ResourceService.GetResource("ReportFields", "Report", SchedulerHelper.GetCultureStr()));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append("\n");
                        if (showDateRange)
                        {
                            strCSVWriter.Append(_ResourceService.GetResource("DateRange", "Report", SchedulerHelper.GetCultureStr()));
                            strCSVWriter.Append(",");
                        }
                        if (showNameColumn)
                        {
                            strCSVWriter.Append(_ResourceService.GetResource("Name", "CampaignReport", SchedulerHelper.GetCultureStr()));
                            strCSVWriter.Append(",");
                        }

                        if (showSubNameColumn)
                        {
                            strCSVWriter.Append(_ResourceService.GetResource("CampaignName", "AdChart", SchedulerHelper.GetCultureStr()));
                            strCSVWriter.Append(",");
                        }

                        strCSVWriter.Append(_ResourceService.GetResource("Impress", "AdChart", SchedulerHelper.GetCultureStr()));
                        strCSVWriter.Append(",");
                        if (showCI)
                        {
                            strCSVWriter.Append(_ResourceService.GetResource("UniqueImp", "Report"));
                            strCSVWriter.Append(",");
                        }
                        strCSVWriter.Append(_ResourceService.GetResource("Clicks", "AdChart", SchedulerHelper.GetCultureStr()));
                        strCSVWriter.Append(",");
                        if (showCI)
                        {
                            strCSVWriter.Append(_ResourceService.GetResource("UniqueClicks", "Report"));
                            strCSVWriter.Append(",");
                        }
                        strCSVWriter.Append(_ResourceService.GetResource("CTR", "AdChart", SchedulerHelper.GetCultureStr()));
                        strCSVWriter.Append(",");
                        //strCSVWriter.Append(_ResourceService.GetResource("AvgCPC", "AdChart"));
                        //strCSVWriter.Append(",");
                        strCSVWriter.Append(_ResourceService.GetResource("BillableCost", "Global", SchedulerHelper.GetCultureStr()));

                        strCSVWriter.Append("\n");
                        long Impress = 0;
                        long Clicks = 0;
                        decimal Spend = 0;
                        foreach (var item in reportingList)
                        {
                            item.IsExport = true;
                            if (showDateRange)
                            {
                                strCSVWriter.Append(item.DateRange);
                                strCSVWriter.Append(",");
                            }
                            if (showNameColumn)
                            {
                                strCSVWriter.Append(item.Name);
                                strCSVWriter.Append(",");
                            }

                            if (showSubNameColumn)
                            {
                                strCSVWriter.Append(item.SubName);
                                strCSVWriter.Append(",");
                            }
                            Impress = item.Impress + Impress;
                            Clicks = item.Clicks + Clicks;
                            Spend = item.BillableCost + Spend;
                            CTR = item.CTR + CTR;
                            strCSVWriter.Append(item.Impress);
                            strCSVWriter.Append(",");
                            if (showCI)
                            {
                                strCSVWriter.Append(item.UniqueImp);
                                strCSVWriter.Append(",");
                            }

                            strCSVWriter.Append(item.Clicks);
                            strCSVWriter.Append(",");
                            if (showCI)
                            {
                                strCSVWriter.Append(item.UniqueClicks);
                                strCSVWriter.Append(",");
                            }
                            strCSVWriter.Append(item.CtrText);
                            strCSVWriter.Append(",");
                            //strCSVWriter.Append(item.AvgCPC);
                            //strCSVWriter.Append(",");
                            strCSVWriter.Append(item.BillableCostText);

                            strCSVWriter.Append("\n");
                        }


                        if (reportingList != null && reportingList.Count > 1)
                        {
                            strCSVWriter.Append(_ResourceService.GetResource("GrandTotal", "Report", SchedulerHelper.GetCultureStr()));
                            strCSVWriter.Append(",");
                            Totalsamp.Impress = Impress;
                            Totalsamp.Clicks = Clicks;
                            Totalsamp.CTR = CTR;
                            Totalsamp.BillableCost = Spend;
                            if (showNameColumn && showDateRange)
                            {
                                strCSVWriter.Append("---");
                                strCSVWriter.Append(",");
                            }

                            if (showSubNameColumn && showDateRange)
                            {
                                strCSVWriter.Append("---");
                                strCSVWriter.Append(",");
                            }

                            strCSVWriter.Append(Impress);
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(Clicks);
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(Totalsamp.CtrText);
                            strCSVWriter.Append(",");
                            //strCSVWriter.Append(_ResourceService.GetResource("AvgCPC", "AdChart"));
                            //strCSVWriter.Append(",");
                            strCSVWriter.Append(Totalsamp.BillableCostText);

                            strCSVWriter.Append("\n");
                        }
                        //completePath = System.Configuration.ConfigurationManager.AppSettings["ReportFolderCreation"] + DateTime.UtcNow.Ticks.ToString() + ".csv";

                        completePath = System.Configuration.ConfigurationManager.AppSettings["ReportFolderCreation"] + dto.FileName + ".csv";
                        using (FileStream file = new FileStream(completePath, FileMode.Create, System.IO.FileAccess.Write))
                        {

                        }
                        File.WriteAllText(completePath, strCSVWriter.ToString(), Encoding.UTF8);
                        break;
                    default:
                        completePath = string.Empty;
                        break;
                }

                return completePath;
            }
        }
        public string BuildReportFile<T>(List<T> reportingList, string exportType, ReportSchedulerDto dto, ref string emailBoday, T Totalsamp, bool showImpClicksColumn = true, bool showuniqClicksColumn = true) where T : BaseReportResult, new()
        {
            IReportService _reportService = SchedulerHelper.ReportService;

            bool showDateRange = true;

            _ResourceService = SchedulerHelper.ResourceService;
            string completePath = string.Empty;
            bool showNameColumn, showSubNameColumn;
            showNameColumn = showSubNameColumn = true;
        
            


            if (reportingList.Where(p => string.IsNullOrEmpty(p.DateRange)).Count() == reportingList.Count)
            {
                showDateRange = false;
            }

            if (reportingList.Where(p => string.IsNullOrEmpty(p.Name)).Count() == reportingList.Count)
            {
                showNameColumn = false;
            }

            if (reportingList.Where(p => string.IsNullOrEmpty(p.SubName)).Count() == reportingList.Count)
            {
                showSubNameColumn = false;
            }

            

         
            int SubNameid = 0;

            int Nameid = 0;
            int DateRangeid = 0;

            int UniqImpid = 0;
            int UniqClickid = 0;


            if (showSubNameColumn && dto.MatixColumns != null && dto.MatixColumns.Count > 0)
            {
                SubNameid = _reportService.GetColumnId("SubName", dto.ReportSectionType == ReportSectionType.Publisher);
            }
            if (!showSubNameColumn && dto.MatixColumns != null && dto.MatixColumns.Count > 0)
            {
                SubNameid = _reportService.GetColumnId("SubName", dto.ReportSectionType == ReportSectionType.Publisher);
            }

            if (!showNameColumn && dto.MatixColumns != null && dto.MatixColumns.Count > 0)
            {
                Nameid = _reportService.GetColumnId("Name", dto.ReportSectionType == ReportSectionType.Publisher);
            }

            if (!showDateRange && dto.MatixColumns != null && dto.MatixColumns.Count > 0)
            {
                DateRangeid = _reportService.GetColumnId("DateRangeProp", dto.ReportSectionType == ReportSectionType.Publisher);
            }


            if (!showImpClicksColumn && dto.MatixColumns != null && dto.MatixColumns.Count > 0)
            {
                UniqImpid = _reportService.GetColumnId("UniqueImp", dto.ReportSectionType == ReportSectionType.Publisher);
            }

            if (!showuniqClicksColumn && dto.MatixColumns != null && dto.MatixColumns.Count > 0)
            {
                UniqClickid = _reportService.GetColumnId("UniqueClicks", dto.ReportSectionType == ReportSectionType.Publisher);
            }

            if (dto.MatixColumns != null && dto.MatixColumns.Count > 0)
            {
                List<int> MatrixColumnsIds = dto.MatixColumns.ToList();
                List<int> OrderMatrixColumnsIds = new List<int>() ;

                if (!showDateRange)
                {
                    MatrixColumnsIds = MatrixColumnsIds.Where(M=>M != DateRangeid).ToList();
                }

                if (!showNameColumn)
                {
                    MatrixColumnsIds = MatrixColumnsIds.Where(M => M != Nameid).ToList();
                }
                if (!showSubNameColumn)
                {
                    MatrixColumnsIds = MatrixColumnsIds.Where(M => M != SubNameid).ToList();
                }
                if (!showImpClicksColumn)
                {
                    MatrixColumnsIds = MatrixColumnsIds.Where(M => M != UniqImpid).ToList();
                }
                if (!showuniqClicksColumn)
                {
                    MatrixColumnsIds = MatrixColumnsIds.Where(M => M != UniqClickid).ToList();
                }
                if (showSubNameColumn && SubNameid != 0)
                {
                    if (dto.MatixColumns.Where(x => x == SubNameid).Count() == 0)
                    {
                        MatrixColumnsIds.Add(SubNameid);

                    }
                }
                List<metriceColumnDto> allcolumns =null ;
                    if (dto.ReportSectionType == ReportSectionType.Publisher)
                        allcolumns = _reportService.GetAllmetriceColumnsForPublisher();
                    else
                        allcolumns= _reportService.GetAllmetriceColumnsForAdvertiser();

                    foreach (var column in allcolumns)
                    {
                        if (!(MatrixColumnsIds.Where(x => x == column.Id).Count() == 0))
                        {
                            OrderMatrixColumnsIds.Add(column.Id);
                        }

                    }
                    MatrixColumnsIds = OrderMatrixColumnsIds;
                    dto.MatixColumns = MatrixColumnsIds;
               
               
                if (MatrixColumnsIds.Count > 0)
                {

                    iTextSharp.text.Font font = GetFont();
                    iTextSharp.text.Font fontHeader = GetHeaderFont();

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

                                        PdfPTable table = GetTable(MatrixColumnsIds.Count);

                                        foreach (int id in MatrixColumnsIds)
                                        {
                                            metriceColumnDto Column = _reportService.GetColumn(id);
                                            if (Column != null)
                                            {
                                                PdfPCell Header = GetCell(_ResourceService.GetResource(Column.HeaderResourceKey, Column.HeaderResourceSet), fontHeader);

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
                                completePath = System.Configuration.ConfigurationManager.AppSettings["ReportFolderCreation"] + dto.FileName + ".pdf";
                                using (FileStream file = new FileStream(completePath, FileMode.Create, System.IO.FileAccess.Write))
                                {
                                    byte[] bytes = new byte[memDocument.Length];
                                    memDocument.Read(bytes, 0, (int)memDocument.Length);
                                    file.Write(bytes, 0, bytes.Length);
                                    memDocument.Close();
                                }
                                break;
                            case "excel":
                                StringBuilder strExcelWriter = new StringBuilder();

                                for (int i = 0; i < MatrixColumnsIds.Count; i++)
                                {
                                    metriceColumnDto Column = _reportService.GetColumn(MatrixColumnsIds[i]);
                                    if (Column != null)
                                    {
                                        strExcelWriter.Append(_ResourceService.GetResource(Column.HeaderResourceKey, Column.HeaderResourceSet));

                                        if (i != MatrixColumnsIds.Count - 1)
                                            strExcelWriter.Append("\t");
                                        else
                                            strExcelWriter.Append("\n");
                                    }
                                }

                                foreach (object item in reportingList)
                                {
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

                                completePath = System.Configuration.ConfigurationManager.AppSettings["ReportFolderCreation"] + "AdFalcon_stats" + "_" + DateTime.UtcNow.Ticks.ToString() + ".xls";
                                using (FileStream file = new FileStream(completePath, FileMode.Create, System.IO.FileAccess.Write))
                                {
                                    byte[] bytes = new byte[memDocument.Length];
                                    memDocument.Read(bytes, 0, (int)memDocument.Length);
                                    file.Write(bytes, 0, bytes.Length);
                                    memDocument.Close();
                                }
                                break;
                            case "csv":

                                StringBuilder strCSVWriter = new StringBuilder();


                                strCSVWriter.Append("\n");
                                strCSVWriter.Append("\n");
                                var ReportTitle = _ResourceService.GetResource("ReportTitle", "Report", SchedulerHelper.GetCultureStr());
                                dto.ReportTitle = dto.ReportTitle.Replace(" ", "_").Replace("-", "_").Replace(":", "_").Replace("___", "_").Replace("__", "_"); ;
                                emailBoday = emailBoday + "<b>" + ReportTitle + _ResourceService.GetResource("SemiColonSubject", "Report", SchedulerHelper.GetCultureStr()) + "</b>" + dto.ReportTitle + "<br/>";
                                strCSVWriter.Append(ReportTitle);
                                strCSVWriter.Append(",");
                                strCSVWriter.Append(dto.ReportTitle);
                                strCSVWriter.Append(",");
                                strCSVWriter.Append(",");
                                strCSVWriter.Append(",");
                                strCSVWriter.Append(",");
                                strCSVWriter.Append(",");
                                strCSVWriter.Append(",");
                                strCSVWriter.Append(",");
                                strCSVWriter.Append("\n");
                                var DateTimeGenerated = _ResourceService.GetResource("DateTimeGenerated", "Report", SchedulerHelper.GetCultureStr());
                                var stringDate = String.Format(System.Configuration.ConfigurationManager.AppSettings["LongDateFormatService"], DateTime.Now);
                                emailBoday = emailBoday + "<b>" + DateTimeGenerated + _ResourceService.GetResource("SemiColonSubject", "Report", SchedulerHelper.GetCultureStr()) + "</b>" + stringDate + "<br/>";
                                strCSVWriter.Append(DateTimeGenerated);
                                strCSVWriter.Append(",");
                                strCSVWriter.Append(stringDate);
                                strCSVWriter.Append(",");
                                strCSVWriter.Append(",");
                                strCSVWriter.Append(",");
                                strCSVWriter.Append(",");
                                strCSVWriter.Append(",");
                                strCSVWriter.Append(",");
                                strCSVWriter.Append(",");
                                strCSVWriter.Append("\n");
                                var TimeZoneGenerated = _ResourceService.GetResource("TimeZoneGenerated", "Report", SchedulerHelper.GetCultureStr());
                                var GMT = _ResourceService.GetResource("UTC", "Global");
                                emailBoday = emailBoday + "<b>" + TimeZoneGenerated + _ResourceService.GetResource("SemiColonSubject", "Report", SchedulerHelper.GetCultureStr()) + "</b>" + GMT + "<br/>";
                                strCSVWriter.Append(TimeZoneGenerated);
                                strCSVWriter.Append(",");
                                strCSVWriter.Append(GMT);
                                strCSVWriter.Append(",");
                                strCSVWriter.Append(",");
                                strCSVWriter.Append(",");
                                strCSVWriter.Append(",");
                                strCSVWriter.Append(",");
                                strCSVWriter.Append(",");
                                strCSVWriter.Append("\n");

                                var ReportID = _ResourceService.GetResource("ReportID", "Report", SchedulerHelper.GetCultureStr());
                                emailBoday = emailBoday + "<b>" + ReportID + _ResourceService.GetResource("SemiColonSubject", "Report", SchedulerHelper.GetCultureStr()) + "</b>" + dto.ReportId + "<br/>";
                                strCSVWriter.Append(ReportID);
                                strCSVWriter.Append(",");
                                strCSVWriter.Append(dto.ReportId);
                                strCSVWriter.Append(",");
                                strCSVWriter.Append(",");
                                strCSVWriter.Append(",");
                                strCSVWriter.Append(",");
                                strCSVWriter.Append(",");
                                strCSVWriter.Append(",");
                                strCSVWriter.Append("\n");

                                var DateRange = _ResourceService.GetResource("DateRange", "Report", SchedulerHelper.GetCultureStr());
                                emailBoday = emailBoday + "<b>" + DateRange + _ResourceService.GetResource("SemiColonSubject", "Report", SchedulerHelper.GetCultureStr()) + "</b>" + dto.ReportDto.FromDateString + _ResourceService.GetResource("ToSubject", "Report", SchedulerHelper.GetCultureStr()) + dto.ReportDto.ToDateString + "<br/>";
                                strCSVWriter.Append(DateRange);
                                strCSVWriter.Append(",");
                                strCSVWriter.Append(dto.ReportDto.FromDateString + _ResourceService.GetResource("ToSubject", "Report", SchedulerHelper.GetCultureStr()) + dto.ReportDto.ToDateString);
                                strCSVWriter.Append(",");
                                strCSVWriter.Append(",");
                                strCSVWriter.Append(",");
                                strCSVWriter.Append(",");
                                strCSVWriter.Append(",");
                                strCSVWriter.Append(",");
                                strCSVWriter.Append("\n");
                                strCSVWriter.Append("\n");
                                strCSVWriter.Append("\n");
                                strCSVWriter.Append("\n");




                                strCSVWriter.Append(_ResourceService.GetResource("ReportFields", "Report", SchedulerHelper.GetCultureStr()));
                                strCSVWriter.Append(",");
                                strCSVWriter.Append(",");
                                strCSVWriter.Append(",");
                                strCSVWriter.Append(",");
                                strCSVWriter.Append(",");
                                strCSVWriter.Append(",");
                                strCSVWriter.Append(",");
                                strCSVWriter.Append(",");
                                strCSVWriter.Append("\n");
                                for (int i = 0; i < MatrixColumnsIds.Count; i++)
                                {
                                    metriceColumnDto Column = _reportService.GetColumn(MatrixColumnsIds[i]);
                                    if (Column != null)
                                    {
                                        strCSVWriter.Append(_ResourceService.GetResource(Column.HeaderResourceKey, Column.HeaderResourceSet));

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
                                // strCSVWriter.Append("\n");
                                if (reportingList != null && reportingList.Count > 1)
                                {
                                    strCSVWriter.Append(_ResourceService.GetResource("GrandTotal", "Report", SchedulerHelper.GetCultureStr()));
                                    strCSVWriter.Append(",");

                                    if (showNameColumn && showDateRange)
                                    {
                                        strCSVWriter.Append("---");
                                        strCSVWriter.Append(",");
                                    }

                                    if (showSubNameColumn && showDateRange)
                                    {
                                        strCSVWriter.Append("---");
                                        strCSVWriter.Append(",");
                                    }
                                    if (showSubNameColumn && showNameColumn && !showDateRange)
                                    {
                                        strCSVWriter.Append("---");
                                        strCSVWriter.Append(",");
                                    }

                                    for (int i = 0; i < MatrixColumnsIds.Count; i++)
                                    {
                                        metriceColumnDto Column = _reportService.GetColumn(MatrixColumnsIds[i]);
                                        if (Column != null && (Column.AppFieldName != "Name" && Column.AppFieldName != "SubName" && Column.AppFieldName != "DateRangeProp"))
                                        {
                                            var valueobj = GenericExtensions.GetPropertyValue(Totalsamp, Column.AppFieldName);
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

                                completePath = System.Configuration.ConfigurationManager.AppSettings["ReportFolderCreation"] + dto.FileName + ".csv";
                                using (FileStream file = new FileStream(completePath, FileMode.Create, System.IO.FileAccess.Write))
                                {

                                }
                                File.WriteAllText(completePath, strCSVWriter.ToString(), Encoding.UTF8);

                                break;
                            default:
                                completePath = string.Empty;
                                break;
                        }
                    }
                }
            }
            return completePath;
        }
        public string BuildCSVReportFile(List<StringBuilder> reportingList, string exportType, ReportSchedulerDto dto, ref string emailBoday)
        {
          

            _ResourceService = SchedulerHelper.ResourceService;
            string completePath = string.Empty;
            using (MemoryStream memDocument = new MemoryStream())
            {
                

          
             
                        StringBuilder strCSVWriter = new StringBuilder();
                        strCSVWriter.Append("\n");
                        strCSVWriter.Append("\n");
                        var ReportTitle = _ResourceService.GetResource("ReportTitle", "Report", SchedulerHelper.GetCultureStr());
                        dto.ReportTitle = dto.ReportTitle.Replace(" ", "_").Replace("-", "_").Replace(":", "_").Replace("___", "_").Replace("__", "_"); ;
                        emailBoday = emailBoday + "<b>" + ReportTitle + _ResourceService.GetResource("SemiColonSubject", "Report", SchedulerHelper.GetCultureStr()) + "</b>" + dto.ReportTitle + "<br/>";
                        strCSVWriter.Append(ReportTitle);
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(dto.ReportTitle);
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append("\n");
                        var DateTimeGenerated = _ResourceService.GetResource("DateTimeGenerated", "Report", SchedulerHelper.GetCultureStr());
                        var stringDate = String.Format(System.Configuration.ConfigurationManager.AppSettings["LongDateFormatService"], DateTime.Now);
                        emailBoday = emailBoday + "<b>" + DateTimeGenerated + _ResourceService.GetResource("SemiColonSubject", "Report", SchedulerHelper.GetCultureStr()) + "</b>" + stringDate + "<br/>";
                        strCSVWriter.Append(DateTimeGenerated);
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(stringDate);
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append("\n");
                        var TimeZoneGenerated = _ResourceService.GetResource("TimeZoneGenerated", "Report", SchedulerHelper.GetCultureStr());
                        var GMT = _ResourceService.GetResource("UTC", "Global");
                        emailBoday = emailBoday + "<b>" + TimeZoneGenerated + _ResourceService.GetResource("SemiColonSubject", "Report", SchedulerHelper.GetCultureStr()) + "</b>" + GMT + "<br/>";
                        strCSVWriter.Append(TimeZoneGenerated);
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(GMT);
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append("\n");

                        var ReportID = _ResourceService.GetResource("ReportID", "Report", SchedulerHelper.GetCultureStr());
                        emailBoday = emailBoday + "<b>" + ReportID + _ResourceService.GetResource("SemiColonSubject", "Report", SchedulerHelper.GetCultureStr()) + "</b>" + dto.ReportId + "<br/>";
                        strCSVWriter.Append(ReportID);
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(dto.ReportId);
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append("\n");

                        var DateRange = _ResourceService.GetResource("DateRange", "Report", SchedulerHelper.GetCultureStr());
                        emailBoday = emailBoday + "<b>" + DateRange + _ResourceService.GetResource("SemiColonSubject", "Report", SchedulerHelper.GetCultureStr()) + "</b>" + dto.ReportDto.FromDateString + _ResourceService.GetResource("ToSubject", "Report", SchedulerHelper.GetCultureStr()) + dto.ReportDto.ToDateString + "<br/>";
                        strCSVWriter.Append(DateRange);
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(dto.ReportDto.FromDateString + _ResourceService.GetResource("ToSubject", "Report", SchedulerHelper.GetCultureStr()) + dto.ReportDto.ToDateString);
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append("\n");
                        strCSVWriter.Append("\n");
                        strCSVWriter.Append("\n");
                        strCSVWriter.Append("\n");




                        strCSVWriter.Append(_ResourceService.GetResource("ReportFields", "Report", SchedulerHelper.GetCultureStr()));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(",");
                        strCSVWriter.Append("\n");
                        strCSVWriter.Append("\n");
                StringBuilder row;
                        foreach (var item in reportingList)
                        {
                         row= PrepareCSVPacket(item);

                                strCSVWriter.Append(row);
                        }
                        //completePath = System.Configuration.ConfigurationManager.AppSettings["ReportFolderCreation"] + DateTime.UtcNow.Ticks.ToString() + ".csv";

                        completePath = System.Configuration.ConfigurationManager.AppSettings["ReportFolderCreation"] + dto.FileName + ".csv";
                        using (FileStream file = new FileStream(completePath, FileMode.Create, System.IO.FileAccess.Write))
                        {

                        }
                        File.WriteAllText(completePath, strCSVWriter.ToString(), Encoding.UTF8);
                  
                }

           return completePath;
           
        }
      
        public StringBuilder PrepareCSVPacket(StringBuilder row)
        {
            List<string> cleanRow = new List<string>();
            foreach (string cell in row.ToString().Split('^'))
            {
                string temp = cell;
                if (cell.Contains("\""))
                {
                    temp = cell.Replace("\"", "\"\"");
                }
                if (cell.Contains(","))
                {
                    temp = string.Format("\"{0}\"", cell);
                }
                if (cell.Contains(System.Environment.NewLine))
                {
                    temp = string.Format("\"{0}\"", cell);
                }
                cleanRow.Add(temp);
            }
            row = new StringBuilder(string.Join(",", cleanRow));
            row.AppendLine();
            return row;
           // Download(row.ToString());
        }
        
        public AppCommonReportDto GetGrandTotalForAppReport(List<AppCommonReportDto> reportingList)
        {
            AppCommonReportDto TotalStam = new AppCommonReportDto();
            TotalStam.IsExport = true;
            long AdRequests = 0;
            long AdImpress = 0;
            long Clicks = 0;

            long RequestsByType = 0;
            long WonImpressions = 0;
            decimal Revenue = 0;
            decimal eCPMv = 0;
            foreach (var item in reportingList)
            {
                item.IsExport = true;

                AdRequests = AdRequests + item.AdRequests;
                AdImpress = AdImpress + item.AdImpress;
                Clicks = Clicks + item.Clicks;
                Revenue = Revenue + item.Revenue;
                eCPMv = eCPMv + item.eCPM;
                RequestsByType = RequestsByType + item.RequestsByType;
                WonImpressions = WonImpressions + item.WonImpressions;
            }
            TotalStam.AdRequests = AdRequests;
            TotalStam.AdImpress = AdImpress;
            TotalStam.Clicks = Clicks;
            TotalStam.Revenue = Revenue;

            TotalStam.RequestsByType = RequestsByType;
            TotalStam.WonImpressions = WonImpressions;

            return TotalStam;
        }

        public CampaignCommonReportDto GetGrandTotalForCampaignReport(List<CampaignCommonReportDto> reportingList)
        {
            CampaignCommonReportDto TotalStamp = new CampaignCommonReportDto();
            TotalStamp.IsExport = true;
            long Impress = 0;
            long Clicks = 0;
            decimal Spend = 0;
            decimal CTR = 0;
            decimal AvgCPC = 0;
            decimal totaldataprice = 0;
            long WonImpressions = 0;
            long RequestsByType = 0;

            long custom_events = 0;
            long vcomplete = 0;
            long vthirdquartile = 0;
            long vmidpoint = 0;

            long vfirstquartile = 0;

            long vstart = 0;

            long vcreativeviews = 0;

            long pageviews = 0;



            decimal conv_ot_vt_rev = 0;
            decimal conv_ot_ct_rev = 0;
            decimal conv_ot_rev = 0;
            decimal conv_pr_vt_rev = 0;
            decimal conv_pr_ct_rev = 0;
            decimal conv_pr_rev = 0;

long conv_ot_vt = 0;
long conv_ot_ct = 0;
long conv_ot = 0;
long conv_pr_vt = 0;
long conv_pr_ct = 0;
long conv_pr = 0;
            foreach (var item in reportingList)
            {
                item.IsExport = true;

                Impress = item.Impress + Impress;
                Clicks = item.Clicks + Clicks;
               
                CTR = item.CTR + CTR;
                AvgCPC = item.AvgCPC + AvgCPC;
                Spend = item.BillableCost + Spend;
                RequestsByType = item.RequestsByType + RequestsByType;
                WonImpressions = item.WonImpressions + WonImpressions;

                pageviews = pageviews + item.PageViews;


                custom_events = item.CustomEvents + custom_events;
                vcomplete = item.VComplete + vcomplete;
                vthirdquartile = item.VThirdQuartile + vthirdquartile;


                vmidpoint = item.VMidPoint + vmidpoint;
                vfirstquartile =  item.VFirstQuartile + vfirstquartile;
                vstart = item.VStart + vstart;

                vcreativeviews = item.VCreativeViews + vcreativeviews;


                totaldataprice = item.DataFee + totaldataprice;


                conv_ot_vt_rev = item.conv_ot_vt_rev + conv_ot_vt_rev;
                conv_ot_ct_rev = item.conv_ot_ct_rev + conv_ot_ct_rev;
                conv_ot_rev = item.conv_ot_rev + conv_ot_rev;
                conv_pr_vt_rev = item.conv_pr_vt_rev + conv_pr_vt_rev;
                conv_pr_ct_rev = item.conv_pr_ct_rev + conv_pr_ct_rev;
                conv_pr_rev = item.conv_pr_rev + conv_pr_rev;

                conv_ot_vt = item.conv_ot_vt + conv_ot_vt;
                conv_ot_ct = item.conv_ot_ct + conv_ot_ct;
                conv_ot = item.conv_ot + conv_ot;
                conv_pr_vt = item.conv_pr_vt + conv_pr_vt;
                conv_pr_ct = item.conv_pr_ct + conv_pr_ct;
                conv_pr = item.conv_pr + conv_pr;




            }
            TotalStamp.Impress = Impress;
            TotalStamp.Clicks = Clicks;
            TotalStamp.BillableCost = Spend;
            TotalStamp.CTR = CTR;
            TotalStamp.AvgCPC = AvgCPC;
           
            TotalStamp.RequestsByType = RequestsByType;
            TotalStamp.WonImpressions = WonImpressions;

            TotalStamp.PageViews = pageviews;



            TotalStamp.CustomEvents = custom_events;
            TotalStamp.VComplete = vcomplete;
            TotalStamp.VThirdQuartile = vthirdquartile;
            TotalStamp.DataProvider= "---";

            TotalStamp.VMidPoint = vmidpoint;
            TotalStamp.VFirstQuartile = vfirstquartile;


            TotalStamp.VStart = vstart;
            TotalStamp.VCreativeViews = vcreativeviews;
            TotalStamp.DataFee = totaldataprice;



            TotalStamp.conv_ot_vt_rev =  conv_ot_vt_rev;
            TotalStamp.conv_ot_ct_rev = conv_ot_ct_rev;
            TotalStamp.conv_ot_rev =  conv_ot_rev;
            TotalStamp.conv_pr_vt_rev =  conv_pr_vt_rev;
            TotalStamp.conv_pr_ct_rev = conv_pr_ct_rev;
            TotalStamp.conv_pr_rev =  conv_pr_rev;

            TotalStamp.conv_ot_vt = conv_ot_vt;
            TotalStamp.conv_ot_ct = conv_ot_ct;
            TotalStamp.conv_ot =  conv_ot;
            TotalStamp.conv_pr_vt = conv_pr_vt;
            TotalStamp.conv_pr_ct = conv_pr_ct;
            TotalStamp.conv_pr = conv_pr;
            return TotalStamp;
        }
    }
}

