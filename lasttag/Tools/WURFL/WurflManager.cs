using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using WURFL;
using WURFL.Config;

namespace WURFL.Web
{
    public static class WurflManager
    {
        public const String WurflManagerCacheKey = "__WurflManager";
        public static String WurflDataFilePath = System.Configuration.ConfigurationManager.AppSettings["WurflDataFilePath"];
        public static String WurflPatchFilePath = System.Configuration.ConfigurationManager.AppSettings["WurflPatchFilePath"];
        public static Dictionary<string, List<Device>> caps = new Dictionary<string, List<Device>>();

        public static void Initialize()
        {
            var configurer = new InMemoryConfigurer()
                    .MainFile(WurflDataFilePath)
                    .PatchFile(WurflPatchFilePath);

            _instance = WURFLManagerBuilder.Build(configurer);
            var all = _instance.GetAllDevices();

            foreach (var item in all)
            {
                var brand = item.GetCapability("brand_name").ToString();
                if (brand == "")
                    brand = "Unknown";
                var mark = item.GetCapability("marketing_name").ToString();
              
                List<Device> devices;

                if (!caps.TryGetValue(brand, out devices))
                {
                    devices = new List<Device>();
                    caps[brand] = devices;
                }

                devices.Add(new Device() { Name = item.FallbackId.ToString(), Id = item.Id, MarketingName = mark });

            }

        }

        private static readonly object _syncObject = new object();
        private static IWURFLManager _instance;
        public static IWURFLManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock(_syncObject)
                    {
                        if (_instance == null)
                        {
                            Initialize();
                            //HttpContext.Current.Cache[WurflManagerCacheKey] = manager;  
                        }
                    }
                }
                return _instance;
            }
        }
    }


    public class Device
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string MarketingName { get; set; }
    }
}