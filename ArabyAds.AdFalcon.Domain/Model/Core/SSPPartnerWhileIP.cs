using ArabyAds.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Model.Core
{
   

    public class SSPPartnerWhiteIP : IEntity<int>
    {
        public virtual byte[] IP { get; set; }
        public virtual SSPPartner SSPPartner { get; set; }

        public virtual int ID { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual string Description { get; set; }
        public virtual string GetDescription()
        {

            return string.Format("{0}-{1}", GetString(IP), SSPPartner.Name);
        }
        public virtual string GetString(byte[] bytes)
        {
            StringBuilder value = new StringBuilder();

            if (bytes != null)
            {

                for (int ctr = 0; ctr < bytes.Length; ctr++)
                {
                    value.Append(Convert.ToInt32(bytes[ctr]).ToString());

                    if (ctr != bytes.Length - 1) value.Append(".");

                }
            }
            return value.ToString();
        }
        public virtual string GetIPString(string bytesString)
        {
            StringBuilder value = new StringBuilder();
            if (!string.IsNullOrEmpty(bytesString))
            {
                byte[] bytes = Convert.FromBase64String(bytesString);
                if (bytes != null)
                {

                    for (int ctr = 0; ctr < bytes.Length; ctr++)
                    {
                        value.Append(Convert.ToInt32(bytes[ctr]).ToString());

                        if (ctr != bytes.Length - 1) value.Append(".");

                    }
                }
            }
            return value.ToString();
        }


      


    }

}
