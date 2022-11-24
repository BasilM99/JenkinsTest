using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;
using System.IO;
using iTextSharp.text.pdf;
using System.Web;
using iTextSharp.text;
using System.Globalization;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;

using ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Account;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.Dashboard;
using Microsoft.Net.Http.Headers;
using ArabyAds.Framework;

namespace ArabyAds.AdFalcon.Web.Controllers.Utilities
{
    public class WriteDashboardDocumentsHelper
    {
        public FileContentResult BuildAppGeoLocationExportFile(List<AppSiteGeoLocationDto> appsiteGeoLocation, string type, List<KeyValueDto> keyValueDtos)
        {
            using (MemoryStream memDocument = new MemoryStream())
            {
                FileContentResult streamResult;
                switch (type.ToLower())
                {
                    case "pdf":
                       // using ()
                        {
                            Document document = new Document(PageSize.A4, 30, 30, 40, 40);
                           // using ()
                            {
                                PdfWriter writer = PdfWriter.GetInstance(document, memDocument);
                                writer.CloseStream = false;
                                document.Open();

                                addHeader(document, keyValueDtos);
                                iTextSharp.text.Font fontHeader = WriteReportDocumentsHelper.GetHeaderFont();

                                PdfPTable table = WriteReportDocumentsHelper.GetTable(9); ;

                                PdfPCell countryNameHeader = WriteReportDocumentsHelper.GetCell(ResourcesUtilities.GetResource("CountryName", "AppChart"), fontHeader);
                                PdfPCell appSiteNameHeader = WriteReportDocumentsHelper.GetCell(ResourcesUtilities.GetResource("AppName", "AppChart"), fontHeader);
                                PdfPCell adRequestsHeader = WriteReportDocumentsHelper.GetCell(ResourcesUtilities.GetResource("AdRequests", "AppChart"), fontHeader);
                                PdfPCell adImpressHeader = WriteReportDocumentsHelper.GetCell(ResourcesUtilities.GetResource("AdImpress", "AppChart"), fontHeader);
                                PdfPCell adClicksHeader = WriteReportDocumentsHelper.GetCell(ResourcesUtilities.GetResource("AdClicks", "AppChart"), fontHeader);
                                PdfPCell fillRateHeader = WriteReportDocumentsHelper.GetCell(ResourcesUtilities.GetResource("FillRate", "AppChart"), fontHeader);
                                PdfPCell CTRHeader = WriteReportDocumentsHelper.GetCell(ResourcesUtilities.GetResource("CTR", "AppChart"), fontHeader);
                                PdfPCell eCPMHeader = WriteReportDocumentsHelper.GetCell(ResourcesUtilities.GetResource("eCPM", "AppChart"), fontHeader);
                                PdfPCell revenueHeader = WriteReportDocumentsHelper.GetCell(ResourcesUtilities.GetResource("Revenue", "AppChart"), fontHeader);

                                table.AddCell(countryNameHeader);
                                table.AddCell(appSiteNameHeader);
                                table.AddCell(adRequestsHeader);
                                table.AddCell(adImpressHeader);
                                table.AddCell(adClicksHeader);
                                table.AddCell(fillRateHeader);
                                table.AddCell(CTRHeader);
                                table.AddCell(eCPMHeader);
                                table.AddCell(revenueHeader);

                                foreach (var item in appsiteGeoLocation)
                                {
                                    iTextSharp.text.Font font = WriteReportDocumentsHelper.GetFont();

                                    PdfPCell countryName = WriteReportDocumentsHelper.GetCell(item.CountryName, font);
                                    PdfPCell appSiteName = WriteReportDocumentsHelper.GetCell(item.AppSiteName, font);
                                    PdfPCell adRequests = WriteReportDocumentsHelper.GetCell(item.AdRequests.ToString(), font);
                                    PdfPCell adImpress = WriteReportDocumentsHelper.GetCell(item.AdImpress.ToString(), font);
                                    PdfPCell adClicks = WriteReportDocumentsHelper.GetCell(item.AdClicks.ToString(), font);
                                    PdfPCell fillRate = WriteReportDocumentsHelper.GetCell(item.FillRateText, font);
                                    PdfPCell CTR = WriteReportDocumentsHelper.GetCell(item.CtrText, font);
                                    PdfPCell eCPM = WriteReportDocumentsHelper.GetCell(item.eCPMText, font);
                                    PdfPCell revenue = WriteReportDocumentsHelper.GetCell(item.RevenueText, font);

                                    table.AddCell(countryName);
                                    table.AddCell(appSiteName);
                                    table.AddCell(adRequests);
                                    table.AddCell(adImpress);
                                    table.AddCell(adClicks);
                                    table.AddCell(fillRate);
                                    table.AddCell(CTR);
                                    table.AddCell(eCPM);
                                    table.AddCell(revenue);
                                }

                                document.Add(table);
                                document.Close();

                                writer.Flush();
                                memDocument.Flush();
                            }
                        }

                        //prepare output stream
                        //HttpContextHelper.Current.Response.ContentType = "application/pdf";
                        //HttpContextHelper.Current.Response.AddHeader("content-disposition", "attachment;filename=Statistics.pdf");
                        //HttpContextHelper.Current.Response.Buffer = true;
                        //HttpContextHelper.Current.Response.Clear();
                        //HttpContextHelper.Current.Response.OutputStream.Write(memDocument.GetBuffer(), 0, memDocument.GetBuffer().Length);
                        //HttpContextHelper.Current.Response.OutputStream.Flush();
                        //HttpContextHelper.Current.Response.End();

                        //streamResult = new FileContentResult(HttpContextHelper.Current.Response.OutputStream, "application/pdf");
                        //var memstrea = new MemoryStream((new UTF8Encoding(true).GetBytes(strExcelWriter.ToString())));

                        streamResult = new FileContentResult(memDocument.ToArray(), new MediaTypeHeaderValue("application/vnd.pdf"))
                        {
                            FileDownloadName = "Statistics.pdf"
                        };
                        break;
                    case "excel":
                        StringBuilder strExcelWriter = new StringBuilder();
                        addHeader(strExcelWriter, keyValueDtos);
                        strExcelWriter.Append(ResourcesUtilities.GetResource("CountryName", "AppChart"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(ResourcesUtilities.GetResource("AppName", "AppChart"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(ResourcesUtilities.GetResource("AdRequests", "AppChart"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(ResourcesUtilities.GetResource("AdImpress", "AppChart"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(ResourcesUtilities.GetResource("AdClicks", "AppChart"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(ResourcesUtilities.GetResource("FillRate", "AppChart"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(ResourcesUtilities.GetResource("CTR", "AppChart"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(ResourcesUtilities.GetResource("eCPM", "AppChart"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(ResourcesUtilities.GetResource("Revenue", "AppChart"));

                        strExcelWriter.Append("\n");

                        foreach (var item in appsiteGeoLocation)
                        {
                            item.IsExport = true;

                            strExcelWriter.Append(item.CountryName);
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.AppSiteName);
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.AdRequests);
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.AdImpress);
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.AdClicks);
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.FillRate);
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.CTR);
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.eCPM);
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.Revenue);

                            strExcelWriter.Append("\n");
                        }

                        //HttpContextHelper.Current.Response.Clear();
                        //HttpContextHelper.Current.Response.ClearHeaders();
                        //HttpContextHelper.Current.Response.ClearContent();
                        //HttpContextHelper.Current.Response.ContentType = "application/vnd.ms-excel";
                        //HttpContextHelper.Current.Response.AddHeader("content-disposition", "attachment;filename=Statistics.xls");
                        //HttpContextHelper.Current.Response.HeaderEncoding = Encoding.UTF8;
                        //HttpContextHelper.Current.Response.AddHeader("Pragma", "public");
                        //HttpContextHelper.Current.Response.ContentEncoding = Encoding.Unicode;
                        //// Start the feed with BOM
                        //HttpContextHelper.Current.Response.BinaryWrite(Encoding.Unicode.GetPreamble());
                        //HttpContextHelper.Current.Response.Write(strExcelWriter.ToString());
                        //HttpContextHelper.Current.Response.End();

                        //streamResult = new FileContentResult(HttpContextHelper.Current.Response.OutputStream, "application/vnd.ms-excel");



                        var memstrea = new MemoryStream((new UTF8Encoding(true).GetBytes(strExcelWriter.ToString())));

                        streamResult = new FileContentResult(memstrea.ToArray(), new MediaTypeHeaderValue("application/vnd.ms-excel"))
                        {
                            FileDownloadName = "Statistics.xls"
                        };
                        break;
                    case "csv":
                        StringBuilder strCSVWriter = new StringBuilder();
                        addHeader(strCSVWriter, keyValueDtos);
                        strCSVWriter.Append(ResourcesUtilities.GetResource("CountryName", "AppChart"));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(ResourcesUtilities.GetResource("AppName", "AppChart"));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(ResourcesUtilities.GetResource("AdRequests", "AppChart"));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(ResourcesUtilities.GetResource("AdImpress", "AppChart"));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(ResourcesUtilities.GetResource("AdClicks", "AppChart"));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(ResourcesUtilities.GetResource("FillRate", "AppChart"));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(ResourcesUtilities.GetResource("CTR", "AppChart"));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(ResourcesUtilities.GetResource("eCPM", "AppChart"));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(ResourcesUtilities.GetResource("Revenue", "AppChart"));

                        strCSVWriter.Append("\n");

                        foreach (var item in appsiteGeoLocation)
                        {
                            item.IsExport = true;

                            strCSVWriter.Append(item.CountryName);
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(item.AppSiteName);
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(item.AdRequests);
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(item.AdImpress);
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(item.AdClicks);
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(item.FillRate);
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(item.CTR);
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(item.eCPM);
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(item.Revenue);

                            strCSVWriter.Append("\n");
                        }

                        //HttpContextHelper.Current.Response.Clear();
                        //HttpContextHelper.Current.Response.ClearHeaders();
                        //HttpContextHelper.Current.Response.ClearContent();
                        //HttpContextHelper.Current.Response.AddHeader("content-disposition", "attachment;filename=Statistics.csv");
                        //HttpContextHelper.Current.Response.ContentType = "text/csv";
                        //HttpContextHelper.Current.Response.HeaderEncoding = Encoding.UTF8;
                        //HttpContextHelper.Current.Response.AddHeader("Pragma", "public");
                        //HttpContextHelper.Current.Response.ContentEncoding = Encoding.Unicode;
                        //// Start the feed with BOM
                        //HttpContextHelper.Current.Response.BinaryWrite(Encoding.Unicode.GetPreamble());
                        //HttpContextHelper.Current.Response.Write(strCSVWriter.ToString());
                        //HttpContextHelper.Current.Response.End();

                        //streamResult = new FileContentResult(HttpContextHelper.Current.Response.OutputStream, "text/csv");


                       var memstreaexcecv = new MemoryStream((new UTF8Encoding(true).GetBytes(strCSVWriter.ToString())));

                        streamResult = new FileContentResult(memstreaexcecv.ToArray(), new MediaTypeHeaderValue("text/csv"))
                        {
                            FileDownloadName = "Statistics.csv"
                        };
                        break;
                    default:
                        streamResult = new FileContentResult(memDocument.ToArray(), "text/plain");
                        break;
                }

                return streamResult;
            }
        }
        public FileContentResult BuildDealPerformanceExportFile(List<DealPerformanceDto> DealPerformanceList, string type, List<KeyValueDto> keyValueDtos, string NameTitle = "")
        {
            using (MemoryStream memDocument = new MemoryStream())
            {
                FileContentResult streamResult;
                if (string.IsNullOrEmpty(NameTitle))
                {
                    NameTitle = ResourcesUtilities.GetResource("Name", "Global");
                }

                switch (type.ToLower())
                {
                    case "pdf":
                       // using ()
                        {
                            Document document = new Document(PageSize.A4, 30, 30, 40, 40);
                            //using ()
                            {
                                PdfWriter writer = PdfWriter.GetInstance(document, memDocument);
                                writer.CloseStream = false;
                                document.Open();
                                addHeader(document, keyValueDtos);
                                iTextSharp.text.Font fontHeader = WriteReportDocumentsHelper.GetHeaderFont();

                                PdfPTable table = WriteReportDocumentsHelper.GetTable(9);
                                PdfPCell DateHeader = WriteReportDocumentsHelper.GetCell(ResourcesUtilities.GetResource("Date", "AccountHistory"), fontHeader);
                                PdfPCell NameHeader = WriteReportDocumentsHelper.GetCell(NameTitle, fontHeader);
                                PdfPCell AvailableImpressionsHeader = WriteReportDocumentsHelper.GetCell(ResourcesUtilities.GetResource("AvailableImpressions", "PMPDeal"), fontHeader);
                                PdfPCell DisplayedImpressionsHeader = WriteReportDocumentsHelper.GetCell(ResourcesUtilities.GetResource("DisplayedImpressions", "PMPDeal"), fontHeader);
                                PdfPCell AdResponseHeader = WriteReportDocumentsHelper.GetCell(ResourcesUtilities.GetResource("AdResponse", "PMPDeal"), fontHeader);
                                PdfPCell WonImpressionsHeader = WriteReportDocumentsHelper.GetCell(ResourcesUtilities.GetResource("WonImpressions", "PMPDeal"), fontHeader);
                                PdfPCell DisplayRateTextHeader = WriteReportDocumentsHelper.GetCell(ResourcesUtilities.GetResource("DisplayRateText", "PMPDeal"), fontHeader);
                                PdfPCell ResponseRateTextHeader = WriteReportDocumentsHelper.GetCell(ResourcesUtilities.GetResource("ResponseRateText", "PMPDeal"), fontHeader);
                                PdfPCell WinRateTextHeader = WriteReportDocumentsHelper.GetCell(ResourcesUtilities.GetResource("WinRateText", "PMPDeal"), fontHeader);
                                table.AddCell(DateHeader);

                                table.AddCell(NameHeader);
                                table.AddCell(AvailableImpressionsHeader);
                                table.AddCell(AdResponseHeader);
                                table.AddCell(ResponseRateTextHeader);
                                table.AddCell(WonImpressionsHeader);
                                table.AddCell(WinRateTextHeader);
                                table.AddCell(DisplayedImpressionsHeader);
                                table.AddCell(DisplayRateTextHeader);

                                foreach (var item in DealPerformanceList)
                                {
                                    iTextSharp.text.Font font = WriteReportDocumentsHelper.GetFont();
                                    PdfPCell Date = WriteReportDocumentsHelper.GetCell(item.DateRange, font);
                                    PdfPCell Name = WriteReportDocumentsHelper.GetCell(item.FinalSecondSubName, font);
                                    PdfPCell AvailableImpressions = WriteReportDocumentsHelper.GetCell(item.FinalAvailableImpressions.ToString(), font);
                                    PdfPCell DisplayedImpressions = WriteReportDocumentsHelper.GetCell(item.DisplayedImpressions.ToString(), font);
                                    PdfPCell AdResponses = WriteReportDocumentsHelper.GetCell(item.AdResponse.ToString(), font);
                                    PdfPCell WonImpressions = WriteReportDocumentsHelper.GetCell(item.getWins().ToString(), font);
                                    PdfPCell DisplayRateText = WriteReportDocumentsHelper.GetCell(item.DisplayRateText, font);
                                    PdfPCell ResponseRateText = WriteReportDocumentsHelper.GetCell(item.ResponseRateText, font);
                                    PdfPCell WinRateText = WriteReportDocumentsHelper.GetCell(item.WinRateText, font);
                                    table.AddCell(Date);

                                    table.AddCell(Name);
                                    table.AddCell(AvailableImpressions);
                                    table.AddCell(AdResponses);
                                    table.AddCell(ResponseRateText);
                                    table.AddCell(WonImpressions);
                                    table.AddCell(WinRateText);
                                    table.AddCell(DisplayedImpressions);
                                    table.AddCell(DisplayRateText);
                                }

                                document.Add(table);
                                document.Close();

                                writer.Flush();
                                memDocument.Flush();
                            }
                        }
                        //prepare output stream
                        //HttpContextHelper.Current.Response.ContentType = "application/pdf";
                        //HttpContextHelper.Current.Response.AddHeader("content-disposition", "attachment;filename=Performance.pdf");
                        //HttpContextHelper.Current.Response.Buffer = true;
                        //HttpContextHelper.Current.Response.Clear();
                        //HttpContextHelper.Current.Response.OutputStream.Write(memDocument.GetBuffer(), 0, memDocument.GetBuffer().Length);
                        //HttpContextHelper.Current.Response.OutputStream.Flush();
                        //HttpContextHelper.Current.Response.End();

                        //streamResult = new FileContentResult(HttpContextHelper.Current.Response.OutputStream, "application/pdf");

                        //var memstrea = new MemoryStream((new UTF8Encoding(true).GetBytes(strExcelWriter.ToString())));

                        streamResult = new FileContentResult(memDocument.ToArray(), new MediaTypeHeaderValue("application/pdf"))
                        {
                            FileDownloadName = "Performance.pdf"
                        };

                        break;
                    case "excel":
                        StringBuilder strExcelWriter = new StringBuilder();
                        addHeader(strExcelWriter, keyValueDtos);

                        strExcelWriter.Append(ResourcesUtilities.GetResource("Date", "AccountHistory"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(NameTitle);
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(ResourcesUtilities.GetResource("AvailableImpressions", "PMPDeal"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(ResourcesUtilities.GetResource("AdResponse", "PMPDeal"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(ResourcesUtilities.GetResource("ResponseRateText", "PMPDeal"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(ResourcesUtilities.GetResource("WonImpressions", "PMPDeal"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(ResourcesUtilities.GetResource("WinRateText", "PMPDeal"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(ResourcesUtilities.GetResource("DisplayedImpressions", "PMPDeal"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(ResourcesUtilities.GetResource("DisplayRateText", "PMPDeal"));
                        strExcelWriter.Append("\n");

                        foreach (var item in DealPerformanceList)
                        {
                            item.IsExport = true;

                            strExcelWriter.Append(item.DateRange);
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.FinalSecondSubName);
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.FinalAvailableImpressions);
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.AdResponse);
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.ResponseRateText);
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.WonImpressions);
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.WinRateText);
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.DisplayedImpressions);
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.DisplayRateText);
                            strExcelWriter.Append("\n");

                        }

                        //HttpContextHelper.Current.Response.Clear();
                        //HttpContextHelper.Current.Response.ClearHeaders();
                        //HttpContextHelper.Current.Response.ClearContent();
                        //HttpContextHelper.Current.Response.ContentType = "application/vnd.ms-excel";
                        //HttpContextHelper.Current.Response.AddHeader("content-disposition", "attachment;filename=Performance.xls");
                        //HttpContextHelper.Current.Response.HeaderEncoding = Encoding.UTF8;
                        //HttpContextHelper.Current.Response.AddHeader("Pragma", "public");
                        //HttpContextHelper.Current.Response.ContentEncoding = Encoding.Unicode;
                        //// Start the feed with BOM
                        //HttpContextHelper.Current.Response.BinaryWrite(Encoding.Unicode.GetPreamble());
                        //HttpContextHelper.Current.Response.Write(strExcelWriter.ToString());
                        //HttpContextHelper.Current.Response.End();

                        //streamResult = new FileContentResult(HttpContextHelper.Current.Response.OutputStream, "application/vnd.ms-excel");


                        var memstrea = new MemoryStream((new UTF8Encoding(true).GetBytes(strExcelWriter.ToString())));

                        streamResult = new FileContentResult(memstrea.ToArray(), new MediaTypeHeaderValue("application/vnd.ms-excel"))
                        {
                            FileDownloadName = "Performance.xls"
                        };

                        break;
                    case "csv":
                        StringBuilder strCSVWriter = new StringBuilder();
                        addHeader(strCSVWriter, keyValueDtos);

                        strCSVWriter.Append(ResourcesUtilities.GetResource("Date", "AccountHistory"));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(NameTitle);
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(ResourcesUtilities.GetResource("AvailableImpressions", "PMPDeal"));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(ResourcesUtilities.GetResource("AdResponse", "PMPDeal"));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(ResourcesUtilities.GetResource("ResponseRateText", "PMPDeal"));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(ResourcesUtilities.GetResource("WonImpressions", "PMPDeal"));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(ResourcesUtilities.GetResource("WinRateText", "PMPDeal"));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(ResourcesUtilities.GetResource("DisplayedImpressions", "PMPDeal"));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(ResourcesUtilities.GetResource("DisplayRateText", "PMPDeal"));
                        strCSVWriter.Append("\n");

                        foreach (var item in DealPerformanceList)
                        {
                            item.IsExport = true;

                            strCSVWriter.Append(item.DateRange);
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(item.FinalSecondSubName);
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(item.FinalAvailableImpressions);
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(item.AdResponse);
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(item.ResponseRateText);
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(item.WonImpressions);
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(item.WinRateText);
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(item.DisplayedImpressions);
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(item.DisplayRateText);
                            strCSVWriter.Append("\n");

                        }

                        //HttpContextHelper.Current.Response.Clear();
                        //HttpContextHelper.Current.Response.ClearHeaders();
                        //HttpContextHelper.Current.Response.ClearContent();
                        //HttpContextHelper.Current.Response.AddHeader("content-disposition", "attachment;filename=Performance.csv");
                        //HttpContextHelper.Current.Response.ContentType = "text/csv";
                        //HttpContextHelper.Current.Response.HeaderEncoding = Encoding.UTF8;
                        //HttpContextHelper.Current.Response.AddHeader("Pragma", "public");
                        //HttpContextHelper.Current.Response.ContentEncoding = Encoding.Unicode;
                        //// Start the feed with BOM
                        //HttpContextHelper.Current.Response.BinaryWrite(Encoding.Unicode.GetPreamble());
                        //HttpContextHelper.Current.Response.Write(strCSVWriter.ToString());
                        //HttpContextHelper.Current.Response.End();

                        //streamResult = new FileContentResult(HttpContextHelper.Current.Response.OutputStream, "text/csv");



                             var memstreaccsv = new MemoryStream((new UTF8Encoding(true).GetBytes(strCSVWriter.ToString())));

                        streamResult = new FileContentResult(memstreaccsv.ToArray(), new MediaTypeHeaderValue("text/csv"))
                        {
                            FileDownloadName = "Performance.csv"
                        };

                        break;
                    default:
                        streamResult = new FileContentResult(memDocument.ToArray(), "text/plain");
                        break;
                }

                return streamResult;
            }
        }
        public FileContentResult BuildAppPerformanceExportFile(List<AppSitePerformanceDto> appsitePerformanceList,  string type , List<KeyValueDto> keyValueDtos)
        {
            using (MemoryStream memDocument = new MemoryStream())
            {
                FileContentResult streamResult;
                switch (type.ToLower())
                {
                    case "pdf":
                       // using ()
                        {
                            Document document = new Document(PageSize.A4, 30, 30, 40, 40);
                          //  using ()
                            {
                                PdfWriter writer = PdfWriter.GetInstance(document, memDocument);
                                writer.CloseStream = false;
                                document.Open();
                                addHeader(document, keyValueDtos);
                                iTextSharp.text.Font fontHeader = WriteReportDocumentsHelper.GetHeaderFont();

                                PdfPTable table = WriteReportDocumentsHelper.GetTable(8);

                                PdfPCell appSiteNameHeader = WriteReportDocumentsHelper.GetCell(ResourcesUtilities.GetResource("AppName", "AppChart"), fontHeader);
                                PdfPCell adRequestsHeader = WriteReportDocumentsHelper.GetCell(ResourcesUtilities.GetResource("AdRequests", "AppChart"), fontHeader);
                                PdfPCell adImpressHeader = WriteReportDocumentsHelper.GetCell(ResourcesUtilities.GetResource("AdImpress", "AppChart"), fontHeader);
                                PdfPCell adClicksHeader = WriteReportDocumentsHelper.GetCell(ResourcesUtilities.GetResource("AdClicks", "AppChart"), fontHeader);
                                PdfPCell fillRateHeader = WriteReportDocumentsHelper.GetCell(ResourcesUtilities.GetResource("FillRate", "AppChart"), fontHeader);
                                PdfPCell CTRHeader = WriteReportDocumentsHelper.GetCell(ResourcesUtilities.GetResource("CTR", "AppChart"), fontHeader);
                                PdfPCell eCPMHeader = WriteReportDocumentsHelper.GetCell(ResourcesUtilities.GetResource("eCPM", "AppChart"), fontHeader);
                                PdfPCell revenueHeader = WriteReportDocumentsHelper.GetCell(ResourcesUtilities.GetResource("Revenue", "AppChart"), fontHeader);

                                table.AddCell(appSiteNameHeader);
                                table.AddCell(adRequestsHeader);
                                table.AddCell(adImpressHeader);
                                table.AddCell(adClicksHeader);
                                table.AddCell(fillRateHeader);
                                table.AddCell(CTRHeader);
                                table.AddCell(eCPMHeader);
                                table.AddCell(revenueHeader);

                                foreach (var item in appsitePerformanceList)
                                {
                                    iTextSharp.text.Font font = WriteReportDocumentsHelper.GetFont();

                                    PdfPCell appSiteName = WriteReportDocumentsHelper.GetCell(item.AppSiteName, font);
                                    PdfPCell adRequests = WriteReportDocumentsHelper.GetCell(item.AdRequests.ToString(), font);
                                    PdfPCell adImpress = WriteReportDocumentsHelper.GetCell(item.AdImpress.ToString(), font);
                                    PdfPCell adClicks = WriteReportDocumentsHelper.GetCell(item.AdClicks.ToString(), font);
                                    PdfPCell fillRate = WriteReportDocumentsHelper.GetCell(item.FillRateText, font);
                                    PdfPCell CTR = WriteReportDocumentsHelper.GetCell(item.CtrText, font);
                                    PdfPCell eCPM = WriteReportDocumentsHelper.GetCell(item.eCPMText, font);
                                    PdfPCell revenue = WriteReportDocumentsHelper.GetCell(item.RevenueText, font);

                                    table.AddCell(appSiteName);
                                    table.AddCell(adRequests);
                                    table.AddCell(adImpress);
                                    table.AddCell(adClicks);
                                    table.AddCell(fillRate);
                                    table.AddCell(CTR);
                                    table.AddCell(eCPM);
                                    table.AddCell(revenue);
                                }

                                document.Add(table);
                                document.Close();

                                writer.Flush();
                                writer.Close();
                                memDocument.Flush();
                            }
                        }
                        //prepare output stream
                        //HttpContextHelper.Current.Response.ContentType = "application/pdf";
                        //HttpContextHelper.Current.Response.AddHeader("content-disposition", "attachment;filename=Performance.pdf");
                        //HttpContextHelper.Current.Response.Buffer = true;
                        //HttpContextHelper.Current.Response.Clear();
                        //HttpContextHelper.Current.Response.OutputStream.Write(memDocument.GetBuffer(), 0, memDocument.GetBuffer().Length);
                        //HttpContextHelper.Current.Response.OutputStream.Flush();
                        //HttpContextHelper.Current.Response.End();

                        //streamResult = new FileContentResult(HttpContextHelper.Current.Response.OutputStream, "application/pdf");


                        //var memstrea = new MemoryStream((new UTF8Encoding(true).GetBytes(strExcelWriter.ToString())));

                        streamResult = new FileContentResult(memDocument.ToArray(), new MediaTypeHeaderValue("application/pdf"))
                        {
                            FileDownloadName = "Performance.pdf"
                        };

                        break;
                    case "excel":
                        StringBuilder strExcelWriter = new StringBuilder();
                        addHeader(strExcelWriter, keyValueDtos);

                        strExcelWriter.Append(ResourcesUtilities.GetResource("AppName", "AppChart"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(ResourcesUtilities.GetResource("AdRequests", "AppChart"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(ResourcesUtilities.GetResource("AdImpress", "AppChart"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(ResourcesUtilities.GetResource("AdClicks", "AppChart"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(ResourcesUtilities.GetResource("FillRate", "AppChart"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(ResourcesUtilities.GetResource("CTR", "AppChart"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(ResourcesUtilities.GetResource("eCPM", "AppChart"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(ResourcesUtilities.GetResource("Revenue", "AppChart"));

                        strExcelWriter.Append("\n");

                        foreach (var item in appsitePerformanceList)
                        {
                            item.IsExport = true;

                            strExcelWriter.Append(item.AppSiteName);
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.AdRequests);
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.AdImpress);
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.AdClicks);
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.FillRate);
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.CTR);
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.eCPM);
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.Revenue);

                            strExcelWriter.Append("\n");
                        }

                        //HttpContextHelper.Current.Response.Clear();
                        //HttpContextHelper.Current.Response.ClearHeaders();
                        //HttpContextHelper.Current.Response.ClearContent();
                        //HttpContextHelper.Current.Response.ContentType = "application/vnd.ms-excel";
                        //HttpContextHelper.Current.Response.AddHeader("content-disposition", "attachment;filename=Performance.xls");
                        //HttpContextHelper.Current.Response.HeaderEncoding = Encoding.UTF8;
                        //HttpContextHelper.Current.Response.AddHeader("Pragma", "public");
                        //HttpContextHelper.Current.Response.ContentEncoding = Encoding.Unicode;
                        //// Start the feed with BOM
                        //HttpContextHelper.Current.Response.BinaryWrite(Encoding.Unicode.GetPreamble());
                        //HttpContextHelper.Current.Response.Write(strExcelWriter.ToString());
                        //HttpContextHelper.Current.Response.End();

                        //streamResult = new FileContentResult(HttpContextHelper.Current.Response.OutputStream, "application/vnd.ms-excel");


                        var memstreaexce = new MemoryStream((new UTF8Encoding(true).GetBytes(strExcelWriter.ToString())));

                        streamResult = new FileContentResult(memstreaexce.ToArray(), new MediaTypeHeaderValue("application/vnd.ms-excel"))
                        {
                            FileDownloadName = "Performance.xls"
                        };
                        break;
                    case "csv":
                        StringBuilder strCSVWriter = new StringBuilder();
                        addHeader(strCSVWriter, keyValueDtos);

                        strCSVWriter.Append(ResourcesUtilities.GetResource("AppName", "AppChart"));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(ResourcesUtilities.GetResource("AdRequests", "AppChart"));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(ResourcesUtilities.GetResource("AdImpress", "AppChart"));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(ResourcesUtilities.GetResource("AdClicks", "AppChart"));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(ResourcesUtilities.GetResource("FillRate", "AppChart"));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(ResourcesUtilities.GetResource("CTR", "AppChart"));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(ResourcesUtilities.GetResource("eCPM", "AppChart"));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(ResourcesUtilities.GetResource("Revenue", "AppChart"));

                        strCSVWriter.Append("\n");

                        foreach (var item in appsitePerformanceList)
                        {
                            item.IsExport = true;

                            strCSVWriter.Append(item.AppSiteName);
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(item.AdRequests);
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(item.AdImpress);
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(item.AdClicks);
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(item.FillRate);
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(item.CTR);
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(item.eCPM);
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(item.Revenue);

                            strCSVWriter.Append("\n");
                        }

                        //HttpContextHelper.Current.Response.Clear();
                        //HttpContextHelper.Current.Response.ClearHeaders();
                        //HttpContextHelper.Current.Response.ClearContent();
                        //HttpContextHelper.Current.Response.AddHeader("content-disposition", "attachment;filename=Performance.csv");
                        //HttpContextHelper.Current.Response.ContentType = "text/csv";
                        //HttpContextHelper.Current.Response.HeaderEncoding = Encoding.UTF8;
                        //HttpContextHelper.Current.Response.AddHeader("Pragma", "public");
                        //HttpContextHelper.Current.Response.ContentEncoding = Encoding.Unicode;
                        //// Start the feed with BOM
                        //HttpContextHelper.Current.Response.BinaryWrite(Encoding.Unicode.GetPreamble());
                        //HttpContextHelper.Current.Response.Write(strCSVWriter.ToString());
                        //HttpContextHelper.Current.Response.End();

                        //streamResult = new FileContentResult(HttpContextHelper.Current.Response.OutputStream, "text/csv");
                        var memstreat = new MemoryStream((new UTF8Encoding(true).GetBytes(strCSVWriter.ToString())));

                        streamResult = new FileContentResult(memstreat.ToArray(), new MediaTypeHeaderValue("text/csv"))
                        {
                            FileDownloadName = "Performance.csv"
                        };

                        break;
                    default:
                        streamResult = new FileContentResult(memDocument.ToArray(), "text/plain");
                        break;
                }

                return streamResult;
            }
        }

        private static void addHeader(StringBuilder strWriter,List<KeyValueDto> labels)
        {
  
            for(int i = 0; i < labels.Count; i++)
            {
                strWriter.Append(labels[i].Key.ToString());
                strWriter.Append("\t");
                strWriter.Append(labels[i].Value.ToString());
                strWriter.Append("\n");
            }
        }
        private static void addHeader(Document document,  List<KeyValueDto> labels)
        {
         
            Paragraph para = new Paragraph("\n");
      
            PdfPTable table = WriteReportDocumentsHelper.GetTable(7);
           // table.HorizontalAlignment = 0;
           // table.WidthPercentage=60;
   
            iTextSharp.text.Font fontHeader = WriteReportDocumentsHelper.GetHeaderFont();
      
            for (int i = 0; i < labels.Count; i++)
            {
                iTextSharp.text.Font font = WriteReportDocumentsHelper.GetFont();
                PdfPCell KeyCell = WriteReportDocumentsHelper.GetCell(labels[i].Key.ToString(), fontHeader);
                PdfPCell valueCell = WriteReportDocumentsHelper.GetCell(labels[i].Value.ToString(), fontHeader);
      
        
                KeyCell.Border = 0;
                //KeyCell.HorizontalAlignment = 0;
               // valueCell.HorizontalAlignment = 0;
                valueCell.Border = 0;
                valueCell.Colspan = 6;
                table.AddCell(KeyCell);
                table.AddCell(valueCell);
         
           
         

           }
            document.Add(table);
            document.Add(para);
        }


        public FileContentResult BuildAdGeoLocationExportFile(List<AdGeoLocationDto> adGeoLocation, string type ,List<KeyValueDto> keyValueDtos)
        {
            using (MemoryStream memDocument = new MemoryStream())
            {
                FileContentResult streamResult;
                switch (type.ToLower())
                {
                    case "pdf":
                       // using ()
                        {
                            Document document = new Document(PageSize.A4, 30, 30, 40, 40);
                          //  using ()
                            {
                                PdfWriter writer = PdfWriter.GetInstance(document, memDocument);
                                writer.CloseStream = false;
                                document.Open();


                            
                                    addHeader(document, keyValueDtos);
                               

                                    iTextSharp.text.Font fontHeader = WriteReportDocumentsHelper.GetHeaderFont();

                                PdfPTable table = WriteReportDocumentsHelper.GetTable(7);

                                PdfPCell countryNameHeader = WriteReportDocumentsHelper.GetCell(ResourcesUtilities.GetResource("CountryName", "AppChart"), fontHeader);
                                PdfPCell campainHeader = WriteReportDocumentsHelper.GetCell(ResourcesUtilities.GetResource("CampaignName", "AdChart"), fontHeader);
                                PdfPCell impressHeader = WriteReportDocumentsHelper.GetCell(ResourcesUtilities.GetResource("Impress", "AdChart"), fontHeader);
                                PdfPCell clicksHeader = WriteReportDocumentsHelper.GetCell(ResourcesUtilities.GetResource("Clicks", "AdChart"), fontHeader);
                                PdfPCell ctrHeader = WriteReportDocumentsHelper.GetCell(ResourcesUtilities.GetResource("CTR", "AdChart"), fontHeader);
                                PdfPCell avgCPCHeader = WriteReportDocumentsHelper.GetCell(ResourcesUtilities.GetResource("AvgCPC", "AdChart"), fontHeader);
                                PdfPCell spendHeader = WriteReportDocumentsHelper.GetCell(ResourcesUtilities.GetResource("BillableCost", "Global"), fontHeader);


                                table.AddCell(countryNameHeader);
                                table.AddCell(campainHeader);
                                table.AddCell(impressHeader);
                                table.AddCell(clicksHeader);
                                table.AddCell(ctrHeader);
                                table.AddCell(avgCPCHeader);
                                table.AddCell(spendHeader);


                                foreach (var item in adGeoLocation)
                                {
                                    iTextSharp.text.Font font = WriteReportDocumentsHelper.GetFont();

                                    PdfPCell countryName = WriteReportDocumentsHelper.GetCell(item.CountryName, font);
                                    PdfPCell campaignName = WriteReportDocumentsHelper.GetCell(item.CampaignName, font);
                                    PdfPCell impress = WriteReportDocumentsHelper.GetCell(item.Impress.ToString(), font);
                                    PdfPCell clicks = WriteReportDocumentsHelper.GetCell(item.Clicks.ToString(), font);
                                    PdfPCell ctr = WriteReportDocumentsHelper.GetCell(item.CtrText, font);
                                    PdfPCell avgCPC = WriteReportDocumentsHelper.GetCell(item.AvgCPCText, font);
                                    PdfPCell spend = WriteReportDocumentsHelper.GetCell(item.BillableCostText, font);


                                    table.AddCell(countryName);
                                    table.AddCell(campaignName);
                                    table.AddCell(impress);
                                    table.AddCell(clicks);
                                    table.AddCell(ctr);
                                    table.AddCell(avgCPC);
                                    table.AddCell(spend);

                                }

                                document.Add(table);
                                document.Close();

                                writer.Flush();
                                writer.Close();
                                memDocument.Flush();
                            }
                        }
                        //prepare output stream
                        //HttpContextHelper.Current.Response.ContentType = "application/pdf";
                        //HttpContextHelper.Current.Response.AddHeader("content-disposition", "attachment;filename=Statistics.pdf");
                        //HttpContextHelper.Current.Response.Buffer = true;
                        //HttpContextHelper.Current.Response.Clear();
                        //HttpContextHelper.Current.Response.OutputStream.Write(memDocument.GetBuffer(), 0, memDocument.GetBuffer().Length);
                        //HttpContextHelper.Current.Response.OutputStream.Flush();
                        //HttpContextHelper.Current.Response.End();

                        streamResult = 
                        new FileContentResult(memDocument.ToArray(), new MediaTypeHeaderValue("application/pdf"))
  
                            {
                                FileDownloadName = "Statistics.pdf"
                        };
  
                        break;
                    case "excel":
                        StringBuilder strExcelWriter = new StringBuilder();
                        //strExcelWriter.Append("Header");
                        //strExcelWriter.Append("\n");
                   
                       addHeader(strExcelWriter, keyValueDtos);

                       // addHeader(strExcelWriter, "header");
                        strExcelWriter.Append(ResourcesUtilities.GetResource("CountryName", "AppChart"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(ResourcesUtilities.GetResource("CampaignName", "AdChart"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(ResourcesUtilities.GetResource("Impress", "AdChart"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(ResourcesUtilities.GetResource("Clicks", "AdChart"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(ResourcesUtilities.GetResource("CTR", "AdChart"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(ResourcesUtilities.GetResource("AvgCPC", "AdChart"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(ResourcesUtilities.GetResource("BillableCost", "Global"));

                        strExcelWriter.Append("\n");

                        foreach (var item in adGeoLocation)
                        {
                            item.IsExport = true;

                            strExcelWriter.Append(item.CountryName);
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.CampaignName);
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.Impress);
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.Clicks);
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.CtrText);
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.AvgCPCText);
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.BillableCostText);

                            strExcelWriter.Append("\n");
                        }

                        //HttpContextHelper.Current.Response.Clear();
                        //HttpContextHelper.Current.Response.ClearHeaders();
                        //HttpContextHelper.Current.Response.ClearContent();
                        //HttpContextHelper.Current.Response.ContentType = "application/vnd.ms-excel";
                        //HttpContextHelper.Current.Response.AddHeader("content-disposition", "attachment;filename=Statistics.xls");
                        //HttpContextHelper.Current.Response.HeaderEncoding = Encoding.UTF8;
                        //HttpContextHelper.Current.Response.AddHeader("Pragma", "public");
                        //HttpContextHelper.Current.Response.ContentEncoding = Encoding.Unicode;
                        //// Start the feed with BOM
                        //HttpContextHelper.Current.Response.BinaryWrite(Encoding.Unicode.GetPreamble());
                        //HttpContextHelper.Current.Response.Write(strExcelWriter.ToString());
                        //HttpContextHelper.Current.Response.End();

                        //streamResult = new FileContentResult(HttpContextHelper.Current.Response.OutputStream, "application/vnd.ms-excel");


                        var streamexcel = new MemoryStream(Encoding.Unicode.GetBytes(strExcelWriter.ToString()));
                        streamResult =
      new FileContentResult(streamexcel.ToArray(), new MediaTypeHeaderValue("application/vnd.ms-excel"))

      {
          FileDownloadName = "Statistics.xls"
      };
                        break;
                    case "csv":
                        StringBuilder strCSVWriter = new StringBuilder();
                        //strCSVWriter.Append("Header");
                        //strCSVWriter.Append("\n");
                        // addHeader(strCSVWriter, "header");
                    
                        addHeader(strCSVWriter, keyValueDtos);
                        strCSVWriter.Append(ResourcesUtilities.GetResource("CountryName", "AppChart"));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(ResourcesUtilities.GetResource("CampaignName", "AdChart"));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(ResourcesUtilities.GetResource("Impress", "AdChart"));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(ResourcesUtilities.GetResource("Clicks", "AdChart"));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(ResourcesUtilities.GetResource("CTR", "AdChart"));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(ResourcesUtilities.GetResource("AvgCPC", "AdChart"));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(ResourcesUtilities.GetResource("BillableCost", "Global"));

                        strCSVWriter.Append("\n");

                        foreach (var item in adGeoLocation)
                        {
                            item.IsExport = true;

                            strCSVWriter.Append(item.CountryName);
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(item.CampaignName);
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(item.Impress);
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(item.Clicks);
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(item.CtrText);
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(item.AvgCPCText);
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(item.BillableCostText);

                            strCSVWriter.Append("\n");
                        }

                        //HttpContextHelper.Current.Response.Clear();
                        //HttpContextHelper.Current.Response.ClearHeaders();
                        //HttpContextHelper.Current.Response.ClearContent();
                        //HttpContextHelper.Current.Response.AddHeader("content-disposition", "attachment;filename=Statistics.csv");
                        //HttpContextHelper.Current.Response.ContentType = "text/csv";
                        //HttpContextHelper.Current.Response.HeaderEncoding = Encoding.UTF8;
                        //HttpContextHelper.Current.Response.AddHeader("Pragma", "public");
                        //HttpContextHelper.Current.Response.ContentEncoding = Encoding.Unicode;
                        //// Start the feed with BOM
                        //HttpContextHelper.Current.Response.BinaryWrite(Encoding.Unicode.GetPreamble());
                        //HttpContextHelper.Current.Response.Write(strCSVWriter.ToString());
                        //HttpContextHelper.Current.Response.End();

                        //streamResult = new FileContentResult(HttpContextHelper.Current.Response.OutputStream, "text/csv");

                        var streamtex = new MemoryStream((new UTF8Encoding(true)).GetBytes(strCSVWriter.ToString()));
                        streamResult =
                     new FileContentResult(streamtex.ToArray(), new MediaTypeHeaderValue("text/csv"))

                     {
                         FileDownloadName = "Statistics.csv"
                     };
                        break;
                    default:
                        streamResult = new FileContentResult(memDocument.ToArray(), "text/plain");
                        break;
                }

                return streamResult;
            }
        }

        public FileContentResult BuildAdPerformanceExportFile(List<AdPerformanceDto> adGeoLocation, string type  , List<KeyValueDto> keyValueDtos)
        {
            using (MemoryStream memDocument = new MemoryStream())
            {
                FileContentResult streamResult;
                switch (type.ToLower())
                {
                    case "pdf":
                       // using ()
                        {
                            Document document = new Document(PageSize.A4, 30, 30, 40, 40);
                           //using ()
                            {
                                PdfWriter writer = PdfWriter.GetInstance(document, memDocument);
                                writer.CloseStream = false;
                                document.Open();
                                addHeader(document, keyValueDtos);
                                iTextSharp.text.Font fontHeader = WriteReportDocumentsHelper.GetHeaderFont();

                                PdfPTable table = WriteReportDocumentsHelper.GetTable(6);

                                PdfPCell campainHeader = WriteReportDocumentsHelper.GetCell(ResourcesUtilities.GetResource("CampaignName", "AdChart"), fontHeader);
                                PdfPCell impressHeader = WriteReportDocumentsHelper.GetCell(ResourcesUtilities.GetResource("Impress", "AdChart"), fontHeader);
                                PdfPCell clicksHeader = WriteReportDocumentsHelper.GetCell(ResourcesUtilities.GetResource("Clicks", "AdChart"), fontHeader);
                                PdfPCell ctrHeader = WriteReportDocumentsHelper.GetCell(ResourcesUtilities.GetResource("CTR", "AdChart"), fontHeader);
                                PdfPCell avgCPCHeader = WriteReportDocumentsHelper.GetCell(ResourcesUtilities.GetResource("AvgCPC", "AdChart"), fontHeader);
                                PdfPCell spendHeader = WriteReportDocumentsHelper.GetCell(ResourcesUtilities.GetResource("BillableCost", "Global"), fontHeader);

                                table.AddCell(campainHeader);
                                table.AddCell(impressHeader);
                                table.AddCell(clicksHeader);
                                table.AddCell(ctrHeader);
                                table.AddCell(avgCPCHeader);
                                table.AddCell(spendHeader);


                                foreach (var item in adGeoLocation)
                                {
                                    iTextSharp.text.Font font = WriteReportDocumentsHelper.GetFont();

                                    PdfPCell campaignName = WriteReportDocumentsHelper.GetCell(item.CampaignName, font);
                                    PdfPCell impress = WriteReportDocumentsHelper.GetCell(item.Impress.ToString(), font);
                                    PdfPCell clicks = WriteReportDocumentsHelper.GetCell(item.Clicks.ToString(), font);
                                    PdfPCell ctr = WriteReportDocumentsHelper.GetCell(item.CtrText, font);
                                    PdfPCell avgCPC = WriteReportDocumentsHelper.GetCell(item.AvgCPCText, font);
                                    PdfPCell spend = WriteReportDocumentsHelper.GetCell(item.BillableCostText, font);


                                    table.AddCell(campaignName);
                                    table.AddCell(impress);
                                    table.AddCell(clicks);
                                    table.AddCell(ctr);
                                    table.AddCell(avgCPC);
                                    table.AddCell(spend);

                                }

                                document.Add(table);
                                document.Close();

                                writer.Flush();
                                writer.Close();
                                memDocument.Flush();

                            }
                        }
                        //prepare output stream
                        //HttpContextHelper.Current.Response.ContentType = "application/pdf";
                        //HttpContextHelper.Current.Response.AddHeader("content-disposition", "attachment;filename=Performance.pdf");
                        //HttpContextHelper.Current.Response.Buffer = true;
                        //HttpContextHelper.Current.Response.Clear();
                        //HttpContextHelper.Current.Response.OutputStream.Write(memDocument.GetBuffer(), 0, memDocument.GetBuffer().Length);
                        //HttpContextHelper.Current.Response.OutputStream.Flush();
                        //HttpContextHelper.Current.Response.End();


                        //var streamtex = new MemoryStream(Encoding.Unicode.GetBytes(strCSVWriter.ToString()));
                        streamResult =
                     new FileContentResult(memDocument.ToArray(), new MediaTypeHeaderValue("application/pdf"))

                     {
                         FileDownloadName = "Performance.pdf"
                     };
                       // streamResult = new FileContentResult(HttpContextHelper.Current.Response.OutputStream, "application/pdf");
                        break;
                    case "excel":
                        StringBuilder strExcelWriter = new StringBuilder();
                       addHeader(strExcelWriter, keyValueDtos);

                        strExcelWriter.Append(ResourcesUtilities.GetResource("CampaignName", "AdChart"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(ResourcesUtilities.GetResource("Impress", "AdChart"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(ResourcesUtilities.GetResource("Clicks", "AdChart"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(ResourcesUtilities.GetResource("CTR", "AdChart"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(ResourcesUtilities.GetResource("AvgCPC", "AdChart"));
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(ResourcesUtilities.GetResource("BillableCost", "Global"));

                        strExcelWriter.Append("\n");

                        foreach (var item in adGeoLocation)
                        {
                            item.IsExport = true;
                            strExcelWriter.Append(item.CampaignName);
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.Impress);
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.Clicks);
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.CtrText);
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.AvgCPCText);
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.BillableCostText);

                            strExcelWriter.Append("\n");
                        }

                        //HttpContextHelper.Current.Response.Clear();
                        //HttpContextHelper.Current.Response.ClearHeaders();
                        //HttpContextHelper.Current.Response.ClearContent();
                        //HttpContextHelper.Current.Response.ContentType = "application/vnd.ms-excel";
                        //HttpContextHelper.Current.Response.AddHeader("content-disposition", "attachment;filename=Performance.xls");
                        //HttpContextHelper.Current.Response.HeaderEncoding = Encoding.UTF8;
                        //HttpContextHelper.Current.Response.AddHeader("Pragma", "public");
                        //HttpContextHelper.Current.Response.ContentEncoding = Encoding.Unicode;
                        //// Start the feed with BOM
                        //HttpContextHelper.Current.Response.BinaryWrite(Encoding.Unicode.GetPreamble());
                        //HttpContextHelper.Current.Response.Write(strExcelWriter.ToString());
                        //HttpContextHelper.Current.Response.End();

                       // streamResult = new FileContentResult(HttpContextHelper.Current.Response.OutputStream, "application/vnd.ms-excel");


                       var streamtex = new MemoryStream((new UTF8Encoding(true)).GetBytes(strExcelWriter.ToString()));
                        streamResult =
                     new FileContentResult(streamtex.ToArray(), new MediaTypeHeaderValue("application/vnd.ms-excel"))

                     {
                         FileDownloadName = "Performance.xls"
                     };
                        break;
                    case "csv":
                        StringBuilder strCSVWriter = new StringBuilder();
                        addHeader(strCSVWriter, keyValueDtos);

                        strCSVWriter.Append(ResourcesUtilities.GetResource("CampaignName", "AdChart"));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(ResourcesUtilities.GetResource("Impress", "AdChart"));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(ResourcesUtilities.GetResource("Clicks", "AdChart"));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(ResourcesUtilities.GetResource("CTR", "AdChart"));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(ResourcesUtilities.GetResource("AvgCPC", "AdChart"));
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(ResourcesUtilities.GetResource("BillableCost", "Global"));

                        strCSVWriter.Append("\n");

                        foreach (var item in adGeoLocation)
                        {
                            item.IsExport = true;

                            strCSVWriter.Append(item.CampaignName);
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(item.Impress);
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(item.Clicks);
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(item.CtrText);
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(item.AvgCPCText);
                            strCSVWriter.Append(",");
                            strCSVWriter.Append(item.BillableCostText);

                            strCSVWriter.Append("\n");
                        }

                        //HttpContextHelper.Current.Response.Clear();
                        //HttpContextHelper.Current.Response.ClearHeaders();
                        //HttpContextHelper.Current.Response.ClearContent();
                        //HttpContextHelper.Current.Response.AddHeader("content-disposition", "attachment;filename=Performance.csv");
                        //HttpContextHelper.Current.Response.ContentType = "text/csv";
                        //HttpContextHelper.Current.Response.HeaderEncoding = Encoding.UTF8;
                        //HttpContextHelper.Current.Response.AddHeader("Pragma", "public");
                        //HttpContextHelper.Current.Response.ContentEncoding = Encoding.Unicode;
                        //// Start the feed with BOM
                        //HttpContextHelper.Current.Response.BinaryWrite(Encoding.Unicode.GetPreamble());
                        //HttpContextHelper.Current.Response.Write(strCSVWriter.ToString());
                        //HttpContextHelper.Current.Response.End();

                        //streamResult = new FileContentResult(HttpContextHelper.Current.Response.OutputStream, "text/csv");



                        var streamtexm = new MemoryStream((new UTF8Encoding(true)).GetBytes(strCSVWriter.ToString()));
                        streamResult =
                     new FileContentResult(streamtexm.ToArray(), new MediaTypeHeaderValue("text/csv"))

                     {
                         FileDownloadName = "Performance.csv"
                     };


                        break;
                    default:
                        streamResult = new FileContentResult(memDocument.ToArray(), "text/plain");
                        break;
                }

                return streamResult;
            }
        }


        public FileContentResult BuildImpressionLogPerformanceExportFile(List<ImpressionLogPerformanceDto> ImpressionLogPerformanceList, string type, bool showCampaign, bool showAdvertiser, List<KeyValueDto> keyValueDtos, string NameTitle = "")
        {
            using (MemoryStream memDocument = new MemoryStream())
            {
                FileContentResult streamResult;
                if (string.IsNullOrEmpty(NameTitle))
                {
                    NameTitle = ResourcesUtilities.GetResource("Name", "Global");
                }
                string DateNameHeaderString = ResourcesUtilities.GetResource("Date", "AccountHistory");

                string AdvertiserNameHeaderString = ResourcesUtilities.GetResource("Advertiser");
                string CampaignNameHeaderString = ResourcesUtilities.GetResource("Campaign");
                string ImpressionsHeaderString = ResourcesUtilities.GetResource("Impress", "AdChart");
                string DiscountHeaderString = ResourcesUtilities.GetResource("Discount", "CampaignSettings");
                string RevenueHeaderString = ResourcesUtilities.GetResource("Revenue", "AppChart");
                string grossrevenueString = ResourcesUtilities.GetResource("grossrevenue", "Global");
                string AVRString = ResourcesUtilities.GetResource("AVR", "Global");
                string UsedSegmentHeaderString = ResourcesUtilities.GetResource("UsedSegment", "Audience");
                string BilledSegmentHeaderString = ResourcesUtilities.GetResource("BilledSegment", "Audience");

                switch (type.ToLower())
                {
                    case "pdf":
                       // using ()
                        {
                            Document document = new Document(PageSize.A4, 30, 30, 40, 40);

                            //using ()
                            {
                                PdfWriter writer = PdfWriter.GetInstance(document, memDocument);
                                writer.CloseStream = false;
                                document.Open();

                             addHeader(document, keyValueDtos);
                                iTextSharp.text.Font fontHeader = WriteReportDocumentsHelper.GetHeaderFont();
                                int tds = 8;
                                if (showCampaign)
                                {
                                    tds++;
                                }
                                if (showAdvertiser)
                                {
                                    tds++;
                                }
                                PdfPTable table = WriteReportDocumentsHelper.GetTable(tds);

                                PdfPCell DateNameHeader = WriteReportDocumentsHelper.GetCell(DateNameHeaderString, fontHeader);

                                PdfPCell AdvertiserNameHeader = WriteReportDocumentsHelper.GetCell(AdvertiserNameHeaderString, fontHeader);
                                PdfPCell CampaignNameHeader = WriteReportDocumentsHelper.GetCell(CampaignNameHeaderString, fontHeader);
                                PdfPCell ImpressionsHeader = WriteReportDocumentsHelper.GetCell(ImpressionsHeaderString, fontHeader);
                                PdfPCell RevenueHeader = WriteReportDocumentsHelper.GetCell(RevenueHeaderString, fontHeader);
                                PdfPCell grossrevenueStringHeader = WriteReportDocumentsHelper.GetCell(grossrevenueString, fontHeader);
                                PdfPCell AVRStringHeader = WriteReportDocumentsHelper.GetCell(AVRString, fontHeader);
                                PdfPCell DiscountHeader = WriteReportDocumentsHelper.GetCell(DiscountHeaderString, fontHeader);
                                PdfPCell UsedSegmentHeader = WriteReportDocumentsHelper.GetCell(UsedSegmentHeaderString, fontHeader);
                                PdfPCell BilledSegmentHeader = WriteReportDocumentsHelper.GetCell(BilledSegmentHeaderString, fontHeader);


                                table.AddCell(DateNameHeader);
                                table.AddCell(BilledSegmentHeader);

                                table.AddCell(UsedSegmentHeader);
                                if (showAdvertiser)
                                {
                                    table.AddCell(AdvertiserNameHeader);

                                }
                                if (showCampaign)
                                {
                                    table.AddCell(CampaignNameHeader);

                                }
                               
                                table.AddCell(ImpressionsHeader);
                                table.AddCell(grossrevenueStringHeader);
                                table.AddCell(RevenueHeader);
                                table.AddCell(AVRStringHeader);
                                table.AddCell(DiscountHeader);
                        
                            
                                
                                foreach (var item in ImpressionLogPerformanceList)
                                {
                                    iTextSharp.text.Font font = WriteReportDocumentsHelper.GetFont();

                                    PdfPCell dateName = WriteReportDocumentsHelper.GetCell(DateTime.ParseExact(item.date.ToString(),
                                    "yyyyMMdd",
                                    CultureInfo.InvariantCulture,
                                    DateTimeStyles.None).ToShortDateString(), font);

                                    PdfPCell AdvertiserName = WriteReportDocumentsHelper.GetCell(item.AdvertiserName, font);
                                    PdfPCell CampaignName = WriteReportDocumentsHelper.GetCell(item.CampaignName, font);
                                    PdfPCell Impressions = WriteReportDocumentsHelper.GetCell(item.Impressions.ToString(), font);
                                    PdfPCell Discount = WriteReportDocumentsHelper.GetCell(item.DiscountText.ToString(), font);
                                    PdfPCell Gross = WriteReportDocumentsHelper.GetCell(item.grossrevenueText.ToString(), font);
                                    PdfPCell avr = WriteReportDocumentsHelper.GetCell(item.avrcostText.ToString(), font);
                                    PdfPCell Revenue = WriteReportDocumentsHelper.GetCell(item.RevenueText.ToString(), font);
                                    PdfPCell UsedSegment = WriteReportDocumentsHelper.GetCell(item.UsedSegments.ToString(), font);
                                    PdfPCell BilledSegment = WriteReportDocumentsHelper.GetCell(item.BilledSegment.ToString(), font);
                                    table.AddCell(dateName);
                                    table.AddCell(BilledSegment);

                                    table.AddCell(UsedSegment);
                                    if (showAdvertiser)
                                    {
                                        table.AddCell(AdvertiserName);

                                    }
                                    if (showCampaign)
                                    {
                                        table.AddCell(CampaignName);

                                    }
                                  
                                    table.AddCell(Impressions);
                                    table.AddCell(Gross);
                                    table.AddCell(Revenue);
                                    table.AddCell(avr);
                                    table.AddCell(Discount);
                                   
                      
                            
                                }

                                document.Add(table);
                                document.Close();

                                writer.Flush();
                                writer.Close();
                                memDocument.Flush();
                            }
                        }
                        //prepare output stream
                        //HttpContextHelper.Current.Response.ContentType = "application/pdf";
                        //HttpContextHelper.Current.Response.AddHeader("content-disposition", "attachment;filename=Performance.pdf");
                        //HttpContextHelper.Current.Response.Buffer = true;
                        //HttpContextHelper.Current.Response.Clear();
                        //HttpContextHelper.Current.Response.OutputStream.Write(memDocument.GetBuffer(), 0, memDocument.GetBuffer().Length);
                        //HttpContextHelper.Current.Response.OutputStream.Flush();
                        //HttpContextHelper.Current.Response.End();

                        //streamResult = new FileContentResult(HttpContextHelper.Current.Response.OutputStream, "application/pdf");

                     
                        streamResult = new FileContentResult(memDocument.ToArray(), new MediaTypeHeaderValue("application/pdf"))
                        {
                            FileDownloadName = "Performance.pdf"
                        };

                        break;
                    case "excel":
                        StringBuilder strExcelWriter = new StringBuilder();
                       addHeader(strExcelWriter, keyValueDtos);
                        strExcelWriter.Append(DateNameHeaderString);
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(BilledSegmentHeaderString);
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(UsedSegmentHeaderString);
                        strExcelWriter.Append("\t");
                        if (showAdvertiser)
                        {
                            strExcelWriter.Append(AdvertiserNameHeaderString);
                            strExcelWriter.Append("\t");
                        }

                        if (showCampaign)
                        {
                            strExcelWriter.Append(CampaignNameHeaderString);
                            strExcelWriter.Append("\t");
                        }
                        

                        strExcelWriter.Append(ImpressionsHeaderString);
                        strExcelWriter.Append("\t");

                  
                        strExcelWriter.Append(grossrevenueString);
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(RevenueHeaderString);
                    
                        strExcelWriter.Append("\t");
                        strExcelWriter.Append(AVRString);


                        strExcelWriter.Append("\t");
                    
                        strExcelWriter.Append(DiscountHeaderString);

                        strExcelWriter.Append("\n");





                        foreach (var item in ImpressionLogPerformanceList)
                        {
                            item.IsExport = true;

                            strExcelWriter.Append(DateTime.ParseExact(item.date.ToString(),
                                    "yyyyMMdd",
                                    CultureInfo.InvariantCulture,
                                    DateTimeStyles.None).ToShortDateString());
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.BilledSegment);
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.UsedSegments);
                            strExcelWriter.Append("\t");
                            if (showAdvertiser)
                            {
                                strExcelWriter.Append(item.AdvertiserName);
                                strExcelWriter.Append("\t");
                            }


                            if (showCampaign)
                            {
                                strExcelWriter.Append(item.CampaignName);
                                strExcelWriter.Append("\t");
                            }
                          


                            strExcelWriter.Append(item.Impressions);
                    
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.grossrevenueText);
                            strExcelWriter.Append("\t");

                  
                            strExcelWriter.Append(item.RevenueText);
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.avrcostText);
                            strExcelWriter.Append("\t");


                    
                            strExcelWriter.Append(item.DiscountText);
                        
                        
                            strExcelWriter.Append("\n");


                        }

                        //HttpContextHelper.Current.Response.Clear();
                        //HttpContextHelper.Current.Response.ClearHeaders();
                        //HttpContextHelper.Current.Response.ClearContent();
                        //HttpContextHelper.Current.Response.ContentType = "application/vnd.ms-excel";
                        //HttpContextHelper.Current.Response.AddHeader("content-disposition", "attachment;filename=Performance.xls");
                        //HttpContextHelper.Current.Response.HeaderEncoding = Encoding.UTF8;
                        //HttpContextHelper.Current.Response.AddHeader("Pragma", "public");
                        //HttpContextHelper.Current.Response.ContentEncoding = Encoding.Unicode;
                        //// Start the feed with BOM
                        //HttpContextHelper.Current.Response.BinaryWrite(Encoding.Unicode.GetPreamble());
                        //HttpContextHelper.Current.Response.Write(strExcelWriter.ToString());
                        //HttpContextHelper.Current.Response.End();

                        //streamResult = new FileContentResult(HttpContextHelper.Current.Response.OutputStream, "application/vnd.ms-excel");


                        var streamexce = new MemoryStream((new UTF8Encoding(true)).GetBytes(strExcelWriter.ToString()));

                        streamResult = new FileContentResult(streamexce.ToArray(), new MediaTypeHeaderValue("application/vnd.ms-excel"))
                        {
                            FileDownloadName = "Performance.xls"
                        };
                        break;
                    case "csv":
                        StringBuilder strCSVWriter = new StringBuilder();
                        addHeader(strCSVWriter, keyValueDtos);
                        strCSVWriter.Append(DateNameHeaderString);
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(BilledSegmentHeaderString);
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(UsedSegmentHeaderString);
                        strCSVWriter.Append(",");
                        if (showAdvertiser)
                        {
                            strCSVWriter.Append(AdvertiserNameHeaderString);
                            strCSVWriter.Append(",");
                        }
                        if (showCampaign)
                        {
                            strCSVWriter.Append(CampaignNameHeaderString);
                            strCSVWriter.Append(",");
                        }
                       

                        strCSVWriter.Append(ImpressionsHeaderString);


              
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(grossrevenueString);
                        strCSVWriter.Append(",");

                        strCSVWriter.Append(RevenueHeaderString);
                        strCSVWriter.Append(",");
                        strCSVWriter.Append(AVRString);
                        strCSVWriter.Append(",");
                   
                        strCSVWriter.Append(DiscountHeaderString);
                       
             

                 
                     
                  

                        strCSVWriter.Append("\n");


                        

                        foreach (var item in ImpressionLogPerformanceList)
                        {
                            item.IsExport = true;

                            strCSVWriter.Append(DateTime.ParseExact(item.date.ToString(),
                                    "yyyyMMdd",
                                    CultureInfo.InvariantCulture,
                                    DateTimeStyles.None).ToShortDateString());
                            strCSVWriter.Append(",");

                            strCSVWriter.Append(item.BilledSegment);
                            strCSVWriter.Append(",");
                            strCSVWriter.Append("\"" + item.UsedSegments + "\"");
                            strCSVWriter.Append(",");
                            if (showAdvertiser)
                            {
                                strCSVWriter.Append(item.AdvertiserName);
                                strCSVWriter.Append(",");
                            }

                            if (showCampaign)
                            {
                                strCSVWriter.Append(item.CampaignName);
                                strCSVWriter.Append(",");
                            }
                         

                            strCSVWriter.Append(item.Impressions);
                            strCSVWriter.Append(",");

                            strCSVWriter.Append(item.grossrevenueText);
                            strCSVWriter.Append(",");

                            strCSVWriter.Append(item.RevenueText);
                            strCSVWriter.Append(",");

                            strCSVWriter.Append(item.avrcostText);
                            strCSVWriter.Append(",");
                        
                            strCSVWriter.Append(item.DiscountText);
                            strCSVWriter.Append("\n");



                        }

                        //HttpContextHelper.Current.Response.Clear();
                        //HttpContextHelper.Current.Response.ClearHeaders();
                        //HttpContextHelper.Current.Response.ClearContent();
                        //HttpContextHelper.Current.Response.AddHeader("content-disposition", "attachment;filename=Performance.csv");
                        //HttpContextHelper.Current.Response.ContentType = "text/csv";
                        //HttpContextHelper.Current.Response.HeaderEncoding = Encoding.UTF8;
                        //HttpContextHelper.Current.Response.AddHeader("Pragma", "public");
                        //HttpContextHelper.Current.Response.ContentEncoding = Encoding.Unicode;
                        //// Start the feed with BOM
                        //HttpContextHelper.Current.Response.BinaryWrite(Encoding.Unicode.GetPreamble());
                        //HttpContextHelper.Current.Response.Write(strCSVWriter.ToString());
                        //HttpContextHelper.Current.Response.End();

                        //streamResult = new FileContentResult(HttpContextHelper.Current.Response.OutputStream, "text/csv");

                        var streamCSv = new MemoryStream((new UTF8Encoding(true)).GetBytes(strCSVWriter.ToString()));

                        streamResult = new FileContentResult(streamCSv.ToArray(), new MediaTypeHeaderValue("text/csv"))
                        {
                            FileDownloadName = "Performance.csv"
                        };

                        break;
                    default:
                        streamResult = new FileContentResult(memDocument.ToArray(), "text/plain");
                        break;
                }

                return streamResult;
            }
        }

        public FileContentResult BuildIAudienceSegmentsPerformanceExportFile(List<AudienceSegmentDto> AudienceSegmentsPerformanceList, string type, bool showCampaign, bool showAdvertiser, List<KeyValueDto> keyValueDtos, string NameTitle = "")
        {
            using (MemoryStream memDocument = new MemoryStream())
            {
                IAccountService _AccountService = ArabyAds.Framework.IoC.Instance.Resolve<IAccountService>(); 

                FileContentResult streamResult;
               
               NameTitle = ResourcesUtilities.GetResource("Name", "Global");
                
                string SegmentParent = "SegmentParent";

                string SegmentCod = "SegmentCod";
                string OperatorSegmentCode = "OperatorSegmentCode";
                string Price = ResourcesUtilities.GetResource("Price", "SSPFloorPrices");
                string Description = ResourcesUtilities.GetResource("Description", "Campaign");
                string IsSelectedable = ResourcesUtilities.GetResource("IsSelectedable", "Audience");
                string Id = "Id";
                string ParenId = "ParenId";
                switch (type.ToLower())
                {
                    case "pdf":
                        //using ()
                        {
                            Document document = new Document(PageSize.A4, 30, 30, 40, 40);
                            //using ()
                            {

                                PdfWriter writer = PdfWriter.GetInstance(document, memDocument);
                                writer.CloseStream = false;
                                document.Open();
                             addHeader(document, keyValueDtos);
                                iTextSharp.text.Font fontHeader = WriteReportDocumentsHelper.GetHeaderFont();
                                int tds = 5;
                                if (showCampaign)
                                {
                                    tds++;
                                }
                                if (showAdvertiser)
                                {
                                    tds++;
                                }
                                PdfPTable table = WriteReportDocumentsHelper.GetTable(tds);

                                PdfPCell IdHeader = WriteReportDocumentsHelper.GetCell(Id, fontHeader);
                                PdfPCell ParenIdHeader = WriteReportDocumentsHelper.GetCell(ParenId, fontHeader);
                                PdfPCell NameHeader = WriteReportDocumentsHelper.GetCell(NameTitle, fontHeader);
                                PdfPCell SegmentParentHeader = WriteReportDocumentsHelper.GetCell(SegmentParent, fontHeader);
                                PdfPCell SegmentCodeHeader = WriteReportDocumentsHelper.GetCell(SegmentCod, fontHeader);
                                PdfPCell OperatorSegmentCodeHeader = WriteReportDocumentsHelper.GetCell(OperatorSegmentCode, fontHeader);
                                PdfPCell PriceCodeHeader = WriteReportDocumentsHelper.GetCell(Price, fontHeader);
                               // PdfPCell DescriptionHeader = WriteReportDocumentsHelper.GetCell(Description, fontHeader);
                                PdfPCell IsSelectedableHeader = WriteReportDocumentsHelper.GetCell(IsSelectedable, fontHeader);

                              
                                table.AddCell(IdHeader);
                                table.AddCell(ParenIdHeader);

                                table.AddCell(NameHeader);
                                table.AddCell(SegmentParentHeader);
                                table.AddCell(SegmentCodeHeader);
                                table.AddCell(OperatorSegmentCodeHeader);
                                table.AddCell(PriceCodeHeader);

                              //  table.AddCell(DescriptionHeader);
                                table.AddCell(IsSelectedableHeader);
                                foreach (var item in AudienceSegmentsPerformanceList)
                                {
                                    iTextSharp.text.Font font = WriteReportDocumentsHelper.GetFont();

                                 

                                    PdfPCell IdName = WriteReportDocumentsHelper.GetCell(item.ID.ToString(), font);
                                    PdfPCell ParenIdName = WriteReportDocumentsHelper.GetCell(item.ParentId.ToString(), font);
                                    PdfPCell Namep = WriteReportDocumentsHelper.GetCell(item.Name.GetValue().ToString(), font);
                                    PdfPCell SegmentParentp = WriteReportDocumentsHelper.GetCell(item.ParentName, font);
                                    PdfPCell SegmentCodep = WriteReportDocumentsHelper.GetCell(item.CodeUQ.ToString(), font);
                                    PdfPCell OperatorSegmentCodep = WriteReportDocumentsHelper.GetCell(item.OperatorSegmentCode.ToString(), font);

                                    PdfPCell PriceCodep = WriteReportDocumentsHelper.GetCell(item.priceString, font);
                                  //  PdfPCell Descriptionp = WriteReportDocumentsHelper.GetCell(item.Description, font);
                                    PdfPCell IsSelectedablep= WriteReportDocumentsHelper.GetCell(item.IsSelectedable.ToString(), font);


                                    table.AddCell(IdName);
                                    table.AddCell(ParenIdName);

                                  
                                    table.AddCell(Namep);
                                    table.AddCell(SegmentParentp);


                                    table.AddCell(SegmentCodep);
                                    table.AddCell(OperatorSegmentCodep);


                                    table.AddCell(PriceCodep);
                                 //   table.AddCell(Descriptionp);
                                    table.AddCell(IsSelectedablep);
                                }

                                document.Add(table);
                                document.Close();

                                writer.Flush();
                                writer.Close();
                                memDocument.Flush();
                            }
                        }
                        //prepare output stream
                        //HttpContextHelper.Current.Response.ContentType = "application/pdf";
                        //HttpContextHelper.Current.Response.AddHeader("content-disposition", "attachment;filename=Segments.pdf");
                        //HttpContextHelper.Current.Response.Buffer = true;
                        //HttpContextHelper.Current.Response.Clear();
                        //HttpContextHelper.Current.Response.OutputStream.Write(memDocument.GetBuffer(), 0, memDocument.GetBuffer().Length);
                        //HttpContextHelper.Current.Response.OutputStream.Flush();
                        //HttpContextHelper.Current.Response.End();

                        //streamResult = new FileContentResult(HttpContextHelper.Current.Response.OutputStream, "application/pdf");


                        //var stream = new MemoryStream(Encoding.Unicode.GetBytes(strExcelWriter.ToString()));
                        streamResult = new FileContentResult(memDocument.ToArray(), new MediaTypeHeaderValue("application/pdf"))
                        {
                            FileDownloadName = "Segments.pdf"
                        };


                        break;
                    case "excel":
                        StringBuilder strExcelWriter = new StringBuilder();
                       addHeader(strExcelWriter, keyValueDtos);

                        var LevelCounts= AudienceSegmentsPerformanceList.Max(X => X.Level);

                        for (var c=1;c<= LevelCounts;c++)
                        {
                            strExcelWriter.Append("Level"+c);
                            strExcelWriter.Append("\t");
                        }
                  
                    

                       

                        strExcelWriter.Append(SegmentCod);
                        strExcelWriter.Append("\t");
              
                        strExcelWriter.Append(Price);
                        strExcelWriter.Append("\t");

                        //strExcelWriter.Append(Description);
                       // strExcelWriter.Append("\t");
               
                        strExcelWriter.Append("\n");
                        foreach (var item in AudienceSegmentsPerformanceList)
                        {
                            if (item.IsPermissionNeed && !Domain.Configuration.IsAdmin && !_AccountService.checkAdPermissions(new ValueMessageWrapper<Domain.Common.Model.Core.PortalPermissionsCode> { Value = Domain.Common.Model.Core.PortalPermissionsCode.AudianceSegmentUsagePermission }).Value)
                            {
                                continue;
                            }

                            for (var c = 0; c < item.Level; c++)
                            {
                                strExcelWriter.Append(item.Names[ c]);
                                strExcelWriter.Append("\t");
                            }

                            for (var c = 0; c < LevelCounts -item.Level  ; c++)
                            {
                                strExcelWriter.Append(" ");
                                strExcelWriter.Append("\t");
                            }

                            strExcelWriter.Append(item.CodeUQ.ToString());
                            strExcelWriter.Append("\t");
                            strExcelWriter.Append(item.priceString);
                            strExcelWriter.Append("\t");
                         //   strExcelWriter.Append(item.Description);
                         //   strExcelWriter.Append("\t");
                            strExcelWriter.Append("\n");


                        }

                        //HttpContextHelper.Current.Response.Clear();
                        //HttpContextHelper.Current.Response.ClearHeaders();
                        //HttpContextHelper.Current.Response.ClearContent();
                        //HttpContextHelper.Current.Response.ContentType = "application/vnd.ms-excel";
                        //HttpContextHelper.Current.Response.AddHeader("content-disposition", "attachment;filename=Segments.xls");
                        //HttpContextHelper.Current.Response.HeaderEncoding = Encoding.UTF8;
                        //HttpContextHelper.Current.Response.AddHeader("Pragma", "public");
                        //HttpContextHelper.Current.Response.ContentEncoding = Encoding.Unicode;
                        //// Start the feed with BOM
                        //HttpContextHelper.Current.Response.BinaryWrite(Encoding.Unicode.GetPreamble());
                        //HttpContextHelper.Current.Response.Write(strExcelWriter.ToString());
                        //HttpContextHelper.Current.Response.End();

                        //streamResult = new FileContentResult(HttpContextHelper.Current.Response.OutputStream, "application/vnd.ms-excel");

                        var stream = new MemoryStream((new UTF8Encoding(true)).GetBytes(strExcelWriter.ToString()));
                        streamResult = new FileContentResult(stream.ToArray(), new MediaTypeHeaderValue("application/vnd.ms-excel"))
                        {
                            FileDownloadName = "Segments.xls"
                        };



                        break;
                    case "csv":
                        StringBuilder strCSVWriter = new StringBuilder();
                       addHeader(strCSVWriter, keyValueDtos);

                        var LevelCountsd = AudienceSegmentsPerformanceList.Max(X => X.Level);

                        for (var c = 1; c <= LevelCountsd; c++)
                        {
                            strCSVWriter.Append("Level" + c);
                            strCSVWriter.Append("\t");
                        }





                        strCSVWriter.Append(SegmentCod);
                        strCSVWriter.Append("\t");

                        strCSVWriter.Append(Price);
                        strCSVWriter.Append("\t");

                       // strCSVWriter.Append(Description);
                       // strCSVWriter.Append("\t");

                        strCSVWriter.Append("\n");






                        foreach (var item in AudienceSegmentsPerformanceList)
                        {

                            if (item.IsPermissionNeed && !Domain.Configuration.IsAdmin && !_AccountService.checkAdPermissions(new ValueMessageWrapper<Domain.Common.Model.Core.PortalPermissionsCode> { Value = Domain.Common.Model.Core.PortalPermissionsCode.AudianceSegmentUsagePermission}).Value)
                            {
                                continue;
                            }
                            for (var c = 0; c < item.Level; c++)
                            {
                                strCSVWriter.Append(item.Names[c]);
                                strCSVWriter.Append(",");
                            }

                            for (var c = 0; c < LevelCountsd-item.Level; c++)
                            {
                                strCSVWriter.Append(" ");
                                strCSVWriter.Append(",");
                            }



                            strCSVWriter.Append(item.CodeUQ.ToString());
                            strCSVWriter.Append(",");
                            
                            strCSVWriter.Append(item.priceString);
                            strCSVWriter.Append(",");
                        ////    strCSVWriter.Append(item.Description);
                          ////  strCSVWriter.Append(",");
                            
                            strCSVWriter.Append("\n");



                        }

                        //HttpContextHelper.Current.Response.Clear();
                        //HttpContextHelper.Current.Response.ClearHeaders();
                        //HttpContextHelper.Current.Response.ClearContent();
                        //HttpContextHelper.Current.Response.AddHeader("content-disposition", "attachment;filename=Segments.csv");
                        //HttpContextHelper.Current.Response.ContentType = "text/csv";
                        //HttpContextHelper.Current.Response.HeaderEncoding = Encoding.UTF8;
                        //HttpContextHelper.Current.Response.AddHeader("Pragma", "public");
                        //HttpContextHelper.Current.Response.ContentEncoding = Encoding.Unicode;
                        //// Start the feed with BOM
                        //HttpContextHelper.Current.Response.BinaryWrite(Encoding.Unicode.GetPreamble());
                        //HttpContextHelper.Current.Response.Write(strCSVWriter.ToString());
                        //HttpContextHelper.Current.Response.End();

                        //streamResult = new FileContentResult(HttpContextHelper.Current.Response.OutputStream, "text/csv");

                        var streamcsv = new MemoryStream((new UTF8Encoding(true)).GetBytes(strCSVWriter.ToString()));
                        streamResult =new FileContentResult(streamcsv.ToArray(), new MediaTypeHeaderValue("text/csv"))
                        {
                            FileDownloadName = "Segments.csv"
                        };


                        break;
                    default:
                        streamResult = new FileContentResult(memDocument.ToArray(), "text/plain");
                        break;
                }

                return streamResult;
            }
        }

    }
}
