using System;
using System.Collections.Generic;
using ArabyAds.AdFalcon.Domain.Model.Core;
using System.Linq;
using ArabyAds.Framework;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;

namespace ArabyAds.AdFalcon.Domain.Model.Campaign
{
    public class AdTrackerCreative : AdCreative
    {
        public AdTrackerCreative()
        {
            TypeId = AdTypeIds.TrackingAd;

        }
        public virtual AppMarketingPartner AppMarketingPartner { get; set; }

        public virtual string ClickTrackerUrl { get; set; }

        public virtual Platform Platform { get; set; }
        public override void Approve()
        {
            base.Approve();
            var ftpBaseDirectory = Configuration.FtpBaseDirectory;
            var cdnBaseUrl = Configuration.CdnBaseUrl;
            if (AdCreativeUnits != null)
            {
                if (AdCreativeUnits.Any(M => M.Document != null))
                {

                    ftpUpload(ftpBaseDirectory, cdnBaseUrl);

                }
            }
        }
        public virtual void ftpUpload(string baseDirectory, string cdnBaseUrl)
        {
            string temp = string.Empty;
            var subFolder = string.Empty;
            if (!string.IsNullOrWhiteSpace(Group.Campaign.FolderURL))
            {
                subFolder = Group.Campaign.FolderURL;
                ApplicationContext.Instance.Logger.Info(string.Format("Campaign Folder Already Exists - AdId: {0}, CampaginID: {1}, Campaign Folder: {2}", this.ID, Group.Campaign.ID, Group.Campaign.FolderURL));
            }
            else
            {
                ApplicationContext.Instance.Logger.Info(string.Format("Campaign Folder Not Exist - AdId: {0}, CampaginID: {1}", this.ID, Group.Campaign.ID));

                //we need to create folder for it
                subFolder = Framework.Utilities.Environment.GetServerTime().ToString("yyyyMMdd");
                temp = string.Format("{0}/{1}", baseDirectory, subFolder);
                //create folder fo the current date
                Framework.Utilities.CDNHelper.CreateDirectory(temp);
                //create folder for current Campaign
                var isFolderCreated = false;
                while (!isFolderCreated)
                {
                    temp = string.Format("{0}/{1}", baseDirectory, subFolder);
                    var r = RandomNumber(1, 100000);
                    temp = string.Format("{0}/{1}", temp, r);
                    if (!Framework.Utilities.CDNHelper.DirectoryExists(temp))
                    {
                        Framework.Utilities.CDNHelper.CreateDirectory(temp);
                        isFolderCreated = true;
                        subFolder += "/" + r;

                        ApplicationContext.Instance.Logger.Info(string.Format("Campaign Folder Created - AdId: {0}, CampaginID: {1}, Campaign Folder: {2}", this.ID, Group.Campaign.ID, subFolder));
                        Group.Campaign.FolderURL = subFolder;
                    }
                }
            }

            // create folder for current ad if not found
            subFolder = string.Format("{0}/{1}", subFolder, uId);
            temp = string.Format("{0}/{1}", baseDirectory, subFolder);
            Framework.Utilities.CDNHelper.CreateDirectory(temp);

            var directory = string.Format("{0}/{1}", baseDirectory, subFolder);
            var cdnUrl = string.Format("{0}/{1}", cdnBaseUrl, subFolder);

            var duplicates = AdCreativeUnits.GroupBy(s => s.Document.Name.ToLower()).SelectMany(grp => grp.Skip(1));
            foreach (var adCreativeUnit in duplicates)
            {
                adCreativeUnit.Document.NewName = string.Format("{0}_{1}_{2}", adCreativeUnit.Document.GetNameWithNoExtension(),
                                                             adCreativeUnit.CreativeUnit.Width,
                                                             adCreativeUnit.CreativeUnit.Height);
            }
            foreach (var adCreativeUnit in AdCreativeUnits)
            {
                if (string.IsNullOrWhiteSpace(adCreativeUnit.Content))
                {
                    string url = adCreativeUnit.Document.ftpUpload(directory, cdnUrl);
                    adCreativeUnit.Content = url;
                }

                if (!adCreativeUnit.KeepShapshot)
                {
                    adCreativeUnit.SnapshotUrl = adCreativeUnit.Content;
                    adCreativeUnit.KeepShapshot = true;
                }
            }
        }
        public virtual void SetAllBannersUnused()
        {
            foreach (var adCreativeUnit in AdCreativeUnits)
            {
                adCreativeUnit.Document.UpdateUsage(isRemove: true);
            }
        }
        public override void ClearUnusedBanners()
        {
            var unusedItems = AdCreativeUnits.Where(item => item.Document.Usage < 1).ToList();
            foreach (var unusedItem in unusedItems)
            {
                //AdCreativeUnits.Remove(unusedItem);
                unusedItem.Delete();
            }
        }
        public override void AddCreativeUnit(AdCreativeUnit adCreativeUnit)
        {
           // adCreativeUnit.ImageType = adCreativeUnit.Document != null ? adCreativeUnit.Document.Extension.Trim('.') : null;
            base.AddCreativeUnit(adCreativeUnit);
        }



        public override AdCreative Clone(AdGroup adGroup)
        {
            var cloneObj = base.Clone<AdTrackerCreative>();

            cloneObj.ClickTrackerUrl = this.ClickTrackerUrl;

            cloneObj.AppMarketingPartner = this.AppMarketingPartner;
            cloneObj.Platform = this.Platform;

            foreach (var adCreativeUnit in AdCreativeUnits)
            {
                var adCreativeUnitClone = adCreativeUnit.Copy(cloneObj);
                if (adCreativeUnitClone.Document != null)
                {
                    adCreativeUnitClone.Document.UpdateUsage();
                }

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