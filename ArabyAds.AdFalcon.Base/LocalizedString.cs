using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ArabyAds.AdFalcon.Base
{
    public class LocalizedValue 
    {
        public virtual int ID { get; protected set; }
        public virtual string Culture { get; set; }
        public virtual string Value { get; set; }
        public virtual LocalizedString LocalizedString { get; set; }

    }

    public class LocalizedString
    {
        public virtual int ID { get; protected set; }

        public LocalizedString()
            : base()
        {
            Values = new List<LocalizedValue>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupKey">internal groupKey only</param>
        public LocalizedString(string groupKey)
            : this()
        {
            GroupKey = groupKey;
        }

        /// <summary>
        /// Localized String Id
        /// </summary>
        public virtual int LocalizedStringId { get; protected set; }
        /// <summary>
        /// GroupKey of the dictionary. 
        /// - only for internal use. Eases browsing data directly in database
        /// </summary>
        public virtual string GroupKey { get; set; }

        /// <summary>
        /// Stores all Values for a Culture Code
        /// </summary>
        public virtual IList<LocalizedValue> Values { get; protected set; }

        /// <summary>
        /// Adds a Localized Value for a given Culture Code
        /// - if the Culture Code exists then the LocalizedValue is updated
        /// - empty strings are considered as "not available". 
        ///     Thus adding an empty LocalizedValue might remove an existing if already existed
        /// </summary>
        /// <param name="CultureCode"></param>
        /// <param name="value"></param>
        /// <returns>Returns itself. Useful for method chaining.</returns>
        public virtual LocalizedValue SetValue(string value, string CultureCode)
        {
            if (ContainsKey(CultureCode))
            {
                if (string.IsNullOrEmpty(value))
                {
                    //TODO:OSAleh fix this
                    throw new NotImplementedException();
                }
                else
                {
                    //throw new NotImplementedException();
                    Values.First(localizedValue => localizedValue.Culture.Equals(CultureCode, StringComparison.OrdinalIgnoreCase))
                        .Value = value.Trim();
                }
            }
            else if (!string.IsNullOrEmpty(value))
            {
                Values.Add(getLocalizedValue(CultureCode, value));
            }
            return null;
        }
        protected virtual LocalizedValue getLocalizedValue(string CultureCode, string value)
        {
            return new LocalizedValue { Culture = CultureCode, Value = value, LocalizedString = this };
        }
        public virtual LocalizedValue SetValue(string value)
        {
            return SetValue(value, Thread.CurrentThread.CurrentUICulture.Name);
        }
        public virtual bool ContainsKey(string Key)
        {
            //TODO:Osaleh Check if the dic is faster
            return Values.Any(localizedValue => localizedValue.Culture.Equals(Key, StringComparison.OrdinalIgnoreCase));
        }
        public virtual string Get(string Key)
        {
            if (ContainsKey(Key))
                return Values.First(localizedValue => localizedValue.Culture.Equals(Key, StringComparison.OrdinalIgnoreCase)).Value;
            else
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// Gets a Localized Value string for a given Culture Code
        /// - returns empty string if it does not exist
        /// </summary>
        /// <param name="CultureCode"></param>
        /// <returns></returns>
        public virtual string GetValue(string CultureCode)
        {
            if (ContainsKey(CultureCode))
                return Get(CultureCode);
            return string.Empty;
        }

        public virtual string GetValue()
        {
            return GetValue(Thread.CurrentThread.CurrentUICulture.Name);
        }

        /// <summary>
        /// Gets the Value for the current UI Culture
        /// </summary>
        public virtual string Value
        {
            get
            {
                return GetValue();
            }
            set
            {
                SetValue(value);
            }
        }
        public override int GetHashCode()
        {
            return Values.GetHashCode();
        }
        public override string ToString()
        {
            return Value;
        }
    }
}
