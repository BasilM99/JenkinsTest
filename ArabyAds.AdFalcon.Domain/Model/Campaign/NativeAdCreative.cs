using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArabyAds.AdFalcon.Domain.Model.Campaign
{

    public class NativeAdCreative : AdCreative
    {
        public NativeAdCreative()
        {
            TypeId = AdTypeIds.NativeAd;

        }

        public virtual string Description { get; set; }
        public virtual string AppOpenUrl { get; set; }
        public virtual float? StarRating { get; set; }
        public virtual string ActionText { get; set; }
        public virtual bool ShowIfInstalled { get; set; }

        public virtual IList<NativeAdImage> Images { get; set; }
        public virtual IList<NativeAdIcon> Icons { get; set; }

        public override void Approve()
        {
            base.Approve();
            var ftpBaseDirectory = Configuration.FtpBaseDirectory;
            var cdnBaseUrl = Configuration.CdnBaseUrl;
            ftpUpload(ftpBaseDirectory, cdnBaseUrl);
            base.UploadSnapshots();
        }

        public override AdCreative Clone(AdGroup adGroup)
        {
            var cloneObj = base.Clone<NativeAdCreative>();

            cloneObj.Icons = new List<NativeAdIcon>();
            cloneObj.Images = new List<NativeAdImage>();
            cloneObj.Description = this.Description;
            cloneObj.StarRating = this.StarRating;
            cloneObj.ShowIfInstalled = this.ShowIfInstalled;
            cloneObj.ActionText = this.ActionText;
            cloneObj.AppOpenUrl = this.AppOpenUrl;

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

            foreach (var item in this.Icons)
            {
                var clonedIcon = item.Copy(cloneObj);
                if (clonedIcon.Document != null)
                {
                    clonedIcon.Document.UpdateUsage();
                }

                cloneObj.Icons.Add(clonedIcon);
            }

            foreach (var item in this.Images)
            {
                var clonedImage = item.Copy(cloneObj);
                if (clonedImage.Document != null)
                {
                    clonedImage.Document.UpdateUsage();
                }

                cloneObj.Images.Add(clonedImage);
            }


            return cloneObj;
        }

        public override AdCreative Clone()
        {
            return Clone(this.Group);
        }

        public virtual void ftpUpload(string baseDirectory, string cdnBaseUrl)
        {
            string temp = string.Empty;
            var subFolder = string.Empty;
            if (!string.IsNullOrWhiteSpace(Group.Campaign.FolderURL))
            {
                subFolder = Group.Campaign.FolderURL;
            }
            else
            {
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
                        Group.Campaign.FolderURL = subFolder;
                    }
                }
            }

            if (!Framework.Utilities.CDNHelper.DirectoryExists((baseDirectory + "/" + subFolder)))
            {
                ApplicationContext.Instance.Logger.Info(string.Format("SubFolder:{0}", (baseDirectory + "/" + subFolder)));
                Framework.Utilities.CDNHelper.CreateDirectory((baseDirectory + "/" + subFolder));
            }


            // create folder for current ad if not found
            subFolder = string.Format("{0}/{1}", subFolder, uId);
            temp = string.Format("{0}/{1}", baseDirectory, subFolder);
            Framework.Utilities.CDNHelper.CreateDirectory(temp);

            var directory = string.Format("{0}/{1}", baseDirectory, subFolder);
            var cdnUrl = string.Format("{0}/{1}", cdnBaseUrl, subFolder);

            var duplicateDocumentsNames = this.Images.Select(p => p as NativeAdCreativeBase).Union(this.Icons.Select(p => p as NativeAdCreativeBase)).Where(p => p.Document != null).GroupBy(p => p.Document.Name.ToLower()).SelectMany(p => p.Skip(1));
            foreach (var item in duplicateDocumentsNames)
            {
                item.Document.NewName = string.Format("{0}_{1}_{2}", item.Document.GetNameWithNoExtension(),
                                                            item.CreativeUnit.Width,
                                                            item.CreativeUnit.Height);
            }

            foreach (var item in this.Images)
            {
                if (item.Document != null)
                {
                    item.URL = item.Document.ftpUpload(directory, cdnUrl);
                }
            }

            foreach (var item in this.Icons)
            {
                if (item.Document != null)
                {
                    item.URL = item.Document.ftpUpload(directory, cdnUrl);
                }
            }
        }

    }
}
