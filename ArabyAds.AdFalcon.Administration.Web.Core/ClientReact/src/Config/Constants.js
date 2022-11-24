export default {
    backend: {
       // baseUrl: 'https://72.251.244.32:5559',
        baseUrl: 'https://localhost:44328',
        advertisorsListUrl: '/Dashboard/GetAdvList?',
        campainsListUrl: '/Dashboard/GetCampList?',
        ChartControlPostUrl: '/Dashboard/GChartControlPost',
        DashboardGridPostUrl: '/Filter/GetPaginationGrid',
        tokenUrl: '',
        refreshTokenUrl: '',
        getAdvList: "/Dashboard/GetAdvList",
        getCampList: "/Dashboard/GetCampList",
        getAppSiteList: "/Dashboard/GetAppSiteList",
        getDealList: "/Dashboard/GetDealList",
        getMetricsByType: "/Dashboard/GetMetricsByType",
        getAdGeoLocationGrid:"/dashboard/AdGeoLocation",
        getAdPerformanceGrid:"/dashboard/adPerformance",
        getReportCriteriaForDashboardList:"/dashboard/GetReportCriteriaForDashboardApi",
        GetResourcesURL:"/en/Common/ResourcesTest?langName={{lng}}&setName={{ns}}",
        getDashboardCardDataList:"/dashboard/GetDashboardCardData",
        getSideBarData:"/dashboard/GetSideBarData",
        getCountres:"/Country/GetCountries",
        getDimTree:"/Tree/Get?type=1&factId=1&IncludeId=false",
        getMeasureTree:"/Tree/Get?type=2&factId=1&IncludeId=false",
        getAdvertizersData:"/Campaign/_AccountAdvertisers",
        getAdvertizersDataandArchive:"/Campaign/AccountAdvertisers",
        GetReportCriteriaForDashboardById:"/dashboard/GetReportCriteriaForDashboardById",
        getCampaignsByAdvertiserAccountId:"/Campaign/_Index?AdvertiseraccId=",
        SaveAddAdvertiser:"/Campaign/SaveAdvertiserAccount",
        GetLookupAdvertiser:"/Advertiser/GetAdvertisers",
        getCampaignsData:"/Campaign/_Index",
        getCampaignsActions:"/Campaign/Index",
        getDealsData:"/Deals/_IndexPMPDeals",
        getExchangeList:"/Party/_IndexNoHttpsSelect2Object",
        getAdvertizersDataandArchive:"/Campaign/AccountAdvertisers",
        getDealsDataArchive:"/Deals/Index",
        getGroupsByCampaignId:"/Campaign/Groups/",
        getGroupData:"/Campaign/_Groups/",
        getAdByGroupId:"/Campaign/Ads/{0}/{1}",
        getAdByGroupIdActions:"/Campaign/Ads/",
        getAdsDataByGroupId:"/Campaign/_Ads/{0}/{1}",
        getAudienceListIdUrl:"/Campaign/_AudienceList/",
        SaveAudienceListUrl:"/Campaign/SaveAudienceList/",

        renameAdGroup:"/Campaign/RenameGroup",
        cloneAdGroup:"/Campaign/CopyAdGroup",
        cloneCampaign:"/Campaign/CopyCampaign",
        cloneAd:"/Campaign/CopyAd",
        GetTreeDataForCountry: "/Country/GetTreeData",
        GetDealData: "/Deals/GetDealData",
        CreateDeal: "/Deals/CreateDeal",
        DashboardCriteriaSaveUrl: '/Dashboard/SaveReportCriteriaForDashboard',
        UploaddevicesIdUrl: "/Document/SaveForCSV?AudienceListId=",
        getContentListsDataUrl: "/Campaign/_MasterAppSites/",
        getContentListsActionsUrl: "/Campaign/MasterAppSites/",
        getContentListsDataUrl: "/Campaign/_MasterAppSites/",
        getContentListsActionsUrl: "/Campaign/MasterAppSites/",
        SaveAccountAdvertiserSettings: "/Campaign/SaveAccountAdvertiserSettings",
        GetAccountAdvertiserSettings: "/Campaign/GetAccountAdvertiserSettings",
        GetCampaign: "/Campaign/Get",
        SaveCampaign: "/Campaign/SaveCampaign",
    },
    keys: {
        cookies: {
            token: 'token',
            refreshToken: 'refresh_token',
            user: 'user'
        },
        localStorage: {
            uiLanguage: 'ui-lang',
            uiDirection: 'ui-dir',
            uiTheme: 'ui-theme',
            User: 'AdFalconUser',
          

        },
        headers:{
            json:"application/json",
            form:"application/x-www-form-urlencoded; charset=UTF-8"
        }
    },
    paging: {
        defaultPageSize: 10
    }

};