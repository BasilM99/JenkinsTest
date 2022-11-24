

using Microsoft.AspNetCore.Mvc.DataAnnotations;

using Microsoft.Extensions.Localization;
using ArabyAds.Framework.Web.ClientValidation;
using System.ComponentModel.DataAnnotations;
namespace ArabyAds.AdFalcon.Web.Controllers.Core
{
    public class CustomValidationAttributeAdapterProvider
    : IValidationAttributeAdapterProvider
    {
        private readonly IValidationAttributeAdapterProvider _baseProvider = new ValidationAttributeAdapterProvider();
        public CustomValidationAttributeAdapterProvider() { }

        IAttributeAdapter IValidationAttributeAdapterProvider.GetAttributeAdapter(
            ValidationAttribute attribute,
            IStringLocalizer stringLocalizer)
        {
            IAttributeAdapter adapter = null;
            if (attribute is ArabyAds.Framework.DataAnnotations.RequiredAttribute)
            {
                adapter = new RequiredAttributeAdapter((RequiredAttribute)attribute, stringLocalizer);
            }
            else if (attribute is Framework.DataAnnotations.RangeAttribute)
            {
                attribute.ErrorMessage = "Invalid Email Address.";

                adapter = new RangeAttributeAdapter((Framework.DataAnnotations.RangeAttribute)attribute, stringLocalizer);
            }
            else if (attribute is Framework.DataAnnotations.RegularExpressionAttribute || attribute is Framework.DataAnnotations.EmailAttribute)
            {
                //attribute.ErrorMessageResourceName = "InvalidCompare";
                //attribute.ErrorMessageResourceType = typeof(Resources.ValidationMessages);
                //var theNewattribute = attribute as CompareAttribute;
                //adapter = new RegularExpressionAttributeAdapter(theNewattribute, stringLocalizer);
                adapter = new RegularExpressionAttributeAdapter((Framework.DataAnnotations.RegularExpressionAttribute)attribute, stringLocalizer);

            }
            else if (attribute is Framework.DataAnnotations.StringLengthAttribute)
            {
                // attribute.ErrorMessageResourceName = "InvalidCompare";
                //attribute.ErrorMessageResourceType = typeof(Resources.ValidationMessages);
                //var theNewattribute = attribute as CompareAttribute;
                //adapter = new CompareAttributeAdapter(theNewattribute, stringLocalizer);
                adapter =new StringLengthAttributeAdapter((Framework.DataAnnotations.StringLengthAttribute)attribute, stringLocalizer);

            }
            else if (attribute is Framework.DataAnnotations.CompareAttribute)
            {
                // attribute.ErrorMessageResourceName = "InvalidCompare";
                // attribute.ErrorMessageResourceType = typeof(Resources.ValidationMessages);
                //var theNewattribute = attribute as CompareAttribute;
                //adapter = new CompareAttributeAdapter(theNewattribute, stringLocalizer);
     
                    adapter = new CompareAttributeAdapter((Framework.DataAnnotations.CompareAttribute)attribute, stringLocalizer);
            }
            //else if (attribute is Framework.DataAnnotations.RemoteAttribute)
            //{

            //    adapter = new RemoteAttributeAdapter((Framework.DataAnnotations.RemoteAttribute)attribute, stringLocalizer);
            //    // adapter = new ArabyAds.Framework.Web.ClientValidation.RemoteAttributeAdapter((ArabyAds.Framework.DataAnnotations.RemoteAttribute)attribute, stringLocalizer);

            //}
            return adapter;
        }
    }

}