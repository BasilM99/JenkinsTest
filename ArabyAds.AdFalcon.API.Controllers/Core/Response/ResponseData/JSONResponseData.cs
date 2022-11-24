using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.API.Controllers.Core.Response.ResponseData
{
    public class JSONResponseData<T> : IAPIResponseData<T>
    {
        public T Data
        {
            get;
            set;
        }

        public string ResponseHeader
        {
            get;
            private set;
        }

        public List<string> PropertiesToExeclude { get; set; }

        public JSONResponseData(T data)
        {
            Data = data;
            ResponseHeader = "application/json";
        }

        public JSONResponseData(T data,List<string> propertiesToExeclude)
        {
            Data = data;
            PropertiesToExeclude = propertiesToExeclude;
            ResponseHeader = "application/json";
        }

        public string GetReponseString()
        {
    
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                ContractResolver = new PropertiesResolver(PropertiesToExeclude)
            };

            // Adds root element to the json objects, "data" root element
            dynamic collectionWrapper = new
            {
                data = Data
            };

            return JsonConvert.SerializeObject(collectionWrapper,Formatting.None, settings);
        }
    }

    class PropertiesResolver : DefaultContractResolver
    {
        public List<string> PropertiesToExeclude { get; set; }

        public PropertiesResolver(List<string> propertiesToExeclude)
        {
            PropertiesToExeclude = propertiesToExeclude;
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> props = base.CreateProperties(type, memberSerialization);

            if (PropertiesToExeclude != null)
            {
                IList<JsonProperty> properties = props.Where(p=> !PropertiesToExeclude.Contains(p.PropertyName)).ToList();
                return properties;
            }

            return props.ToList();
        }
    }
}
