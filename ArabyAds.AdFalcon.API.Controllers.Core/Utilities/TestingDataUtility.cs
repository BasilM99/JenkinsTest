using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArabyAds.AdFalcon.API.Controllers.Utilities
{
    public static class TestingDataUtility
    {
        public static List<AppSiteStatisticsReport> GetAppSiteStatistcsReport()
        {
            return new List<AppSiteStatisticsReport>()
            {
                new  AppSiteStatisticsReport()
                {
                    d="2013-01-18 00:00:00",
                    aid= "894473414d7d40a680ce8a166d9087e7",
                    an = "AppSite_1",
                    i= 25028851,
                    rv=5413.58070m,
                    r=553194271,
                    c=215473
                },
                new  AppSiteStatisticsReport()
                {
                    d = "2013-01-18 00:00:00",
                    aid= "39c6f633058a4f70b0efe0de13530f3e",
                    an= "AppSite_2",
                    i=0,
                    rv=0.00000m,
                    r=9225,
                    c=0
                 },
                 new  AppSiteStatisticsReport()
                 {
                    d="2013-01-18 00:00:00",
                    aid="20713eb9e4e943ff813a817a65fcd319",
                    an="AppSite_3",
                    i=228529,
                    rv=48.61006m,
                    r=6746002,
                    c=1740
                 },
                 new  AppSiteStatisticsReport()
                 {
                    d="2013-01-18 00:00:00",
                    aid="66817d30e54f4292981d38cf296546b4",
                    an="AppSite_4",
                    i=3854076,
                    rv=227.90281m,
                    r=98485680,
                    c=9639
                 },
                 new  AppSiteStatisticsReport()
                 {
                    d="2013-01-18 00:00:00",
                    aid="5cb974aeb1294d158f38e423604096e1",
                    an="AppSite_5",
                    i=8614255,
                    rv=610.45393m,
                    r=157910765,
                    c=25022
                 },
                 new AppSiteStatisticsReport()
                 {
                     d = "2013-01-18 00:00:00",
                     aid= "57533d6da3134789a7a4761bd3280738",
                     an = "AppSite_6",
                     i= 5647862,
                     rv = 3354.18105m,
                     r = 45817997,
                     c = 98429
                 },
                 new AppSiteStatisticsReport()
                 {
                     d = "2013-01-18 00:00:00",
                     aid= "ea66bf183ad84580b3c529b5c7feb73b",
                     an = "AppSite_7",
                     i= 5219862,
                     rv = 2355.01105m,
                     r = 30117997,
                     c = 94429
                 },
                 new AppSiteStatisticsReport()
                 {
                     d ="2013-01-18 00:00:00",
                     aid = "075250857a514b0c8a01bf35531d5bcd", 
                     an = "AppSite_8",
                     i = 0,
                     rv = 0.00000m,
                     r=12685,
                     c=0
                 },
                 new AppSiteStatisticsReport()
                 {
                     d = "2013-01-18 00:00:00",
                     aid = "f47762a81eb749fa8d4d3fc7e7162a2f",
                     an = "AppSite_9",
                     i= 7984,
                     rv = 1.19572m,
                     r= 23467,
                     c = 44
                 },
                 new AppSiteStatisticsReport()
                 {
                     d = "2013-01-18 00:00:00",
                     aid = "a16b9672cfb24c38b735acc9ba04aa6d",
                     an = "AppSite_10",
                     i = 331235,
                     rv = 66.75582m,
                     r = 3030948,
                     c= 2711
                 }
                                
            };
        }

        public static List<AppSiteStatisticsGeoReport> GetAppSiteStatistcsGeoReport()
        {
            return new List<AppSiteStatisticsGeoReport>()
            {
                new AppSiteStatisticsGeoReport()
                {
                    cc = "null",
                    d = "2013-01-18 00:00:00",
                    aid = "1f03597dae084565afeedcdab041e7d2",
                    an = "AppSite_1",
                    i = 2,
                    rv = 0.00000m,
                    r = 476331,
                    c = 0
                },
                new AppSiteStatisticsGeoReport()
                {
                   cc = "JO",
                   d = "2013-01-18 00:00:00",
                   aid = "03e44ad6471d4fc890173f95b7efeb4d",
                   an = "AppSite_2",
                   i = 19361,
                   rv = 5.26850m,
                   r = 258395,
                   c = 187
                },
                new AppSiteStatisticsGeoReport()
                {
                   cc = "QA",
                   d = "2013-01-18 00:00:00",
                   aid = "f85bcb96d9654f6bace8d23ccfb2172e",
                   an = "AppSite_3",
                   i = 11740,
                   rv = 3.97953m,
                   r = 67247,
                   c = 205
                },
                new AppSiteStatisticsGeoReport()
                {
                    cc = "AE",
                    d = "2013-01-18 00:00:00",
                    aid = "c7b7370d90a042dfb91e2a3a249eb3ec",
                    an = "AppSite_4",
                    i = 91138,
                    rv = 12.85699m,
                    r = 151702,
                    c = 400
                },
                new AppSiteStatisticsGeoReport()
                {
                    cc = "MA",
                    d = "2013-01-18 00:00:00",
                    aid = "98c46986970c4aa69fe981a8b44e944a",
                    an = "AppSite_5",
                    i = 2073,
                    rv = 0.92903m,
                    r = 15443,
                    c = 34
                },
                new AppSiteStatisticsGeoReport()
                {
                    cc = "OM",
                    d = "2013-01-18 00:00:00",
                    aid = "6b056c5ced744ce19886f93245feacf7",
                    an = "AppSite_6",
                    i = 28336,
                    rv = 3.87283m,
                    r = 115602,
                    c = 253
                },
                new AppSiteStatisticsGeoReport()
                {
                    cc = "IQ",
                    d = "2013-01-18 00:00:00",
                    aid = "83a99cd826704fc086c4c5f0908d8770",
                    an = "AppSite_7",
                    i = 26828,
                    rv = 1.93928m,
                    r = 250313,
                    c = 117
                },
                new AppSiteStatisticsGeoReport()
                {
                    cc = "JO",
                    d = "2013-01-18 00:00:00",
                    aid = "29bd1928a2e34d208b15e79b2f449c56",
                    an = "AppSite_8",
                    i = 20902,
                    rv = 1.68002m,
                    r = 218501,
                    c = 61
                },
                new AppSiteStatisticsGeoReport()
                {
                    cc = "KW",
                    d = "2013-01-18 00:00:00",
                    aid = "a38778e8e33942f5972067dcdddf737d",
                    an = "AppSite_9",
                    i = 80711,
                    rv = 4.53896m,
                    r = 660967,
                    c = 153
                },
                new AppSiteStatisticsGeoReport()
                {
                    cc = "OM",
                    d = "2013-01-18 00:00:00",
                    aid = "c8ceca51c9b14a1cbf9f4c870da403c8",
                    an = "AppSite_10",
                    i = 27656,
                    rv = 1.54271m,
                    r = 133097,
                    c = 95
                }
            };
        }
    }
}
