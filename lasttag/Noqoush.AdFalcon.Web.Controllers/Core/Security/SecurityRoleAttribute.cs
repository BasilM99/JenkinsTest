using Noqoush.AdFalcon.Domain.Common.Model.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Noqoush.AdFalcon.Web.Controllers.Core.Security
{
    
    public abstract class SecurityRoleAttribute : ActionFilterAttribute
    {
        private string _roles;
        protected string[] _RolesSplit = new string[0];
        private string _authorizeroles;
        public AccountRole[] AccountRoles { get; set; }

        protected string[] _authorizeRolesSplit = new string[0];
        public string Roles
        {
            get
            {
                return _roles ?? String.Empty;
            }
            set
            {
                _roles = value;
                _RolesSplit = SplitString(value);
            }
        }
        public string AuthorizeRoles
        {
            get
            {
                return _authorizeroles ?? String.Empty;
            }
            set
            {
                _authorizeroles = value;
                _authorizeRolesSplit = SplitString(value);
            }
        }

        protected string[] SplitString(string original)
        {
            if (String.IsNullOrEmpty(original))
            {
                return new string[0];
            }

            var split = from piece in original.Split(',')
                        let trimmed = piece.Trim()
                        where !String.IsNullOrEmpty(trimmed)
                        select trimmed;
            return split.ToArray();
        }
    }
}
