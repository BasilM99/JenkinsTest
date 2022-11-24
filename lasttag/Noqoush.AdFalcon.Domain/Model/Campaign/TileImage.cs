﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Noqoush.AdFalcon.Domain.Model.Core;

namespace Noqoush.AdFalcon.Domain.Model.Campaign
{
    public class TileImage : LookupBase<TileImage, int>
    {
        public virtual bool IsCustom { get; set; }
        public virtual bool IsClickAction { get; set; }
        public virtual IList<TileImageDocument> Images { get; set; }
        public virtual void ftpUpload(string directory, string cdnUrl)
        {
            if (IsCustom)
            {
                foreach (var tileImage in Images)
                {
                    if (string.IsNullOrWhiteSpace(tileImage.URL))
                    {
                        tileImage.URL = tileImage.Document.ftpUpload(directory, cdnUrl);
                    }
                }
            }
        }
        public override string GetDescription()
        {
            if (Name == null)
                return Framework.Resources.ResourceManager.Instance.GetResource("Custom", "Global");
            else
            {
                return Name.ToString();
            }
        }
        public virtual TileImage Copy()
        {
            if (this.Name != null)
            {
                var dummyObj = this.Name.Values.Count;
            }
            if (this.IsCustom)
            {
                //clone it
                var cloneObj = new TileImage
                {
                    Name = this.Name,
                    IsCustom = this.IsCustom,
                    IsClickAction = this.IsClickAction,
                    IsDeleted = this.IsDeleted,
                    Images = new List<TileImageDocument>()
                };

                foreach (var tileImageDocument in Images)
                {
                    var tileImageDocumentClone = tileImageDocument.Copy();
                    tileImageDocumentClone.TileImage = cloneObj;
                    cloneObj.Images.Add(tileImageDocumentClone);
                }
                return cloneObj;
            }
            else
            {
                //return reference
                return this;
            }

        }
    }
}

