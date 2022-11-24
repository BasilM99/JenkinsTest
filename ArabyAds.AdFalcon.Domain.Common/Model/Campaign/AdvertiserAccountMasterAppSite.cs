using ArabyAds.AdFalcon.Domain.Common.Model.Account;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Common.Model.Campaign
{
    [DataContract(Name = "MasterAppSiteStatus")]
    public enum MasterAppSiteStatus
    {
        [EnumMember]
        [EnumText("Undefined", "AccountDSPRequest")]
        None = 0,
        [EnumMember]
        [EnumText("InActiveAdvertisers", "Global")]
        InActive = 2,

        [EnumMember]
        [EnumText("Active", "JobGrid")]
        Active = 1
    }
    [DataContract(Name = "MasterAppSiteItemType")]
    public enum MasterAppSiteItemType
    {
        [EnumMember]
        [EnumText("Undefined", "AccountDSPRequest")]
        None =0,
        [EnumMember]
        [EnumText("SSPSites", "SiteMapLocalizations")]
        Site = 2,
        [EnumMember]
        [EnumText("AppEnvironmentType", "Campaign")]
        App = 1
    }
    [DataContract(Name = "MasterAppSiteType")]
    public enum MasterAppSiteType
    {
        [EnumMember]
        [EnumText("Undefined", "AccountDSPRequest")]
        None = 0,
        [EnumMember]
        [EnumText("WhiteList", "Global")]
        WhiteList = 1,
        [EnumMember]
        [EnumText("BlockList", "Global")]
        BlockList = 2
    }
   /* public class AdvertiserAccountMasterAppSite : IEntity<int>
    {
        public virtual int ID { get; set; }
        //public virtual string Name { get; set; }
        //public virtual int Status { get; set; }

        public virtual MasterAppSiteStatus Status { set; get; }

        public virtual string Name { get; set; }
        public virtual bool IsDeleted { get; set; }

        public virtual User User { get; set; }
        public virtual MasterAppSiteType Type { get; set; }
        public virtual Account.Account Account { get; set; }
        public virtual AdvertiserAccount Link { get; set; }
        public virtual IList<AdvertiserAccountMasterAppSiteItem> Items { get; set; }
        public virtual bool GlobalScope { get; set; }


        public virtual DateTime LastModifiedDate{get;set;}
        public virtual string GetDescription()
        {
            return string.Empty;
        }

        public virtual void Delete()
        {
            this.IsDeleted = true;
        }

        public virtual void Activate()
        {
            this.Status = MasterAppSiteStatus.Active;
        }
        public virtual void DeActivate()
        {
            this.Status = MasterAppSiteStatus.InActive;
        }
    }

    public class AdvertiserAccountMasterAppSiteItem : IEntity<int>
    {
        private string MD5Value;
        public virtual int ID { get; protected set; }
        //public virtual string Name { get; set; }
        public virtual string AppSiteID { get; set; }
        public virtual MasterAppSiteItemType  Type { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual string AppSiteName { get; set; }
        public virtual string BundleID { get; set; }
        public virtual string Domain { get; set; }
        //public virtual Account.Account Account { get; set; }
        public virtual AdvertiserAccountMasterAppSite Link { get; set; }
        public virtual User User { get; set; }
        public virtual Account.Account Account { get; set; }
        public virtual string GetDescription()
        {
            return string.Empty;
        }
        public virtual string Code {
            set { MD5Value = value; }

            get {
                if (!string.IsNullOrEmpty(BundleID))
                {
                    MD5Value = "app:" + BundleID;
                }
                else if (!string.IsNullOrEmpty(Domain))
                {
                    MD5Value = "site:"+Domain;
                }
                else
                {
                    MD5Value = string.Empty;
                        }
                return MD5Value;
            }
        }
        public virtual void Delete()
        {
            this.IsDeleted = true;
        }

        private  string MD5Encryption(string originalText)
        {
            var enc = MD5.Create();
            byte[] rescBytes = Encoding.ASCII.GetBytes(originalText);
            byte[] hashBytes = enc.ComputeHash(rescBytes);

            var str = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                str.Append(hashBytes[i].ToString("X2"));
            }

            return str.ToString();
        }
    }*/
}
