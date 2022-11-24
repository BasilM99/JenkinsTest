using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Noqoush.AdFalcon.WarmUp
{
    public static class Config
    {
        internal static string UserName
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["UserName"];
            }
        }
        internal static string Password
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["PassWord"]; ;
            }
        }
        internal static string CookieName
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["CookieName"]; ;
            }
        }
        internal static string Domain
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["Domain"]; ;
            }
        }
        //internal static string BaseURL
        //{
        //    get
        //    {
        //        return System.Configuration.ConfigurationManager.AppSettings["BaseURL"]; ;
        //    }
        //}
        internal static string LoginURL
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["LoginURL"]; ;
            }
        }
        internal static int TimeOut
        {
            get
            {
                return Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["TimeOut"]);
            }
        }
        internal static int TotalMinutes
        {
            get
            {
                return Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["TotalMinutes"]);
            }
        }
        internal static string UserAgent
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["UserAgent"];
            }
        }
    }
}
