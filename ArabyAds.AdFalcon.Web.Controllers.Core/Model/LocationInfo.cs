using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Web.Controllers.Model
{
    public class LocationInfo
    {
        public LocationInfo() { }

        public LocationInfo(string country, string city, string countryCode_Alpha2)
        {
            Country = country;
            City = city;
            CountryCode_Alpha2 = countryCode_Alpha2;
        }

        public LocationInfo(string country, string city, string countryCode_Alpha2, string org, string isp)
        {
            Country = country;
            City = city;
            CountryCode_Alpha2 = countryCode_Alpha2;
            Organization = org;
            ISP = isp;
        }

        public string Country { get; set; }
        public string CountryCode_Alpha2 { get; set; }

        public string RegionCode_FIPS { get; set; }
        public string RegionCode_ISO { get; set; }

        public string City { get; set; }

        public string Organization { get; set; }
        public string ISP { get; set; }
    }
}
