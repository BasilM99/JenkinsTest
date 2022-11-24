﻿using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArabyAds.Framework;
namespace ArabyAds.AdFalcon.Domain.Model.Core
{

    public class Segment : ManagedLookupBase
    {
        private IAudienceSegmentOccupationRepository _AudienceSegmentOccupationRepository = IoC.Instance.Resolve<IAudienceSegmentOccupationRepository>();
        static readonly object LockObj = new object();
        public virtual decimal Price
        {
            get;
            set;
        }

        public virtual int SegmentType
        {
            get;
            set;
        }

        public virtual BusinessPartner Provider
        {
            get;
            set;
        }
        public virtual Segment Parent
        {
            get;
            set;
        }

        public virtual string Description
        {
            get;
            set;
        }
        public virtual bool IsPermissionNeed
        {
            get;
            set;
        }
        public virtual int Code
        {
            get;
            set;
        }
        public virtual string OperatorSegmentCode
        {
            get;
            set;
        }
        public virtual bool Selectable
        {
            get;
            set;
        }
        public virtual bool Activated
        {
            get;
            set;
        }
        public virtual Account.Account Account { get; set; }
        public virtual Account.User User { get; set; }
        public virtual AdvertiserAccount Advertiser { get; set; }
        public virtual CostModel CostModel { get; set; }
        public virtual int IntegrationId { get; set; }
        public virtual int? BinIndex
        {
            get;
            set;
        }

        public virtual int CalculateBinIndex(int Id)
        {
            lock (LockObj)
            {
                //100 should be configured
                var result = (Id % 100);
                var count = 1;
                var audienceSegmentOcuupobj = _AudienceSegmentOccupationRepository.Query(M => M.BinIndex == (result + 1)).SingleOrDefault();
                var bindIndex = result;
                //10000 should be configured

                while (count <= 100)
                {
                    if (bindIndex + 1 == 101)
                        bindIndex = 1;
                    else
                        bindIndex = bindIndex + 1;
                    audienceSegmentOcuupobj = _AudienceSegmentOccupationRepository.Query(M => M.BinIndex == bindIndex).SingleOrDefault();
                    if (audienceSegmentOcuupobj == null)
                    {
                        audienceSegmentOcuupobj = new AudienceSegmentOccupation();
                        audienceSegmentOcuupobj.BinIndex = bindIndex;
                        _AudienceSegmentOccupationRepository.Save(audienceSegmentOcuupobj);
                    }
                    if (!((audienceSegmentOcuupobj.NumberOfSegments + 1) > 10000))
                    {
                        audienceSegmentOcuupobj.NumberOfSegments = audienceSegmentOcuupobj.NumberOfSegments + 1;
                        _AudienceSegmentOccupationRepository.Save(audienceSegmentOcuupobj);
                        return bindIndex;
                    }
                    count = count + 1;
                }

                return -1;

            }
        }
    }
    public class AudienceSegment : Segment
    {
      
    }


    public class AudienceSegmentOccupation : IEntity<int>

    {
        public virtual int ID { get; set; }

        public virtual int BinIndex { get; set; }
        public virtual int NumberOfSegments { get; set; }
        public virtual bool IsDeleted { get; set; }

        public virtual string GetDescription()
        {
            throw new NotImplementedException();
        }
    }


    public class ContextualSegment: Segment
    {
        public virtual string Type
        {
            get;
            set;
        }

        public virtual string SubType
        {
            get;
            set;
        }


        public virtual string TargetingIntent
        {
            get;
            set;
        }

    }
    

}
