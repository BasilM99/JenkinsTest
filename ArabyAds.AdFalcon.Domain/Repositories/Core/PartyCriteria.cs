using System;

using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.Framework.Persistence;
using System.Collections;
using System.Collections.Generic;

using System.Linq.Expressions;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;

namespace ArabyAds.AdFalcon.Domain.Repositories.Core
{
    public class PartyCriteria : CriteriaBase<Party>
    {
        public List<int> notInclud { get; set; }
        public PartyType? Type { get; set; }
        public string Name { get; set; }
        public int? Page { get; set; }
        public int Size { get; set; }
        public string Code { get; set; }
        public bool Visible { get; set; }
        public bool ShowArchive { get; set; }

        public void CopyFromCommonToDomain(ArabyAds.AdFalcon.Domain.Common.Repositories.Core.PartyCriteria Commoncr)
        {
            Visible = Commoncr.Visible;
            ShowArchive = Commoncr.ShowArchive;
            Name = Commoncr.Name;
            Code = Commoncr.Code;
            Size = Commoncr.Size;
            Page = Commoncr.Page;
            Type = Commoncr.Type;
            notInclud = Commoncr.notInclud;

        }
        public Expression<Func<T, bool>> GetExpression<T>() where T: Party
        {
            Expression<Func<T, bool>> filter = null;
            if (Type == null)
            {
                // filter = (c => (Name == null || c.Name.Contains(Name)));
                filter = (c => (c is BusinessPartner || c is Employee || c is SSPPartner || c is DSPPartner || c is DPPartner) && (Name == null || c.Name.Contains(Name)) && (c.IsDeleted == false || c.IsDeleted == ShowArchive));
            }
            else
            {
                switch (Type.Value)
                {
                    case PartyType.Account:
                        {
                            filter = (c => (Name == null || c.Name.Contains(Name)) && (c.IsDeleted == false || c.IsDeleted == ShowArchive));
                            break;
                        }
                    case PartyType.BusinessPartner:
                        {
                            filter = (c => (c is BusinessPartner || c is SSPPartner || c is DSPPartner || c is DPPartner) && ((string.IsNullOrEmpty(Code) || ((c as BusinessPartner).Type != null && (c as BusinessPartner).Type.Code == Code))) && (Name == null || c.Name.Contains(Name)) && (c.IsDeleted == false || c.IsDeleted == ShowArchive));
                            break;
                        }
                    case PartyType.Normal:
                        {
                            filter = (c => (c is BusinessPartner && !(c is SSPPartner || c is DSPPartner || c is DPPartner)) && ((string.IsNullOrEmpty(Code) || ((c as BusinessPartner).Type != null && (c as BusinessPartner).Type.Code == Code))) && (Name == null || c.Name.Contains(Name)) && (c.IsDeleted == false || c.IsDeleted == ShowArchive));
                            break;
                        }
                    case PartyType.SSP:
                        {
                            if (notInclud != null)
                                filter = (c => (Name == null || c.Name.Contains(Name)) && (c.IsDeleted == false || c.IsDeleted == ShowArchive) && (notInclud.Contains(c.ID)) && (c.Visible == Visible));

                            else
                                filter = (c => (Name == null || c.Name.Contains(Name)) && (c.IsDeleted == false || c.IsDeleted == ShowArchive) && (c.Visible == Visible));
                            break;
                        }
                    case PartyType.DSP:
                        {
                            filter = (c =>  (Name == null || c.Name.Contains(Name)) && (c.IsDeleted == false || c.IsDeleted == ShowArchive) && (c.Visible == Visible));
                            break;
                        }
                    case PartyType.DP:
                        {
                            filter = (c =>  (Name == null || c.Name.Contains(Name)) && (c.IsDeleted == false || c.IsDeleted == ShowArchive) && (c.Visible == Visible));
                            break;
                        }
                    case PartyType.Employee:
                        {
                            filter = (c =>  (Name == null || c.Name.Contains(Name)) && (c.IsDeleted == false || c.IsDeleted == ShowArchive));
                            break;
                        }
                    case PartyType.All:
                        {
                            filter = (c => (Name == null || c.Name.Contains(Name)) && (c.IsDeleted == false || c.IsDeleted == ShowArchive));
                            break;
                        }
                }
            }
            return filter;
        }
        public override Expression<Func<Party, bool>> GetExpression()
        {
            Expression<Func<Party, bool>> filter = null;
            if (Type == null)
            {
                // filter = (c => (Name == null || c.Name.Contains(Name)));
                filter = (c => (c is BusinessPartner || c is Employee || c is SSPPartner || c is DSPPartner || c is DPPartner) && (Name == null || c.Name.Contains(Name)) && (c.IsDeleted == false || c.IsDeleted== ShowArchive));
            }
            else
            {
                switch (Type.Value)
                {
                    case PartyType.Account:
                        {
                            filter = (c => c is Model.Account.Account && (Name == null || c.Name.Contains(Name)) && (c.IsDeleted == false || c.IsDeleted == ShowArchive));
                            break;
                        }
                    case PartyType.BusinessPartner:
                        {
                            filter = (c => (c is BusinessPartner || c is SSPPartner || c is DSPPartner || c is DPPartner) && ((string.IsNullOrEmpty(Code) || ((c as BusinessPartner).Type != null && (c as BusinessPartner).Type.Code == Code))) && (Name == null || c.Name.Contains(Name)) && (c.IsDeleted == false || c.IsDeleted == ShowArchive));
                            break;
                        }
                    case PartyType.Normal:
                        {
                            filter = (c => (c is BusinessPartner && !(c is SSPPartner || c is DSPPartner || c is DPPartner)) && ((string.IsNullOrEmpty(Code) || ((c as BusinessPartner).Type != null && (c as BusinessPartner).Type.Code == Code))) && (Name == null || c.Name.Contains(Name)) && (c.IsDeleted == false || c.IsDeleted == ShowArchive));
                            break;
                        }
                    case PartyType.SSP:
                        {
                            if (notInclud != null)
                                filter = (c => (c is SSPPartner) && (Name == null || c.Name.Contains(Name)) && (c.IsDeleted == false || c.IsDeleted == ShowArchive) && (notInclud.Contains(c.ID)) && (c.Visible == Visible));

                            else
                                filter = (c => (c is SSPPartner) && (Name == null || c.Name.Contains(Name)) && (c.IsDeleted == false || c.IsDeleted == ShowArchive) && (c.Visible == Visible));
                            break;
                        }
                    case PartyType.DSP:
                        {
                            filter = (c => (c is DSPPartner) && (Name == null || c.Name.Contains(Name)) && (c.IsDeleted == false || c.IsDeleted == ShowArchive) && (c.Visible == Visible));
                            break;
                        }
                    case PartyType.DP:
                        {
                            filter = (c => (c is DPPartner) && (Name == null || c.Name.Contains(Name)) && (c.IsDeleted == false || c.IsDeleted == ShowArchive) && (c.Visible == Visible));
                            break;
                        }
                    case PartyType.Employee:
                        {
                            filter = (c => c is Employee && (Name == null || c.Name.Contains(Name)) && (c.IsDeleted == false || c.IsDeleted == ShowArchive));
                            break;
                        }
                    case PartyType.All:
                        {
                            filter = (c => c is Party &&(Name == null || c.Name.Contains(Name)) && (c.IsDeleted == false || c.IsDeleted == ShowArchive));
                            break;
                        }
                }
            }
            return filter;
        }

        public override Func<Party, bool> GetWhere()
        {
            Func<Party, bool> filter = null;
            if (Type == null)
            {
                filter = (c => (Name == null || c.Name.Contains(Name)) && (c.IsDeleted == false || c.IsDeleted == ShowArchive));
            }
            else
            {
                switch (Type.Value)
                {
                    case PartyType.Account:
                        {
                            filter = (c => c is Model.Account.Account && (Name == null || c.Name.Contains(Name)) && (c.IsDeleted == false || c.IsDeleted == ShowArchive));
                            break;
                        }
                    case PartyType.BusinessPartner:
                        {
                            filter = (c => (c is BusinessPartner || c is SSPPartner || c is DSPPartner || c is DPPartner) && (Name == null || c.Name.Contains(Name)) && (c.IsDeleted == false || c.IsDeleted == ShowArchive));
                            break;
                        }

                    case PartyType.Normal:
                        {
                            filter = (c => (c is BusinessPartner && !(c is SSPPartner || c is DSPPartner || c is DPPartner)) && (Name == null || c.Name.Contains(Name)) && (c.IsDeleted == false || c.IsDeleted == ShowArchive));
                            break;
                        }
                    case PartyType.DSP:
                        {
                            filter = (c => (c is DSPPartner) && (Name == null || c.Name.Contains(Name)) && (c.IsDeleted == false || c.IsDeleted == ShowArchive) && (c.Visible == Visible));
                            break;
                        }
                    case PartyType.DP:
                        {
                            filter = (c => (c is DPPartner) && (Name == null || c.Name.Contains(Name)) && (c.IsDeleted == false || c.IsDeleted == ShowArchive) && (c.Visible == Visible));
                            break;
                        }
                    case PartyType.SSP:
                        {
                            if (notInclud != null)
                                filter = (c => (c is SSPPartner) && (Name == null || c.Name.Contains(Name)) && (c.IsDeleted == false || c.IsDeleted == ShowArchive) && (notInclud.Contains(c.ID)) && (c.Visible == Visible));
                            else
                                filter = (c => (c is SSPPartner) && (Name == null || c.Name.Contains(Name)) && (c.IsDeleted == false || c.IsDeleted == ShowArchive) && (c.Visible == Visible));
                            break;
                        }
                    case PartyType.Employee:
                        {
                            filter = (c => c is Employee && (Name == null || c.Name.Contains(Name)) && (c.IsDeleted == false || c.IsDeleted == ShowArchive));
                            break;
                        }
                }
            }
            return filter;
        }
    }
    public class DPPartnerCriteria : CriteriaBase<DPPartner>
    {
        public List<int> notInclud { get; set; }
        public PartyType? Type { get; set; }
        public string Name { get; set; }
        public int? Page { get; set; }
        public int Size { get; set; }
        public string Code { get; set; }
        public bool Visible { get; set; }


        public void CopyFromCommonToDomain(ArabyAds.AdFalcon.Domain.Common.Repositories.Core.DPPartnerCriteria Commoncr)
        {
            Visible = Commoncr.Visible;
            Name = Commoncr.Name;
            Code = Commoncr.Code;
            Size = Commoncr.Size; 
            Page = Commoncr.Page;
            Type = Commoncr.Type;
            notInclud = Commoncr.notInclud;

        }

        public override Expression<Func<DPPartner, bool>> GetExpression()
        {
            Expression<Func<DPPartner, bool>> filter = null;


            filter = (c => (Name == null || c.Name.Contains(Name)) && (c.IsDeleted == false ) && (c.Visible == Visible) && c.IsExternalProvider == true);




            return filter;
        }

        public override Func<DPPartner, bool> GetWhere()
        {
            Func<DPPartner, bool> filter = null;


            filter = (c => (Name == null || c.Name.Contains(Name)) && (c.IsDeleted == false ) && (c.Visible == Visible) && c.IsExternalProvider == true);



            return filter;
        }

    }

}
