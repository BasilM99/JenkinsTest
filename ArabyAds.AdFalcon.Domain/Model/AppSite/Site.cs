using ArabyAds.Framework.DataAnnotations;
namespace ArabyAds.AdFalcon.Domain.Model.AppSite
{
    public class Site : AppSite
    {
        [Required]
        [StringLength(255)]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Url)]
        public virtual string SiteURL
        {
            get;
            set;
        }
        public override string GetURL()
        {
            return SiteURL;
        }

    }
}

