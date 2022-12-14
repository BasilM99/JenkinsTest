using System;
using System.Collections.Generic;
using Noqoush.Framework.DomainServices;
using Noqoush.AdFalcon.Domain.Model.Core;
using System.Collections.Specialized;
using System.Xml.Linq;

namespace Noqoush.AdFalcon.Domain.Model.Account
{
    [Serializable]
    public partial class AccountFundPgw : LookupBase<AccountFundPgw,int>
    {
        Dictionary<string,string> _settings = null;

        public virtual string Code { get; set; }

        public virtual string ApiResolver
        {
            get;
            set;
        }

        public virtual string OutLetId
        {
            get;
            set;
        }
        public virtual string ApiRef
        {
            get;
            set;
        }

        public virtual string Realm
        {
            get;
            set;
        }
        public virtual string IntegrationPageUrl
        {
            get;
            set;
        }
      
        public virtual string ReturnPageUrl
        {
            get;
            set;
        }
        public virtual string ConfigData
        {
            get;
            set;
        }

        public virtual Dictionary<string,string> Settings
        {
            get 
            {
                if (_settings == null && !string.IsNullOrEmpty(ConfigData) && Code.ToLower() != "migs")
                {
                    var decryptedConfigData = Noqoush.Framework.Utilities.Cryptography.Decrypt(ConfigData, true);
                    _settings = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
                    if (!string.IsNullOrEmpty(decryptedConfigData))
                    {
                        XElement dt = XElement.Parse(decryptedConfigData);
                        foreach (var c in dt.Elements())
                            _settings.Add(c.Attribute("key").Value, c.Attribute("value").Value);
                    }
                }
                else
                {
                 string[] vlauessp=   ConfigData.Split(',');

                    this.ApiRef= vlauessp[0];
                    this.OutLetId = vlauessp[1];
                    this.Realm = vlauessp[2];
                    _settings = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
                }
                return _settings;
            }
        }
    }
}