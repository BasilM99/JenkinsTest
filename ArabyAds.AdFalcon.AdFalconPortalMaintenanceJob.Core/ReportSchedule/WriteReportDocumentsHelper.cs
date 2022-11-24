using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text.pdf;

using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;
using System.IO;
using iTextSharp.text;
using ArabyAds.Framework;
using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Reports;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Services.Interfaces.Core;
using ArabyAds.AdFalcon.Domain.Repositories;
using ArabyAds.AdFalcon.Services.Interfaces.Messages;
using ArabyAds.Framework.Utilities;

namespace ArabyAds.AdFalcon.AdFalconPortalMaintenanceJob.ReportSchedule
{

    public class WriteReportDocumentsHelper
    {
        private ArabyAds.Framework.Resources.IResourceService _ResourceService;
        private ArabyAds.AdFalcon.Services.Interfaces.Services.Account.IAccountService _AccountService;

        private static Font _dispalyFont;
        private static Font _fontHeader;
        static readonly object LockObj = new object();
        public string ExecuteGetResources(string key , string set, string culture=null)
        {
            lock (LockObj)
            {
                if(!string.IsNullOrEmpty(culture))
               return _ResourceService.GetResource(new Framework.Resources.ResourceRequest { Key =key,  ResourceSet= set }).Result;
                else
                    return _ResourceService.GetResource(new Framework.Resources.ResourceRequest {  Key= key, ResourceSet= set, CultureCode=  culture }).Result;
            }
        
        }
        public static Font GetFont()
        {
            if (_dispalyFont == null)
            {
                lock (LockObj)
                {
                    if (_dispalyFont == null)
                    {
                        var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Fonts) + "\\arial.ttf";
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
                        var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Fonts) + "\\arial.ttf";
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
                        // using ()

                        {
                            iTextSharp.text.Document document = new iTextSharp.text.Document(PageSize.A4, 10, 10, 40, 40);

                            //using ()
                            {
                                PdfWriter writer = PdfWriter.GetInstance(document, memDocument);
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
                                if (_AccountService.GetAccountRole(ValueMessageWrapper.Create(dto.AccountId)).Value == (int)AccountRole.DSP)
                                {
                                    columnCounter += 2;
                                }

                                PdfPTable table = GetTable(columnCounter);

                                PdfPCell dateRangeHeader = GetCell(ExecuteGetResources("DateRange", "Report"), fontHeader);

                                PdfPCell adRequestsHeader = GetCell(ExecuteGetResources("AdRequests", "AppChart"), fontHeader);
                                PdfPCell adImpressHeader = GetCell(ExecuteGetResources("AdImpress", "AppChart"), fontHeader);
                                PdfPCell adClicksHeader = GetCell(ExecuteGetResources("AdClicks", "AppChart"), fontHeader);
                                PdfPCell fillRateHeader = GetCell(ExecuteGetResources("FillRate", "AppChart"), fontHeader);
                                PdfPCell CTRHeader = GetCell(ExecuteGetResources("CTR", "AppChart"), fontHeader);
                                PdfPCell eCPMHeader = GetCell(ExecuteGetResources("eCPM", "AppChart"), fontHeader);
                                PdfPCell revenueHeader = GetCell(ExecuteGetResources("Revenue", "AppChart"), fontHeader);

                                if (showDateRange)
                                    table.AddCell(dateRangeHeader);

                                if (showNameColumn)
                                {
                                    PdfPCell nameHeader = GetCell(ExecuteGetResources("Name", "AppReport"), fontHeader);
                                    table.AddCell(nameHeader);
                                }

                                if (showSubNameColumn)
                                {
                                    PdfPCell campainNameHeader = GetCell(ExecuteGetResources("AppSite", "AppChart"), fontHeader);
                                    table.AddCell(campainNameHeader);
                                }

                                table.AddCell(adRequestsHeader);
                                table.AddCell(adImpressHeader);
                                table.AddCell(adClicksHeader);
                                table.AddCell(fillRateHeader);
                                table.AddCell(CTRHeader);
                                table.AddCell(eCPMHeader);
                                table.AddCell(revenueHeader);

                                if (_AccountService.GetAccountRole(ValueMessageWrapper.Create(dto.AccountId)).Value == (int)AccountRole.DSP)
                                {
                                    PdfPCell WinRateText = GetCell(ExecuteGetResources("WinRateText", "PMPDeal"), fontHeader);
                                    table.AddCell(WinRateText);
                                    PdfPCell DisplayRateText = GetCell(ExecuteGetResources("DisplayRateText", "PMPDeal"), fontHeader);
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
                                    if (_AccountService.GetAccountRole(ValueMessageWrapper.Create(dto.AccountId)).Value == (int)AccountRole.DSP)
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
                        //completePath = JsonConfigurationManager.AppSettings["ReportFolderCreation"] + DateTime.UtcNow.Ticks.ToString() + ".pdf";

                        completePath = JsonConfigurationManager.AppSettings["ReportFolderCreation"] + "AdFalcon_stats" + "_" + DateTime.UtcNow.Ticks.ToString() + ".pdf";


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
                            strExcelWriter.Append(ExecuteGetResources("DateRange", "Report"));
                            strExcelWriter.Append("\t");
                        }

                        if (showNameColumn)
                        {
                            strExcelWriter.Append(ExecuteGetResources("Name", "AppReport"));
                            strExcelWriter.Append("\t");
                        }

                        if (showSubNameColumn)
                        {
                            strExcelWriter.Append(ExecuteGetResources("AppSite", "AppChart"));
                            strExcelWriter.Append("\t");
                        }

                        strExcelWriter.Append(ExecuteGetResources("AdRequests", "AppChart"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(ExecuteGetResources("AdImpress", "AppChart"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(ExecuteGetResources("AdClicks", "AppChart"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(ExecuteGetResources("FillRate", "AppChart"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(ExecuteGetResources("CTR", "AppChart"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(ExecuteGetResources("eCPM", "AppChart"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(ExecuteGetResources("Revenue", "AppChart"));

                        if (_AccountService.GetAccountRole(ValueMessageWrapper.Create(dto.AccountId)).Value == (int)AccountRole.DSP)
                        {
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(ExecuteGetResources("WinRateText", "PMPDeal"));
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(ExecuteGetResources("DisplayRateText", "PMPDeal"));
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

                            if (_AccountService.GetAccountRole(ValueMessageWrapper.Create(dto.AccountId)).Value == (int)AccountRole.DSP)
                            {
                                strExcelWriter.Append("\t");
                                strExcelWriter.Append(item.WinRate);
                                strExcelWriter.Append("\t");
                                strExcelWriter.Append(item.DisplayRate);
                            }


                            strExcelWriter.Append("\n");
                        }


                        // completePath = JsonConfigurationManager.AppSettings["ReportFolderCreation"] + DateTime.UtcNow.Ticks.ToString() + ".xls";


                        completePath = JsonConfigurationManager.AppSettings["ReportFolderCreation"] + "AdFalcon_stats" + "_" + DateTime.UtcNow.Ticks.ToString() + ".xls";
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
                        var ReportTitle = ExecuteGetResources("ReportTitle", "Report", SchedulerHelper.GetCultureStr());

                        dto.ReportTitle = dto.ReportTitle.Replace(" ", "_").Replace("-", "_").Replace(":", "_").Replace("___", "_").Replace("__", "_");
                        emailBoday = emailBoday + "<b>" + ReportTitle + ExecuteGetResources("SemiColonSubject", "Report", SchedulerHelper.GetCultureStr()) + "</b>" + dto.ReportTitle + "<br/>";
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
                        var DateTimeGenerated = ExecuteGetResources("DateTimeGenerated", "Report", SchedulerHelper.GetCultureStr());
                        var stringDate = String.Format(JsonConfigurationManager.AppSettings["LongDateFormatService"], DateTime.Now);
                        emailBoday = emailBoday + "<b>" + DateTimeGenerated + ExecuteGetResources("SemiColonSubject", "Report", SchedulerHelper.GetCultureStr()) + "</b>" + stringDate + "<br/>";
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
                        var TimeZoneGenerated = ExecuteGetResources("TimeZoneGenerated", "Report", SchedulerHelper.GetCultureStr());
                        var GMT = ExecuteGetResources("UTC", "Global");
                        emailBoday = emailBoday + "<b>" + TimeZoneGenerated + ExecuteGetResources("SemiColonSubject", "Report", SchedulerHelper.GetCultureStr()) + "</b>" + GMT + "<br/>";
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

                        var ReportID = ExecuteGetResources("ReportID", "Report", SchedulerHelper.GetCultureStr());
                        emailBoday = emailBoday + "<b>" + ReportID + ExecuteGetResources("SemiColonSubject", "Report", SchedulerHelper.GetCultureStr()) + "</b>" + dto.ReportId + "<br/>";
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

                        var DateRange = ExecuteGetResources("DateRange", "Report", SchedulerHelper.GetCultureStr());
                        emailBoday = emailBoday + "<b>" + DateRange + ExecuteGetResources("SemiColonSubject", "Report", SchedulerHelper.GetCultureStr()) + "</b>" + dto.ReportDto.FromDateString + ExecuteGetResources("ToSubject", "Report", SchedulerHelper.GetCultureStr()) + dto.ReportDto.ToDateString + "<br/>";
                        strCSVWriter.Append(DateRange);
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(dto.ReportDto.FromDateString + ExecuteGetResources("ToSubject", "Report", SchedulerHelper.GetCultureStr()) + dto.ReportDto.ToDateString);
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



                        strCSVWriter.Append(ExecuteGetResources("ReportFields", "Report", SchedulerHelper.GetCultureStr()));
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
                            strCSVWriter.Append(ExecuteGetResources("DateRange", "Report", SchedulerHelper.GetCultureStr()));
                            strCSVWriter.Append(",");
                        }
                        if (showNameColumn)
                        {
                            strCSVWriter.Append(ExecuteGetResources("Name", "AppReport", SchedulerHelper.GetCultureStr()));
                            strCSVWriter.Append(",");
                        }

                        if (showSubNameColumn)
                        {
                            strCSVWriter.Append(ExecuteGetResources("AppSite", "AppChart", SchedulerHelper.GetCultureStr()));
                            strCSVWriter.Append(",");
                        }


                        strCSVWriter.Append(ExecuteGetResources("AdRequests", "AppChart", SchedulerHelper.GetCultureStr()));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(ExecuteGetResources("AdImpress", "AppChart", SchedulerHelper.GetCultureStr()));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(ExecuteGetResources("AdClicks", "AppChart", SchedulerHelper.GetCultureStr()));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(ExecuteGetResources("FillRate", "AppChart", SchedulerHelper.GetCultureStr()));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(ExecuteGetResources("CTR", "AppChart", SchedulerHelper.GetCultureStr()));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(ExecuteGetResources("eCPM", "AppChart", SchedulerHelper.GetCultureStr()));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(ExecuteGetResources("Revenue", "AppChart", SchedulerHelper.GetCultureStr()));

                        if (_AccountService.GetAccountRole(ValueMessageWrapper.Create(dto.AccountId)).Value == (int)AccountRole.DSP)
                        {
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(ExecuteGetResources("WinRateText", "PMPDeal", SchedulerHelper.GetCultureStr()));
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(ExecuteGetResources("DisplayRateText", "PMPDeal", SchedulerHelper.GetCultureStr()));
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
                            if (_AccountService.GetAccountRole(ValueMessageWrapper.Create(dto.AccountId)).Value == (int)AccountRole.DSP)
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
                            //completePath = JsonConfigurationManager.AppSettings["ReportFolderCreation"] + DateTime.UtcNow.Ticks.ToString() + ".csv";
                            strCSVWriter.Append(ExecuteGetResources("GrandTotal", "Report", SchedulerHelper.GetCultureStr()));
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
                            //strCSVWriter.Append(ExecuteGetResources("AvgCPC", "AdChart"));
                            //strCSVWriter.Append(",");
                            strCSVWriter.Append(Totalsamp.RevenueText);

                            if (_AccountService.GetAccountRole(ValueMessageWrapper.Create(dto.AccountId)).Value == (int)AccountRole.DSP)
                            {
                                strCSVWriter.Append(",");
                                strCSVWriter.Append(Totalsamp.WinRate);
                                strCSVWriter.Append(",");
                                strCSVWriter.Append(Totalsamp.DisplayRate);
                            }

                            strCSVWriter.Append("\n");
                        }

                        completePath = JsonConfigurationManager.AppSettings["ReportFolderCreation"] + dto.FileName + ".csv";
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
                       // using ()
                        {
                            iTextSharp.text.Document document = new iTextSharp.text.Document(PageSize.A4, 10, 10, 40, 40);
                           // using ()
                            {
                                PdfWriter writer = PdfWriter.GetInstance(document, memDocument);
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

                                PdfPCell dateRangeHeader = GetCell(ExecuteGetResources("DateRange", "Report"), fontHeader);

                                PdfPCell impressHeader = GetCell(ExecuteGetResources("Impress", "AdChart"), fontHeader);
                                PdfPCell clicksHeader = GetCell(ExecuteGetResources("Clicks", "AdChart"), fontHeader);
                                PdfPCell ctrHeader = GetCell(ExecuteGetResources("CTR", "AdChart"), fontHeader);
                                PdfPCell avgCPCHeader = GetCell(ExecuteGetResources("AvgCPC", "AdChart"), fontHeader);
                                PdfPCell spendHeader = GetCell(ExecuteGetResources("BillableCost", "Global"), fontHeader);

                                if (showDateRange)
                                { table.AddCell(dateRangeHeader); }

                                if (showNameColumn)
                                {
                                    PdfPCell nameHeader = GetCell(ExecuteGetResources("Name", "CampaignReport"), fontHeader);
                                    table.AddCell(nameHeader);
                                }

                                if (showSubNameColumn)
                                {
                                    PdfPCell campainNameHeader = GetCell(ExecuteGetResources("CampaignName", "AdChart"), fontHeader);
                                    table.AddCell(campainNameHeader);
                                }

                                table.AddCell(impressHeader);
                                if (showCI)
                                {
                                    PdfPCell UniqueImpressions = GetCell(ExecuteGetResources("UniqueImp", "Report"), fontHeader);
                                    table.AddCell(UniqueImpressions);

                                }
                                table.AddCell(clicksHeader);
                                if (showCI)
                                {
                                    PdfPCell UniqueClicks = GetCell(ExecuteGetResources("UniqueClicks", "Report"), fontHeader);
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
                        //  completePath = JsonConfigurationManager.AppSettings["ReportFolderCreation"] + DateTime.UtcNow.Ticks.ToString() + ".pdf";

                        completePath = JsonConfigurationManager.AppSettings["ReportFolderCreation"] + dto.FileName + ".pdf";
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
                            strExcelWriter.Append(ExecuteGetResources("DateRange", "Report"));
                            strExcelWriter.Append("\t");
                        }
                        if (showNameColumn)
                        {
                            strExcelWriter.Append(ExecuteGetResources("Name", "CampaignReport"));
                            strExcelWriter.Append("\t");
                        }

                        if (showSubNameColumn)
                        {
                            strExcelWriter.Append(ExecuteGetResources("CampaignName", "AdChart"));
                            strExcelWriter.Append("\t");
                        }

                        strExcelWriter.Append(ExecuteGetResources("Impress", "AdChart"));
                        strExcelWriter.Append("\t");
                        if (showCI)
                        {
                            strExcelWriter.Append(ExecuteGetResources("UniqueImp", "Report"));
                            strExcelWriter.Append("\t");
                        }
                        strExcelWriter.Append(ExecuteGetResources("Clicks", "AdChart"));
                        strExcelWriter.Append("\t");
                        if (showCI)
                        {
                            strExcelWriter.Append(ExecuteGetResources("UniqueClicks", "Report"));
                            strExcelWriter.Append("\t");
                        }
                        strExcelWriter.Append(ExecuteGetResources("CTR", "AdChart"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(ExecuteGetResources("AvgCPC", "AdChart"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(ExecuteGetResources("BillableCost", "Global"));

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



                        //completePath = JsonConfigurationManager.AppSettings["ReportFolderCreation"] + DateTime.UtcNow.Ticks.ToString() + ".xls";

                        completePath = JsonConfigurationManager.AppSettings["ReportFolderCreation"] + "AdFalcon_stats" + "_" + DateTime.UtcNow.Ticks.ToString() + ".xls";
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
                        var ReportTitle = ExecuteGetResources("ReportTitle", "Report", SchedulerHelper.GetCultureStr());
                        dto.ReportTitle = dto.ReportTitle.Replace(" ", "_").Replace("-", "_").Replace(":", "_").Replace("___", "_").Replace("__", "_"); ;
                        emailBoday = emailBoday + "<b>" + ReportTitle + ExecuteGetResources("SemiColonSubject", "Report", SchedulerHelper.GetCultureStr()) + "</b>" + dto.ReportTitle + "<br/>";
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
                        var DateTimeGenerated = ExecuteGetResources("DateTimeGenerated", "Report", SchedulerHelper.GetCultureStr());
                        var stringDate = String.Format(JsonConfigurationManager.AppSettings["LongDateFormatService"], DateTime.Now);
                        emailBoday = emailBoday + "<b>" + DateTimeGenerated + ExecuteGetResources("SemiColonSubject", "Report", SchedulerHelper.GetCultureStr()) + "</b>" + stringDate + "<br/>";
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
                        var TimeZoneGenerated = ExecuteGetResources("TimeZoneGenerated", "Report", SchedulerHelper.GetCultureStr());
                        var GMT = ExecuteGetResources("UTC", "Global");
                        emailBoday = emailBoday + "<b>" + TimeZoneGenerated + ExecuteGetResources("SemiColonSubject", "Report", SchedulerHelper.GetCultureStr()) + "</b>" + GMT + "<br/>";
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

                        var ReportID = ExecuteGetResources("ReportID", "Report", SchedulerHelper.GetCultureStr());
                        emailBoday = emailBoday + "<b>" + ReportID + ExecuteGetResources("SemiColonSubject", "Report", SchedulerHelper.GetCultureStr()) + "</b>" + dto.ReportId + "<br/>";
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

                        var DateRange = ExecuteGetResources("DateRange", "Report", SchedulerHelper.GetCultureStr());
                        emailBoday = emailBoday + "<b>" + DateRange + ExecuteGetResources("SemiColonSubject", "Report", SchedulerHelper.GetCultureStr()) + "</b>" + dto.ReportDto.FromDateString + ExecuteGetResources("ToSubject", "Report", SchedulerHelper.GetCultureStr()) + dto.ReportDto.ToDateString + "<br/>";
                        strCSVWriter.Append(DateRange);
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(dto.ReportDto.FromDateString + ExecuteGetResources("ToSubject", "Report", SchedulerHelper.GetCultureStr()) + dto.ReportDto.ToDateString);
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




                        strCSVWriter.Append(ExecuteGetResources("ReportFields", "Report", SchedulerHelper.GetCultureStr()));
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
                            strCSVWriter.Append(ExecuteGetResources("DateRange", "Report", SchedulerHelper.GetCultureStr()));
                            strCSVWriter.Append(",");
                        }
                        if (showNameColumn)
                        {
                            strCSVWriter.Append(ExecuteGetResources("Name", "CampaignReport", SchedulerHelper.GetCultureStr()));
                            strCSVWriter.Append(",");
                        }

                        if (showSubNameColumn)
                        {
                            strCSVWriter.Append(ExecuteGetResources("CampaignName", "AdChart", SchedulerHelper.GetCultureStr()));
                            strCSVWriter.Append(",");
                        }

                        strCSVWriter.Append(ExecuteGetResources("Impress", "AdChart", SchedulerHelper.GetCultureStr()));
                        strCSVWriter.Append(",");
                        if (showCI)
                        {
                            strCSVWriter.Append(ExecuteGetResources("UniqueImp", "Report"));
                            strCSVWriter.Append(",");
                        }
                        strCSVWriter.Append(ExecuteGetResources("Clicks", "AdChart", SchedulerHelper.GetCultureStr()));
                        strCSVWriter.Append(",");
                        if (showCI)
                        {
                            strCSVWriter.Append(ExecuteGetResources("UniqueClicks", "Report"));
                            strCSVWriter.Append(",");
                        }
                        strCSVWriter.Append(ExecuteGetResources("CTR", "AdChart", SchedulerHelper.GetCultureStr()));
                        strCSVWriter.Append(",");
                        //strCSVWriter.Append(ExecuteGetResources("AvgCPC", "AdChart"));
                        //strCSVWriter.Append(",");
                        strCSVWriter.Append(ExecuteGetResources("BillableCost", "Global", SchedulerHelper.GetCultureStr()));

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
                            strCSVWriter.Append(ExecuteGetResources("GrandTotal", "Report", SchedulerHelper.GetCultureStr()));
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
                            //strCSVWriter.Append(ExecuteGetResources("AvgCPC", "AdChart"));
                            //strCSVWriter.Append(",");
                            strCSVWriter.Append(Totalsamp.BillableCostText);

                            strCSVWriter.Append("\n");
                        }
                        //completePath = JsonConfigurationManager.AppSettings["ReportFolderCreation"] + DateTime.UtcNow.Ticks.ToString() + ".csv";

                        completePath = JsonConfigurationManager.AppSettings["ReportFolderCreation"] + dto.FileName + ".csv";
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
                SubNameid = _reportService.GetColumnId(new GetColumnIdRequest { AppFieldName = "SubName", Publisher = dto.ReportSectionType == ReportSectionType.Publisher }).Value;
            }
            if (!showSubNameColumn && dto.MatixColumns != null && dto.MatixColumns.Count > 0)
            {
                SubNameid = _reportService.GetColumnId(new GetColumnIdRequest { AppFieldName = "SubName", Publisher = dto.ReportSectionType == ReportSectionType.Publisher }).Value;
            }

            if (!showNameColumn && dto.MatixColumns != null && dto.MatixColumns.Count > 0)
            {
                Nameid = _reportService.GetColumnId(new GetColumnIdRequest { AppFieldName = "Name", Publisher = dto.ReportSectionType == ReportSectionType.Publisher }).Value;
            }

            if (!showDateRange && dto.MatixColumns != null && dto.MatixColumns.Count > 0)
            {
                DateRangeid = _reportService.GetColumnId(new GetColumnIdRequest { AppFieldName = "DateRangeProp", Publisher = dto.ReportSectionType == ReportSectionType.Publisher }).Value;
            }


            if (!showImpClicksColumn && dto.MatixColumns != null && dto.MatixColumns.Count > 0)
            {
                UniqImpid = _reportService.GetColumnId(new GetColumnIdRequest { AppFieldName = "UniqueImp", Publisher = dto.ReportSectionType == ReportSectionType.Publisher }).Value;
            }

            if (!showuniqClicksColumn && dto.MatixColumns != null && dto.MatixColumns.Count > 0)
            {
                UniqClickid = _reportService.GetColumnId(new GetColumnIdRequest { AppFieldName = "UniqueClicks", Publisher = dto.ReportSectionType == ReportSectionType.Publisher }).Value;
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
                               // using ()
                                {
                                    iTextSharp.text.Document document = new iTextSharp.text.Document();
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


                                    //using ()
                                    {
                                        PdfWriter writer = PdfWriter.GetInstance(document, memDocument);
                                        writer.CloseStream = false;
                                        document.Open();

                                        PdfPTable table = GetTable(MatrixColumnsIds.Count);

                                        foreach (int id in MatrixColumnsIds)
                                        {
                                            metriceColumnDto Column = _reportService.GetColumn(ValueMessageWrapper.Create(id));
                                            if (Column != null)
                                            {
                                                PdfPCell Header = GetCell(ExecuteGetResources(Column.HeaderResourceKey, Column.HeaderResourceSet), fontHeader);

                                                table.AddCell(Header);
                                            }
                                        }
                                        foreach (object item in reportingList)
                                        {
                                            foreach (int id in MatrixColumnsIds)
                                            {
                                                metriceColumnDto Column = _reportService.GetColumn(ValueMessageWrapper.Create(id));
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
                                completePath = JsonConfigurationManager.AppSettings["ReportFolderCreation"] + dto.FileName + ".pdf";
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
                                    metriceColumnDto Column = _reportService.GetColumn(ValueMessageWrapper.Create(MatrixColumnsIds[i]));
                                    if (Column != null)
                                    {
                                        strExcelWriter.Append(ExecuteGetResources(Column.HeaderResourceKey, Column.HeaderResourceSet));

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
                                        metriceColumnDto Column = _reportService.GetColumn(ValueMessageWrapper.Create(MatrixColumnsIds[i]));
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

                                completePath = JsonConfigurationManager.AppSettings["ReportFolderCreation"] + "AdFalcon_stats" + "_" + DateTime.UtcNow.Ticks.ToString() + ".xls";
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
                                var ReportTitle = ExecuteGetResources("ReportTitle", "Report", SchedulerHelper.GetCultureStr());
                                dto.ReportTitle = dto.ReportTitle.Replace(" ", "_").Replace("-", "_").Replace(":", "_").Replace("___", "_").Replace("__", "_"); ;
                                emailBoday = emailBoday + "<b>" + ReportTitle + ExecuteGetResources("SemiColonSubject", "Report", SchedulerHelper.GetCultureStr()) + "</b>" + dto.ReportTitle + "<br/>";
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
                                var DateTimeGenerated = ExecuteGetResources("DateTimeGenerated", "Report", SchedulerHelper.GetCultureStr());
                                var stringDate = String.Format(JsonConfigurationManager.AppSettings["LongDateFormatService"], DateTime.Now);
                                emailBoday = emailBoday + "<b>" + DateTimeGenerated + ExecuteGetResources("SemiColonSubject", "Report", SchedulerHelper.GetCultureStr()) + "</b>" + stringDate + "<br/>";
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
                                var TimeZoneGenerated = ExecuteGetResources("TimeZoneGenerated", "Report", SchedulerHelper.GetCultureStr());
                                var GMT = ExecuteGetResources("UTC", "Global");
                                emailBoday = emailBoday + "<b>" + TimeZoneGenerated + ExecuteGetResources("SemiColonSubject", "Report", SchedulerHelper.GetCultureStr()) + "</b>" + GMT + "<br/>";
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

                                var ReportID = ExecuteGetResources("ReportID", "Report", SchedulerHelper.GetCultureStr());
                                emailBoday = emailBoday + "<b>" + ReportID + ExecuteGetResources("SemiColonSubject", "Report", SchedulerHelper.GetCultureStr()) + "</b>" + dto.ReportId + "<br/>";
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

                                var DateRange = ExecuteGetResources("DateRange", "Report", SchedulerHelper.GetCultureStr());
                                emailBoday = emailBoday + "<b>" + DateRange + ExecuteGetResources("SemiColonSubject", "Report", SchedulerHelper.GetCultureStr()) + "</b>" + dto.ReportDto.FromDateString + ExecuteGetResources("ToSubject", "Report", SchedulerHelper.GetCultureStr()) + dto.ReportDto.ToDateString + "<br/>";
                                strCSVWriter.Append(DateRange);
                                strCSVWriter.Append(",");
                                strCSVWriter.Append(dto.ReportDto.FromDateString + ExecuteGetResources("ToSubject", "Report", SchedulerHelper.GetCultureStr()) + dto.ReportDto.ToDateString);
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




                                strCSVWriter.Append(ExecuteGetResources("ReportFields", "Report", SchedulerHelper.GetCultureStr()));
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
                                    metriceColumnDto Column = _reportService.GetColumn(ValueMessageWrapper.Create(MatrixColumnsIds[i]));
                                    if (Column != null)
                                    {
                                        strCSVWriter.Append(ExecuteGetResources(Column.HeaderResourceKey, Column.HeaderResourceSet));

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
                                        metriceColumnDto Column = _reportService.GetColumn(ValueMessageWrapper.Create(MatrixColumnsIds[i]));
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
                                    strCSVWriter.Append(ExecuteGetResources("GrandTotal", "Report", SchedulerHelper.GetCultureStr()));
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
                                        metriceColumnDto Column = _reportService.GetColumn(ValueMessageWrapper.Create(MatrixColumnsIds[i]));
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

                                completePath = JsonConfigurationManager.AppSettings["ReportFolderCreation"] + dto.FileName + ".csv";
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
        public string BuildCSVReportFile(List<string> reportingList, string exportType, ReportSchedulerDto dto, ref string emailBoday)
        {
          

            _ResourceService = SchedulerHelper.ResourceService;
            string completePath = string.Empty;
            using (MemoryStream memDocument = new MemoryStream())
            {
                

          
             
                        StringBuilder strCSVWriter = new StringBuilder();
                        strCSVWriter.Append("\n");
                        strCSVWriter.Append("\n");
                        var ReportTitle = ExecuteGetResources("ReportTitle", "Report", SchedulerHelper.GetCultureStr());
                        dto.ReportTitle = dto.ReportTitle.Replace(" ", "_").Replace("-", "_").Replace(":", "_").Replace("___", "_").Replace("__", "_"); ;
                        emailBoday = emailBoday + "<b>" + ReportTitle + ExecuteGetResources("SemiColonSubject", "Report", SchedulerHelper.GetCultureStr()) + "</b>" + dto.ReportTitle + "<br/>";
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
                        var DateTimeGenerated = ExecuteGetResources("DateTimeGenerated", "Report", SchedulerHelper.GetCultureStr());
                        var stringDate = String.Format(JsonConfigurationManager.AppSettings["LongDateFormatService"], DateTime.Now);
                        emailBoday = emailBoday + "<b>" + DateTimeGenerated + ExecuteGetResources("SemiColonSubject", "Report", SchedulerHelper.GetCultureStr()) + "</b>" + stringDate + "<br/>";
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
                        var TimeZoneGenerated = ExecuteGetResources("TimeZoneGenerated", "Report", SchedulerHelper.GetCultureStr());
                        var GMT = ExecuteGetResources("UTC", "Global");
                        emailBoday = emailBoday + "<b>" + TimeZoneGenerated + ExecuteGetResources("SemiColonSubject", "Report", SchedulerHelper.GetCultureStr()) + "</b>" + GMT + "<br/>";
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

                        var ReportID = ExecuteGetResources("ReportID", "Report", SchedulerHelper.GetCultureStr());
                        emailBoday = emailBoday + "<b>" + ReportID + ExecuteGetResources("SemiColonSubject", "Report", SchedulerHelper.GetCultureStr()) + "</b>" + dto.ReportId + "<br/>";
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

                        var DateRange = ExecuteGetResources("DateRange", "Report", SchedulerHelper.GetCultureStr());
                        emailBoday = emailBoday + "<b>" + DateRange + ExecuteGetResources("SemiColonSubject", "Report", SchedulerHelper.GetCultureStr()) + "</b>" + dto.ReportDto.FromDateString + ExecuteGetResources("ToSubject", "Report", SchedulerHelper.GetCultureStr()) + dto.ReportDto.ToDateString + "<br/>";
                        strCSVWriter.Append(DateRange);
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(dto.ReportDto.FromDateString + ExecuteGetResources("ToSubject", "Report", SchedulerHelper.GetCultureStr()) + dto.ReportDto.ToDateString);
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




                        strCSVWriter.Append(ExecuteGetResources("ReportFields", "Report", SchedulerHelper.GetCultureStr()));
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
                        //completePath = JsonConfigurationManager.AppSettings["ReportFolderCreation"] + DateTime.UtcNow.Ticks.ToString() + ".csv";

                        completePath = JsonConfigurationManager.AppSettings["ReportFolderCreation"] + dto.FileName + ".csv";
                        using (FileStream file = new FileStream(completePath, FileMode.Create, System.IO.FileAccess.Write))
                        {

                        }
                        File.WriteAllText(completePath, strCSVWriter.ToString(), Encoding.UTF8);
                  
                }

           return completePath;
           
        }
      
        public StringBuilder PrepareCSVPacket(string row)
        {
            List<string> cleanRow = new List<string>();
            foreach (string cell in row.Split('^'))
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
            var stbRow = new StringBuilder(string.Join(",", cleanRow));
            stbRow.AppendLine();
            return stbRow;
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

