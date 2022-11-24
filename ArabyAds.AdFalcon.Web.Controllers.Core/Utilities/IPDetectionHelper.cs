using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Configuration;
using ArabyAds.Framework.ConfigurationSetting;
using MaxMind.Db;
using Newtonsoft.Json.Linq;
using System.Threading;
using ArabyAds.Framework;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Web.Controllers.Model;
using System.Net;
using MaxMind.GeoIP2;

namespace ArabyAds.AdFalcon.Web.Controllers.Utilities
{
    public class IPDetectionHelper
    {

        private const string MAXMIND_CITY_FILE_NAME = "GeoIP2-city.mmdb";
        private const string MAXMIND_ISP_FILE_NAME = "GeoIP2-ISP.mmdb";

        static IPDetectionHelper()
        {
            string maxmindDatabaseFilePath = Config.ConfigurationManager.GetConfigurationSetting(null,null, "PortalMaxMindDatabaseFilePath");
            
        }

        //public LocationInfo Detect(string ip)
        //{
        //    LocationInfo locationInfo = new LocationInfo();

        //    IPAddress ipAddress = null;
        //    if (IPAddress.TryParse(ip, out ipAddress))
        //    {
        //        JToken cityResponse = CityReader.Find<JToken>(ipAddress);
        //        if (cityResponse != null)
        //        {
        //            JToken country = cityResponse["country"];
        //            if (country != null)
        //            {
        //                locationInfo.Country = country["names"]["en"].ToString();
        //                JToken countryIsoCode = country["iso_code"];
        //                if (countryIsoCode != null)
        //                    locationInfo.CountryCode_Alpha2 = countryIsoCode.ToString();
        //            }

        //            JToken city = cityResponse["city"];
        //            if (city != null)
        //            {
        //                locationInfo.City = city["names"]["en"].ToString();
        //            }

        //            JToken regions = cityResponse["subdivisions"];
        //            if (regions != null)
        //            {
        //                JToken regionIsoCode = regions[0]["iso_code"];
        //                if (regionIsoCode != null)
        //                    locationInfo.RegionCode_ISO = regionIsoCode.ToString();
        //            }
        //        }

        //        JToken ispResponse = ISPReader.Find<JToken>(ipAddress,null);
        //        if (ispResponse != null)
        //        {
        //            JToken isp = ispResponse["isp"];
        //            if (isp != null)
        //            {
        //                locationInfo.ISP = isp.ToString();
        //            }

        //            JToken org = ispResponse["organization"];
        //            if (org != null)
        //            {
        //                locationInfo.Organization = org.ToString();
        //            }
        //        }
        //    }

        //    return locationInfo;
        //}
        public LocationInfo Detect(string ip)
        {
            LocationInfo locationInfo = new LocationInfo();

            if (IPAddress.TryParse(ip, out IPAddress ipAddress))
            {
          
                if (CityReader.TryCity(ipAddress, out var response))
                {
                    if (response.Country != null)
                    {
                        response.Country.Names.TryGetValue("en", out string countryName);
                        locationInfo.Country = countryName;
                        locationInfo.CountryCode_Alpha2 = response.Country.IsoCode;
                    }

                    if (response.City != null)
                    {
                        response.City.Names.TryGetValue("en", out string cityName);
                        locationInfo.City = cityName;
                    }

                 

                    if (response.Subdivisions?.Count > 0)
                        locationInfo.RegionCode_ISO = response.Subdivisions[0].IsoCode;
                }

               
            }

            return locationInfo;
        }
        private static bool cityFileIsChanging = false;
        private static long cityFileChangeEventLastTime = 0;
    
        private static bool ispFileIsChanging = false;
        private static long ispFileChangeEventLastTime = 0;

        private static void OnMaxmindCityFile_Changed(object o, FileSystemEventArgs e)
        {
            try
            {
                if (ArabyAds.Framework.Utilities.Environment.GetServerTime().Ticks > (cityFileChangeEventLastTime + (10 * 10000000)))
                    cityFileChangeEventLastTime = ArabyAds.Framework.Utilities.Environment.GetServerTime().Ticks;
                else
                    return;

                cityFileIsChanging = true;

                cityReader.Dispose();
                Thread.Sleep(2000);

                cityReader = new DatabaseReader(string.Concat(Config.ConfigurationManager.GetConfigurationSetting(null,null, "PortalMaxMindDatabaseFilePath"), MAXMIND_CITY_FILE_NAME), FileAccessMode.Memory);
                cityFileIsChanging = false;

                ApplicationContext.Instance.Logger.Warn("MaxMindLocationDetectionProvider "+ "OnMaxmindCityFile_Changed "+ "Portal detected that maxmind city data file has been changed");
            }
            catch (Exception ex)
            {
                cityFileIsChanging = false;
                ApplicationContext.Instance.Logger.Error("MaxMindLocationDetectionProvider "+"OnMaxmindCityFile_Changed "+ "an exception has occured while watching maxmind city data file changes ", ex);
            }
            //finally
            //{
            //    cityFileIsChanging = false;
            //}
        }
        private static void OnMaxmindCityFile_Error(object o, ErrorEventArgs e)
        {
            ApplicationContext.Instance.Logger.Error("MaxMindLocationDetectionProvider "+ "OnMaxmindCityFile_Error "+ "an exception has occured while watching maxmind city file changes ", e.GetException());
        }
        private static void OnMaxmindIspFile_Changed(object o, FileSystemEventArgs e)
        {
            try
            {
                if (ArabyAds.Framework.Utilities.Environment.GetServerTime().Ticks > (ispFileChangeEventLastTime + (10 * 10000000)))
                    ispFileChangeEventLastTime = ArabyAds.Framework.Utilities.Environment.GetServerTime().Ticks;
                else
                    return;

                ispFileIsChanging = true;

                ispReader.Dispose();
                Thread.Sleep(2000);

                ispReader = new DatabaseReader(string.Concat(Config.ConfigurationManager.GetConfigurationSetting(null,null, "PortalMaxMindDatabaseFilePath"), MAXMIND_ISP_FILE_NAME), FileAccessMode.Memory);
                ispFileIsChanging = false;

                ApplicationContext.Instance.Logger.Warn("MaxMindLocationDetectionProvider "+ "OnMaxmindIspFile_Changed "+"Portal detected that maxmind isp data file has been changed");
            }
            catch (Exception ex)
            {
                ispFileIsChanging = false;
                ApplicationContext.Instance.Logger.Error("MaxMindLocationDetectionProvider "+ "OnMaxmindIspFile_Changed "+ "an exception has occured while watching maxmind isp data file changes", ex);
            }
            //finally
            //{
            //    ispFileIsChanging = false;
            //}
        }
        private static void OnMaxmindIspFile_Error(object o, ErrorEventArgs e)
        {
            ApplicationContext.Instance.Logger.Error("MaxMindLocationDetectionProvider "+ "OnMaxmindIspFile_Error "+ "an exception has occured while watching maxmind isp file changes ", e.GetException());
        }
        private static readonly object citySyncObject = new object();
        private static DatabaseReader cityReader;
        private static DatabaseReader CityReader
        {
            get
            {
                while (cityFileIsChanging)
                    Thread.Sleep(1);

                if (cityReader == null)
                {
                    lock (citySyncObject)
                    {
                        if (cityReader == null)
                        {
                            cityReader = new DatabaseReader(string.Concat(Config.ConfigurationManager.GetConfigurationSetting(null, null, "PortalMaxMindDatabaseFilePath"), MAXMIND_CITY_FILE_NAME), FileAccessMode.Memory);
                        }
                    }
                }

                return cityReader;
            }
        }

        private static readonly object ispSyncObject = new object();
        private static DatabaseReader ispReader;
        private static DatabaseReader ISPReader
        {
            get
            {
                while (ispFileIsChanging)
                    Thread.Sleep(1);

                if (ispReader == null)
                {
                    lock (ispSyncObject)
                    {
                        if (ispReader == null)
                        {
                            string maxmindDatabasesFilePath = Config.ConfigurationManager.GetConfigurationSetting(null, null, "PortalMaxMindDatabaseFilePath");
                            ispReader = new DatabaseReader(string.Concat(maxmindDatabasesFilePath, MAXMIND_ISP_FILE_NAME), FileAccessMode.Memory);
                        }
                    }
                }
                return ispReader;
            }
        }
    }
}
