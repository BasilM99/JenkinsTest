using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Domain.Common.Model.Core
{
   
    //public class DeviceLanguage : LookupBase<DeviceLanguage, int>
    //{
    //    public virtual string Code
    //    {
    //        get;
    //        set;
    //    }
    //    public virtual string GetCultureName()
    //    {
    //        var result = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
    //        switch (Code)
    //        {
    //            case "ar":
    //                {
    //                    result = "ar-JO";
    //                    break;
    //                }
    //            case "en":
    //                {
    //                    result = "en-US";
    //                    break;
    //                }
    //        }
    //        return result;
    //    }
    //    public virtual CultureInfo GetCulture()
    //    {
    //        return new CultureInfo(GetCultureName());
    //    }

    //}

    /*
    public class ViewAbilityVendor : ManagedLookupBase
    {
        public virtual string Code
        {
            get;
            set;
        }

        public virtual string Description
        {
            get;
            set;
        }
        public virtual string GetCultureName()
        {
            var result = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
            switch (Code)
            {
                case "ar":
                    {
                        result = "ar-JO";
                        break;
                    }
                case "en":
                    {
                        result = "en-US";
                        break;
                    }
            }
            return result;
        }
        public virtual CultureInfo GetCulture()
        {
            return new CultureInfo(GetCultureName());
        }

    }
    */
}
