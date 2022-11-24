using System;
using System.Collections.Generic;
using System.Linq;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign.Creative;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign.Creative;
using ArabyAds.AdFalcon.Services.Mapping;
using ArabyAds.Framework;

namespace ArabyAds.AdFalcon.Services.Services.Campaign.Creative
{
    public class TileImageService : ITileImageService
    {
        private readonly ITileImageRepository _tileImageRepository = null;
        private readonly ITileImageSizeRepository _tileImageSizeRepository = null;
        private readonly IAdActionTypeRepository _adActionTypeRepository = null;
        public TileImageService(ITileImageRepository tileImageRepository,
                                ITileImageSizeRepository tileImageSizeRepository, IAdActionTypeRepository adActionTypeRepository)
        {
            this._tileImageRepository = tileImageRepository;
            this._tileImageSizeRepository = tileImageSizeRepository;
            this._adActionTypeRepository = adActionTypeRepository;
        }
        public IEnumerable<TileImageDto> GetAll()
        {
            var list = _tileImageRepository.Query(item=>item.IsCustom==false && item.IsClickAction==false);
            return list.Select(appSiteTypeDto => MapperHelper.Map<TileImageDto>(appSiteTypeDto)).ToList();
        }
        public IEnumerable<TileImageSizeDto> GetAllSizes()
        {
            var list = _tileImageSizeRepository.GetAll();
            return list.Select(appSiteTypeDto => MapperHelper.Map<TileImageSizeDto>(appSiteTypeDto)).ToList();
        }

        public TileImageDto GetAllByAdAction(ValueMessageWrapper<int> adActionId)
        {
            var item = _adActionTypeRepository.Get(adActionId.Value);
            return item != null && item.ActionImage != null ? MapperHelper.Map<TileImageDto>(item.ActionImage) : null;
        }


        public TileImageSizeDto GetSizeByParentId(ValueMessageWrapper<int> parentTileImageSizeId)
        {
            var tileImageSize = _tileImageSizeRepository.Query(p => p.TitleSize.ID == parentTileImageSizeId.Value).First();
            return MapperHelper.Map<TileImageSizeDto>(tileImageSize);
        }
    }
}
