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
    public class TileImageDocument : IEntity<int>
    {
        public virtual int ID { get; protected set; }
        public virtual string URL { get; set; }
        public virtual Document Document { get; set; }
        public virtual TileImage TileImage { get; set; }
        public virtual TileImageSize TileImageSize { get; set; }
        public virtual bool IsDeleted { get; set; }

        public virtual string GetDescription()
        {
            return TileImageSize.GetDescription();
        }

        public virtual TileImageDocument Copy()
        {
            //dummy call
            var dummyId= this.TileImage.ID;

            var cloneObj = new TileImageDocument
                               {
                                   //URL = this.URL,
                                   IsDeleted = this.IsDeleted,
                                   Document = this.Document,
                                   //TileImage = this.TileImage,
                                   TileImageSize = this.TileImageSize
                               };
            return cloneObj;
        }
    }
}

