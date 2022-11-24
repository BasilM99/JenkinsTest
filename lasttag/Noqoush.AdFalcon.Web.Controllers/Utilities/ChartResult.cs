using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports;
using System.Web.UI.DataVisualization.Charting;
using System.IO;
using System.Drawing;
using Noqoush.AdFalcon.Web.Controllers.Utilities;

namespace Noqoush.AdFalcon.Web.Helper
{
    public class ChartResult : ActionResult
    {

        enum ChartPeriod
        {
            Day,
            Week,
            Month,
            FourMonth,
            SixMonths,
            MoreThanSixMonths
        }

        #region Public Methods

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<ChartDto> ChartDtoList { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Color { get; set; }


        #endregion

        public ChartResult() { }

        #region Private Members

        private Chart ChartComponent = new Chart();

      
        private ChartPeriod ChartPeriodType
        {
            get
            {
                TimeSpan diff = ToDate - FromDate;
                var dateDiff = ToDate.Subtract(FromDate).TotalDays;
                if (dateDiff <= 1)
                {
                    return ChartPeriod.Day;
                }
                if ((dateDiff > 1) && (dateDiff <= 7))
                {
                    return ChartPeriod.Week;
                }
                if ((dateDiff > 7) && (dateDiff <= 31))
                {
                    return ChartPeriod.Month;
                }
                if ((dateDiff > 31) && (dateDiff <= 122))
                {
                    return ChartPeriod.FourMonth;
                }
                if ((dateDiff > 122) && (dateDiff <= 180))
                {
                    return ChartPeriod.SixMonths;
                }
                else
                {
                    return ChartPeriod.MoreThanSixMonths;
                }
             
            }
        }

        #endregion

        public override void ExecuteResult(ControllerContext context)
        {
            FixPoints();
            FixChartData();
            InitilizeChart();

            RearrangeChart();
            StylingChart();

            context.HttpContext.Response.Clear();
            context.HttpContext.Response.ContentType = "image/png";

            // Send image in base64 byte array to be readable from ajax code and assign the result
            // as raw data to html image
            using (MemoryStream memoryStream = new MemoryStream())
            {

                ChartComponent.SaveImage(memoryStream, ChartImageFormat.Png);
                Byte[] inArray = memoryStream.ToArray();

                string base64String = Convert.ToBase64String(inArray, 0, inArray.Length);
                byte[] base64Array = Encoding.UTF8.GetBytes(base64String);

                context.HttpContext.Response.OutputStream.Write(base64Array, 0, base64Array.Length);
            }
            

        }

        private void FixPoints()
        {
            //fix date Times
            FromDate = FromDate.Date;
            //ToDate = ToDate.Date.AddHours(Framework.Utilities.Environment.GetServerTime().Hour > 1 ? Framework.Utilities.Environment.GetServerTime().Hour - 1 : Framework.Utilities.Environment.GetServerTime().Hour);
            ToDate = ToDate.Date.AddHours(24);
            //check for outrange points
            //get the min point
            var lessThanMin = ChartDtoList.FirstOrDefault(p => p.Xaxis < FromDate);
            if (lessThanMin != null)
            {
                lessThanMin.Xaxis = FromDate;
            }
            //get the Max point
            var moreThanMax = ChartDtoList.FirstOrDefault(p => p.Xaxis > ToDate);
            if (moreThanMax != null)
            {
                moreThanMax.Xaxis = ToDate;
            }
        }

        private void InitilizeChart()
        {
            ChartComponent.Width = Width;
            ChartComponent.Height = Height;
            ChartComponent.RightToLeft = (Config.CurrentDirection.ToLower() == "rtl" ? RightToLeft.Yes : RightToLeft.No);

            ChartArea firstArea = new ChartArea("First Area");

            Series seriesLine = new Series("First Series");
            seriesLine.ChartType = SeriesChartType.FastLine;
            seriesLine.ChartArea = "First Area";
            seriesLine.Color = (Color)ColorTranslator.FromHtml(Color);

            if (ChartDtoList != null && ChartDtoList.Count != 0)
            {
                foreach (var item in ChartDtoList.OrderBy(p => p.Xaxis))
                {
                    seriesLine.Points.AddXY(item.Xaxis, item.Yaxis);
                }
            }
            else
            {
                seriesLine.Points.AddXY(0, 0);
            }

            ChartComponent.ChartAreas.Add(firstArea);
            ChartComponent.Series.Add(seriesLine);
        }

        //private void FixChartData()
        //{
        //    TimeSpan diff = ToDate - FromDate;

        //    switch (ChartPeriodType)
        //    {
        //        case ChartPeriod.Day:
        //            for (int i = 0; i < diff.TotalHours; i++)
        //            {
        //                if (ChartDtoList.Where(p => p.Xaxis == FromDate.AddHours(i)).Count() == 0)
        //                {
        //                    ChartDto emptyDto = new ChartDto();
        //                    emptyDto.Xaxis = FromDate.AddHours(i);
        //                    emptyDto.Yaxis = 0;
        //                    ChartDtoList.Add(emptyDto);
        //                }
        //            }
        //            break;
        //        case ChartPeriod.Week:
        //            for (int i = 0; i < (diff.TotalHours / 6); i++)
        //            {
        //                if (ChartDtoList.Where(p => p.Xaxis == FromDate.AddHours(i * 6)).Count() == 0)
        //                {
        //                    ChartDto emptyDto = new ChartDto();
        //                    emptyDto.Xaxis = FromDate.AddHours(i * 6);
        //                    emptyDto.Yaxis = 0;
        //                    ChartDtoList.Add(emptyDto);
        //                }
        //            }
        //            break;
        //        case ChartPeriod.Month:
        //            for (int i = 0; i < diff.TotalDays; i++)
        //            {
        //                if (ChartDtoList.Where(p => p.Xaxis == FromDate.AddDays(i)).Count() == 0)
        //                {
        //                    ChartDto emptyDto = new ChartDto();
        //                    emptyDto.Xaxis = FromDate.AddDays(i);
        //                    emptyDto.Yaxis = 0;
        //                    ChartDtoList.Add(emptyDto);
        //                }
        //            }
        //            break;
        //        case ChartPeriod.FourMonth:
        //            for (int i = 0; i < diff.TotalDays / 4; i++)
        //            {
        //                if (ChartDtoList.Where(p => p.Xaxis == FromDate.AddDays(i*4)).Count() == 0)
        //                {
        //                    ChartDto emptyDto = new ChartDto();
        //                    emptyDto.Xaxis = FromDate.AddDays(i);
        //                    emptyDto.Yaxis = 0;
        //                    ChartDtoList.Add(emptyDto);
        //                }
        //            }
        //            break;
        //        case ChartPeriod.SixMonths:
        //            for (int i = 0; i < (diff.TotalDays / 7); i++)
        //            {
        //                if (ChartDtoList.Where(p => p.Xaxis == FromDate.AddDays(i * 7)).Count() == 0)
        //                {
        //                    ChartDto emptyDto = new ChartDto();
        //                    emptyDto.Xaxis = FromDate.AddDays(i * 7);
        //                    emptyDto.Yaxis = 0;
        //                    ChartDtoList.Add(emptyDto);
        //                }
        //            }
        //            break;
        //        case ChartPeriod.MoreThanSixMonths:
        //            for (int i = 0; i < (diff.TotalDays / 31); i++)
        //            {
        //                if (ChartDtoList.Where(p => p.Xaxis == FromDate.AddDays(i * 7)).Count() == 0)
        //                {
        //                    ChartDto emptyDto = new ChartDto();
        //                    emptyDto.Xaxis = FromDate.AddDays(i * 7);
        //                    emptyDto.Yaxis = 0;
        //                    ChartDtoList.Add(emptyDto);
        //                }
        //            }
        //            break;
        //        default:
        //            for (int i = 0; i < diff.TotalDays; i++)
        //            {
        //                if (ChartDtoList.Where(p => p.Xaxis == FromDate.AddDays(i)).Count() == 0)
        //                {
        //                    ChartDto emptyDto = new ChartDto();
        //                    emptyDto.Xaxis = FromDate.AddDays(i);
        //                    emptyDto.Yaxis = 0;
        //                    ChartDtoList.Add(emptyDto);
        //                }
        //            }
        //            break;
        //    }

        //}

        static DateTime GetNextWeekday(DateTime date, DayOfWeek day)
        {
            DateTime result = date.AddDays(1);
            while (result.DayOfWeek != day)
                result = result.AddDays(1);
            return result;
        }
        static DateTime GetNextMonthStart(DateTime date)
        {
            DateTime result = date.AddDays(1);
            while (result.Day != 1)
                result = result.AddDays(1);
            return result;
        }
        private void FixChartData()
        {
            //TimeSpan diff = ToDate - FromDate;
            var todate = ToDate;
            var fromDate = FromDate;
            switch (ChartPeriodType)
            {
                case ChartPeriod.Day:
                    for (; fromDate < todate; fromDate = fromDate.AddHours(1))
                    {
                        var node = ChartDtoList.FirstOrDefault(p => p.Xaxis == fromDate);
                        if (node == null)//(!ChartDtoList.Any(p => p.Xaxis == fromDate))
                        {
                            object yaxis = 0;
                            if ((Framework.Utilities.Environment.GetServerTime().Minute < Config.CurrentHourMinMinute) &&
                                (fromDate.Hour >= Framework.Utilities.Environment.GetServerTime().Hour) &&
                                (fromDate.Date == Framework.Utilities.Environment.GetServerTime().Date))
                            {
                                yaxis = null;
                            }
                            var emptyDto = new ChartDto { Xaxis = fromDate, Yaxis = yaxis };
                            ChartDtoList.Add(emptyDto);
                        }
                        else
                        {
                            if (
                                (Framework.Utilities.Environment.GetServerTime().Minute < Config.CurrentHourMinMinute) &&
                                (fromDate.Hour == Framework.Utilities.Environment.GetServerTime().Hour) &&
                                (fromDate.Date == Framework.Utilities.Environment.GetServerTime().Date))
                            {
                                node.Yaxis = null;
                            }
                        }
                    }
                    break;
                case ChartPeriod.Week:
                    for (; fromDate < todate; fromDate = fromDate.AddHours(6))
                    {
                        if (!ChartDtoList.Any(p => p.Xaxis == fromDate))
                        {
                            var emptyDto = new ChartDto { Xaxis = fromDate, Yaxis = 0 };
                            ChartDtoList.Add(emptyDto);
                        }
                    }
                    break;
                case ChartPeriod.Month:
                    for (; fromDate < todate; fromDate = fromDate.AddDays(1))
                    {
                        if (!ChartDtoList.Any(p => p.Xaxis == fromDate))
                        {
                            var emptyDto = new ChartDto { Xaxis = fromDate, Yaxis = 0 };
                            ChartDtoList.Add(emptyDto);
                        }
                    }
                    break;
                case ChartPeriod.FourMonth:
                    for (; fromDate < todate; fromDate = fromDate.AddDays(4))
                    {
                        if (!ChartDtoList.Any(p => p.Xaxis == fromDate))
                        {
                            var emptyDto = new ChartDto { Xaxis = fromDate, Yaxis = 0 };
                            ChartDtoList.Add(emptyDto);
                        }
                    }
                    break;
                case ChartPeriod.SixMonths:

                    for (; fromDate < todate; fromDate = GetNextWeekday(fromDate, DayOfWeek.Sunday))
                    {
                        if (!ChartDtoList.Any(p => p.Xaxis == fromDate))
                        {
                            var emptyDto = new ChartDto { Xaxis = fromDate, Yaxis = 0 };
                            ChartDtoList.Add(emptyDto);
                        }
                    }
                    break;
                case ChartPeriod.MoreThanSixMonths:
                    for (; fromDate < todate; fromDate = GetNextMonthStart(fromDate))
                    {
                        if (!ChartDtoList.Any(p => p.Xaxis == fromDate))
                        {
                            var emptyDto = new ChartDto { Xaxis = fromDate, Yaxis = 0 };
                            ChartDtoList.Add(emptyDto);
                        }
                    }
                    break;
                default:
                    for (; fromDate < todate; fromDate = fromDate.AddDays(1))
                    {
                        if (!ChartDtoList.Any(p => p.Xaxis == fromDate))
                        {
                            var emptyDto = new ChartDto { Xaxis = fromDate, Yaxis = 0 };
                            ChartDtoList.Add(emptyDto);
                        }
                    }
                    break;
            }

        }

        private void RearrangeChart()
        {
            TimeSpan diff = ToDate - FromDate;
            ChartComponent.Customize += new EventHandler(chart1_Customize);

            switch (ChartPeriodType)
            {
                case ChartPeriod.Day:
                    ChartComponent.Series[0].XValueType = ChartValueType.Time;
                    ChartComponent.ChartAreas[0].AxisX.Interval = 1;
                    ChartComponent.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Hours;
                    ChartComponent.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
                    ChartComponent.ChartAreas[0].AxisX.LabelStyle.Angle = 0;
                    break;
                case ChartPeriod.Week:
                    ChartComponent.Series[0].XValueType = ChartValueType.DateTime;
                    ChartComponent.Series[0].XAxisType = AxisType.Primary;
                    ChartComponent.ChartAreas[0].AxisX.Interval = 6;
                    ChartComponent.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Hours;
                    ChartComponent.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
                    ChartComponent.ChartAreas[0].AxisX.LabelStyle.Angle = 90;
                    ChartComponent.ChartAreas[0].AxisX.LabelStyle.Format = "MM/dd HH:mm";

                    break;
                case ChartPeriod.Month:
                    ChartComponent.Series[0].XValueType = ChartValueType.Date;
                    ChartComponent.ChartAreas[0].AxisX.Interval = 1;
                    ChartComponent.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Days;
                    ChartComponent.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
                    ChartComponent.ChartAreas[0].AxisX.LabelStyle.Angle = 90;
                    break;
                case ChartPeriod.FourMonth:
                    ChartComponent.Series[0].XValueType = ChartValueType.Date;
                    ChartComponent.ChartAreas[0].AxisX.Interval = 1;
                    ChartComponent.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Days;
                    ChartComponent.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
                    ChartComponent.ChartAreas[0].AxisX.LabelStyle.Angle = 90;
                    break;
                case ChartPeriod.SixMonths:
                    ChartComponent.Series[0].XValueType = ChartValueType.Date;
                    ChartComponent.ChartAreas[0].AxisX.Interval = 1;
                    ChartComponent.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Weeks;
                    ChartComponent.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
                    ChartComponent.ChartAreas[0].AxisX.LabelStyle.Angle = 90;
                    break;
                case ChartPeriod.MoreThanSixMonths:
                    ChartComponent.Series[0].XValueType = ChartValueType.Date;
                    ChartComponent.ChartAreas[0].AxisX.Interval = 1;
                    ChartComponent.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Months;
                    ChartComponent.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
                    ChartComponent.ChartAreas[0].AxisX.LabelStyle.Angle = 90;
                    break;
                default:
                    ChartComponent.Series[0].XValueType = ChartValueType.Date;
                    ChartComponent.ChartAreas[0].AxisX.Interval = 1;
                    ChartComponent.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Days;
                    ChartComponent.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
                    ChartComponent.ChartAreas[0].AxisX.LabelStyle.Angle = 90;
                    break;
            }
        }

        private void StylingChart()
        {
            ChartComponent.BackColor = (Color)ColorTranslator.FromHtml("#dedee9");
            ChartComponent.BorderlineWidth = 1;
            ChartComponent.BorderlineColor = (Color)ColorTranslator.FromHtml("#676890");
            ChartComponent.BorderlineDashStyle = ChartDashStyle.Solid;
            ChartComponent.BackHatchStyle = ChartHatchStyle.Percent10;

            ElementPosition areaPosition = new ElementPosition(0, 5, 95, 95);
            ChartComponent.ChartAreas[0].Position = areaPosition;
            ChartComponent.ChartAreas[0].BackHatchStyle = ChartHatchStyle.Percent10;

            ChartComponent.ChartAreas[0].BackColor = (Color)ColorTranslator.FromHtml("#dedee9");
            ChartComponent.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            ChartComponent.ChartAreas[0].AxisY.MajorGrid.LineColor = (Color)ColorTranslator.FromHtml("#676890");

            ChartComponent.Series[0].BorderWidth = 3;
            ChartComponent.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Verdana", 7, FontStyle.Regular);
            ChartComponent.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Verdana", 7, FontStyle.Bold);
            ChartComponent.ChartAreas[0].AxisX.LabelAutoFitStyle = LabelAutoFitStyles.DecreaseFont;
            ChartComponent.ChartAreas[0].AxisX.IsLabelAutoFit = false;

            ChartComponent.ChartAreas[0].AxisX.TitleFont = new Font("Verdana", 9, FontStyle.Bold);

            switch (ChartPeriodType)
            {
                case ChartPeriod.Day:
                    ChartComponent.ChartAreas[0].AxisX.Title = FromDate.ToShortTimeString() + " - " + ToDate.ToShortTimeString() + "  " + ResourcesUtilities.GetResource("GMT", "Global");
                    break;
                case ChartPeriod.Week:
                case ChartPeriod.Month:
                default:
                    ChartComponent.ChartAreas[0].AxisX.Title = FromDate.ToShortDateString() + "  -  " + ToDate.ToShortDateString() + "  " + ResourcesUtilities.GetResource("GMT", "Global");
                    break;
            }
        }

        private void chart1_Customize(object sender, EventArgs e)
        {
            double range = (ChartComponent.ChartAreas[0].AxisX.CustomLabels.Count / 15d);
            TimeSpan diff = ToDate - FromDate;

            if (range > 1)
            {
                int i = 0;
                foreach (var item in ChartComponent.ChartAreas[0].AxisX.CustomLabels)
                {
                    if (i % 2 != 0)
                    {
                        item.Text = "";
                    }
                    else
                    {
                        if (ChartPeriodType == ChartResult.ChartPeriod.Day)
                        {
                            DateTime itemDateTime = Convert.ToDateTime(item.Text);

                            item.Text = itemDateTime.ToString("HH");
                        }
                    }

                    i++;
                }
            }
        }
    }
}
