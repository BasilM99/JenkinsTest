using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Noqoush.AdFalcon.Web.Controllers.Model.User
{
    public class ImpersonateUserViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }

        public override string ToString()
        {
            return string.Format("{0}        {1}        {2}", Name, CompanyName,Email);
        }
    }

    public class ImpersonateViewModel
    {
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public long TotalCount { get; set; }
        public IList<ImpersonateUserViewModel> Users { get; set; }

    }

    public class ImpersonateSaveModel
    {
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public int? AccountId { get; set; }
        public string returnUrl { get; set; }
    }
}
