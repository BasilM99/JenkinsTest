using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Noqoush.AdFalcon.Domain.Model.Campaign
{
    public class PlainHtmlCreative : AdCreative
    {
        public PlainHtmlCreative()
        {
            TypeId = AdTypeIds.PlainHTML;

        }
        public override void Approve()
        {
            base.Approve();
           
            base.UploadSnapshots();
        }

        public override AdCreative Clone(AdGroup adGroup)
        {
            var cloneObj = base.Clone<PlainHtmlCreative>();

            foreach (var adCreativeUnit in AdCreativeUnits)
            {
                var adCreativeUnitClone = adCreativeUnit.Copy(cloneObj);

                if (adCreativeUnitClone.SnapshotDocument != null)
                {
                    adCreativeUnitClone.SnapshotDocument.UpdateUsage();
                }

                cloneObj.AddCreativeUnit(adCreativeUnitClone);
            }
            return cloneObj;
        }

        public override AdCreative Clone()
        {
            return Clone(this.Group);
        }
    }
}