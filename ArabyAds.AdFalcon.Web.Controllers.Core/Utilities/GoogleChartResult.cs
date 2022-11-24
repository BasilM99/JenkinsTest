using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;

using System.IO;
using System.Drawing;
using ArabyAds.AdFalcon.Web.Controllers.Utilities;

namespace ArabyAds.AdFalcon.Web.Controllers.Utilities
{
    public enum ChartPeriod
    {
        Day,
        Week,
        Month,
  
        FourMonth,
        SixMonths,
        MoreThanSixMonths
    }
    public  class GoogleChartResult
    {

        

        #region Public Methods
            
        public bool slantedText { get; set; }
        public bool ForMonth { get; set; }
        public bool ForWeek { get; set; }
        public int slantedTextAngle { get; set; }
        public int showTextEvery { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<ChartDto> ChartDtoList { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Color { get; set; }
        public bool isRightToLeft { get; set; }
        public string HAxisText { get; set; }
        public string YAxisText { get; set; }
        public string OptionalParameter { get; set; }

        #endregion

        public GoogleChartResult() { }

        #region Private Members

    

      
        public ChartPeriod ChartPeriodType
        {
            get
            {
                TimeSpan diff = ToDate - FromDate;
                var dateDiff = ToDate.Subtract(FromDate).TotalDays;

                if (ForMonth)
                {
                    return ChartPeriod.Month;
                }
                if (ForWeek)
                {
                    return ChartPeriod.Week;
                }
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

        public  void ExecuteResult()
        {
            FixPoints();
            FixChartData();
            InitilizeChart();

            RearrangeChart();
            StylingChart();

          
            

        }
        public void ExecuteNewResult()
        {
            FixPoints();
            FixNewChartData();
            InitilizeChart();

            RearrangeChart();
            StylingChart();




        }
        private void FixPoints()
        {
            slantedText = true;
            slantedTextAngle = 0;
            showTextEvery = 4;
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
     
            this.isRightToLeft = (Config.CurrentDirection.ToLower() == "rtl" ? true : false);
           ChartDtoList= ChartDtoList.OrderBy(p => p.Xaxis).ToList();
          
            //if (ChartDtoList != null && ChartDtoList.Count != 0)
            //{
            //    foreach (var item in ChartDtoList.OrderBy(p => p.Xaxis))
            //    {
            //        seriesLine.Points.AddXY(item.Xaxis, item.Yaxis);
            //    }
            //}
            //else
            //{
            //    seriesLine.Points.AddXY(0, 0);
            //}

           
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

        public static DateTime GetNextWeekday(DateTime date, DayOfWeek day)
        {
            DateTime result = date.AddDays(1);
            while (result.DayOfWeek != day)
                result = result.AddDays(1);
            return result;
        }
        public static DateTime GetNextMonthStart(DateTime date)
        {
            DateTime result = date.AddDays(1);
            while (result.Day != 1)
                result = result.AddDays(1);
            return result;
        }

        public static DateTime GetPreviosWeekday(DateTime date, DayOfWeek day)
        {
            DateTime result = date.AddDays(-1);
            while (result.DayOfWeek != day)
                result = result.AddDays(-1);
            return result;
        }
        public static DateTime GetPreviousMonthStart(DateTime date)
        {
         
            DateTime result = date.AddMonths(-1);
               DateTime endOfMonth = new DateTime(result.Year, result.Month, 1).AddMonths(1).AddDays(-1);
         
            return endOfMonth;
        }
        private void FixChartData()
        {
            //TimeSpan diff = ToDate - FromDate;
            var todate = ToDate;
            var fromDate = FromDate;
            long? obyaxis = 0;
            int i = 0;      
            switch (ChartPeriodType)
            {
                  
                case ChartPeriod.Day:
                    fromDate = fromDate.AddHours(-1);
                    for (; fromDate < todate; fromDate = fromDate.AddHours(1))
                    {
                        var node = ChartDtoList.FirstOrDefault(p => p.Xaxis == fromDate);
                        if (node == null)//(!ChartDtoList.Any(p => p.Xaxis == fromDate))
                        {
                            int? obyaxist = 0;
                            if ((Framework.Utilities.Environment.GetServerTime().Minute < Config.CurrentHourMinMinute) &&
                                (fromDate.Hour >= Framework.Utilities.Environment.GetServerTime().Hour) &&
                                (fromDate.Date == Framework.Utilities.Environment.GetServerTime().Date))
                            {
                                obyaxist = null;
                            }
                            if(fromDate.Hour == 23 && i==0)
                            {
                                obyaxist = -1;

                                i++;
                            }
                            var emptyDto = new ChartDto { Xaxis = fromDate, Yaxis = obyaxist };
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
                    fromDate = fromDate.AddHours(-6);
                    obyaxis = -1;
                    for (; fromDate < todate; fromDate = fromDate.AddHours(6))
                    {

                        if (!ChartDtoList.Any(p => p.Xaxis == fromDate))
                        {
                            var emptyDto = new ChartDto { Xaxis = fromDate, Yaxis = obyaxis };
                            ChartDtoList.Add(emptyDto);
                        }
                        obyaxis = 0;
                    }
                    break;
                case ChartPeriod.Month:
                    fromDate = fromDate.AddDays(-1);
                    obyaxis = -1;
                    for (; fromDate < todate; fromDate = fromDate.AddDays(1))
                    {
                        if (!ChartDtoList.Any(p => p.Xaxis == fromDate))
                        {
                            var emptyDto = new ChartDto { Xaxis = fromDate, Yaxis = obyaxis };
                            ChartDtoList.Add(emptyDto);
                        }
                        obyaxis = 0;
                    }
                    break;
                case ChartPeriod.FourMonth:
                    fromDate = fromDate.AddDays(-4);
                    obyaxis = -1;
                    for (; fromDate < todate; fromDate = fromDate.AddDays(4))
                    {
                        if (!ChartDtoList.Any(p => p.Xaxis == fromDate))
                        {
                            var emptyDto = new ChartDto { Xaxis = fromDate, Yaxis = obyaxis };
                            ChartDtoList.Add(emptyDto);
                        }
                        obyaxis = 0;
                    }
                    break;
                case ChartPeriod.SixMonths:
                    fromDate = GetPreviosWeekday(fromDate, DayOfWeek.Sunday);
                    obyaxis = -1;
                    for (; fromDate < todate; fromDate = GetNextWeekday(fromDate, DayOfWeek.Sunday))
                    {
                        if (!ChartDtoList.Any(p => p.Xaxis == fromDate))
                        {
                            var emptyDto = new ChartDto { Xaxis = fromDate, Yaxis = obyaxis };
                            ChartDtoList.Add(emptyDto);
                        }
                        obyaxis = 0;
                    }
                    break;
                case ChartPeriod.MoreThanSixMonths:
                    fromDate = GetPreviousMonthStart(fromDate);
                    obyaxis =-1;
                    for (; fromDate < todate; fromDate = GetNextMonthStart(fromDate))
                    {
                        if (!ChartDtoList.Any(p => p.Xaxis == fromDate))
                        {
                            var emptyDto = new ChartDto { Xaxis = fromDate, Yaxis = obyaxis };
                            ChartDtoList.Add(emptyDto);
                        }
                        obyaxis = 0;
                    }
                    break;
                default:
                    fromDate = fromDate.AddDays(-1);
                    obyaxis = -1;
                    for (; fromDate < todate; fromDate = fromDate.AddDays(1))
                    {
                        if (!ChartDtoList.Any(p => p.Xaxis == fromDate))
                        {
                            var emptyDto = new ChartDto { Xaxis = fromDate, Yaxis = obyaxis };
                            ChartDtoList.Add(emptyDto);
                        }
                        obyaxis = 0;
                    }
                    break;
            }

        }
        private void FixNewChartData()
        {
            //TimeSpan diff = ToDate - FromDate;
            var todate = ToDate;
            var fromDate = FromDate;
            long? obyaxis = 0;
            int i = 0;

          
            showTextEvery = 2;
            switch (ChartPeriodType)
            {

                case ChartPeriod.Day:
                    showTextEvery = 2;
                    slantedTextAngle = 0;
                    fromDate = fromDate.AddHours(-1);
                    for (; fromDate < todate; fromDate = fromDate.AddHours(1))
                    {
                        var node = ChartDtoList.FirstOrDefault(p => p.Xaxis == fromDate);
                        if (node == null)//(!ChartDtoList.Any(p => p.Xaxis == fromDate))
                        {
                            int? obyaxist = 0;
                            if ((Framework.Utilities.Environment.GetServerTime().Minute < Config.CurrentHourMinMinute) &&
                                (fromDate.Hour >= Framework.Utilities.Environment.GetServerTime().Hour) &&
                                (fromDate.Date == Framework.Utilities.Environment.GetServerTime().Date))
                            {
                                obyaxist = null;
                            }
                            if (fromDate.Hour == 23 && i == 0)
                            {//by Anas
                                //obyaxist = -1;

                                i++;
                            }
                            var emptyDto = new ChartDto { Xaxis = fromDate, Yaxis = obyaxist };
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
                    fromDate = fromDate.AddHours(-6);
                    showTextEvery = 2;
                    slantedTextAngle = 0;
                    obyaxis = 0;
                    for (; fromDate < todate; fromDate = fromDate.AddHours(6))
                    {

                        if (!ChartDtoList.Any(p => p.Xaxis == fromDate))
                        {
                            var emptyDto = new ChartDto { Xaxis = fromDate, Yaxis = obyaxis };
                            ChartDtoList.Add(emptyDto);
                        }
                        obyaxis = 0;
                    }
                    break;
                case ChartPeriod.Month:
                    fromDate = fromDate.AddDays(-1);
                    obyaxis = 0;
                    showTextEvery = 2;
                    slantedTextAngle = 0;
                    for (; fromDate < todate; fromDate = fromDate.AddDays(1))
                    {
                        if (!ChartDtoList.Any(p => p.Xaxis == fromDate))
                        {
                            var emptyDto = new ChartDto { Xaxis = fromDate, Yaxis = obyaxis };
                            ChartDtoList.Add(emptyDto);
                        }
                        obyaxis = 0;
                    }
                    break;
                case ChartPeriod.FourMonth:
                    fromDate = fromDate.AddDays(-4);
                    obyaxis = 0;

                    showTextEvery = 5;
                    slantedTextAngle = 0;
                    for (; fromDate < todate; fromDate = fromDate.AddDays(4))
                    {
                        if (!ChartDtoList.Any(p => p.Xaxis == fromDate))
                        {
                            var emptyDto = new ChartDto { Xaxis = fromDate, Yaxis = obyaxis };
                            ChartDtoList.Add(emptyDto);
                        }
                        obyaxis = 0;
                    }
                    break;
                case ChartPeriod.SixMonths:
                    fromDate = GetPreviosWeekday(fromDate, DayOfWeek.Sunday);
                    obyaxis = 0;
                    showTextEvery = 2;
                    slantedTextAngle = 0;
                    for (; fromDate < todate; fromDate = GetNextWeekday(fromDate, DayOfWeek.Sunday))
                    {
                        if (!ChartDtoList.Any(p => p.Xaxis == fromDate))
                        {
                            var emptyDto = new ChartDto { Xaxis = fromDate, Yaxis = obyaxis };
                            ChartDtoList.Add(emptyDto);
                        }
                        obyaxis = 0;
                    }
                    break;
                case ChartPeriod.MoreThanSixMonths:
                    fromDate = GetPreviousMonthStart(fromDate);
                    obyaxis = 0;
                    showTextEvery = 2;
                    slantedTextAngle = 0;
                    for (; fromDate < todate; fromDate = GetNextMonthStart(fromDate))
                    {
                        if (!ChartDtoList.Any(p => p.Xaxis == fromDate))
                        {
                            var emptyDto = new ChartDto { Xaxis = fromDate, Yaxis = obyaxis };
                            ChartDtoList.Add(emptyDto);
                        }
                        obyaxis = 0;
                    }
                    break;
                default:
                    fromDate = fromDate.AddDays(-1);
                    obyaxis = 0;
                    showTextEvery = 2;
                    slantedTextAngle = 0;
                    for (; fromDate < todate; fromDate = fromDate.AddDays(1))
                    {
                        if (!ChartDtoList.Any(p => p.Xaxis == fromDate))
                        {
                            var emptyDto = new ChartDto { Xaxis = fromDate, Yaxis = obyaxis };
                            ChartDtoList.Add(emptyDto);
                        }
                        obyaxis = 0;
                    }
                    break;
            }

        }

        private void RearrangeChart()
        {
            TimeSpan diff = ToDate - FromDate;
           // ChartComponent.Customize += new EventHandler(chart1_Customize);
            string formate = "";
            switch (ChartPeriodType)
            {
                case ChartPeriod.Day:
                    formate = "HH";
                    slantedText = false;
                    //ChartComponent.Series[0].XValueType = ChartValueType.Time;
                    //ChartComponent.ChartAreas[0].AxisX.Interval = 1;
                    //ChartComponent.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Hours;
                    //ChartComponent.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
                    //ChartComponent.ChartAreas[0].AxisX.LabelStyle.Angle = 0;
                    break;
                case ChartPeriod.Week:
                    //string.Format("dddd, d MMM {0}'hour.' HH:mm", System.Environment.NewLine)
            // , culture)
                    formate = "dd/MM\r\nHH:mm";
                    //ChartComponent.Series[0].XValueType = ChartValueType.DateTime;
                    //ChartComponent.Series[0].XAxisType = AxisType.Primary;
                    //ChartComponent.ChartAreas[0].AxisX.Interval = 6;
                    //ChartComponent.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Hours;
                    //ChartComponent.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
                    //ChartComponent.ChartAreas[0].AxisX.LabelStyle.Angle = 90;
                    //ChartComponent.ChartAreas[0].AxisX.LabelStyle.Format = "MM/dd HH:mm";

                    break;
                case ChartPeriod.Month:
                    formate = "dd/MM";
                    //ChartComponent.Series[0].XValueType = ChartValueType.Date;
                    //ChartComponent.ChartAreas[0].AxisX.Interval = 1;
                    //ChartComponent.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Days;
                    //ChartComponent.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
                    //ChartComponent.ChartAreas[0].AxisX.LabelStyle.Angle = 90;
                    break;
                case ChartPeriod.FourMonth:
                    formate = "dd/MM";
                    //ChartComponent.Series[0].XValueType = ChartValueType.Date;
                    //ChartComponent.ChartAreas[0].AxisX.Interval = 1;
                    //ChartComponent.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Days;
                    //ChartComponent.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
                    //ChartComponent.ChartAreas[0].AxisX.LabelStyle.Angle = 90;
                    break;
                case ChartPeriod.SixMonths:
                    formate = "dd/MM";
                    //ChartComponent.Series[0].XValueType = ChartValueType.Date;
                    //ChartComponent.ChartAreas[0].AxisX.Interval = 1;
                    //ChartComponent.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Weeks;
                    //ChartComponent.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
                    //ChartComponent.ChartAreas[0].AxisX.LabelStyle.Angle = 90;
                    break;
                case ChartPeriod.MoreThanSixMonths:
                    formate = "dd/MM";
                    //ChartComponent.Series[0].XValueType = ChartValueType.Date;
                    //ChartComponent.ChartAreas[0].AxisX.Interval = 1;
                    //ChartComponent.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Months;
                    //ChartComponent.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
                    //ChartComponent.ChartAreas[0].AxisX.LabelStyle.Angle = 90;
                    break;
                default:
                    formate = "dd/MM";
                    //ChartComponent.Series[0].XValueType = ChartValueType.Date;
                    //ChartComponent.ChartAreas[0].AxisX.Interval = 1;
                    //ChartComponent.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Days;
                    //ChartComponent.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
                    //ChartComponent.ChartAreas[0].AxisX.LabelStyle.Angle = 90;
                    break;
            }


            if (ChartDtoList != null && ChartDtoList.Count != 0)
            {
                foreach (var item in ChartDtoList)
                {
                    item.XaxisString = item.Xaxis.ToString(formate);
                }
            }
        }

        private void StylingChart()
        {
           

            switch (ChartPeriodType)
            {
                case ChartPeriod.Day:
                    this.HAxisText = "00:00"+ " - " + "23:59" + "  " + ResourcesUtilities.GetResource("GMT", "Global");
                    break;
                case ChartPeriod.Week:
                case ChartPeriod.Month:
                default:
                    this.HAxisText = FromDate.ToShortDateString() + "  -  " + ToDate.ToShortDateString() + "  " + ResourcesUtilities.GetResource("GMT", "Global");
                    break;
            }
        }

    
    }
    



}
