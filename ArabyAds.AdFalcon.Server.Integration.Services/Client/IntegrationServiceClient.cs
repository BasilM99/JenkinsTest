using Newtonsoft.Json;
using ArabyAds.AdFalcon.Server.Integration.Services.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Server.Integration.Services.Client
{
    public class IntegrationServiceClient : IAudienceListService, IUserOptService
    {
        public readonly HttpClient _httpClient;
        private readonly string _isTrackingEnabledUri;
        private readonly string _updateTrackingUri;
        private readonly string _updateAudienceListUri;

        public IntegrationServiceClient(string host)
        {
            if (string.IsNullOrWhiteSpace(host))
                throw new ArgumentNullException("Agument host cannot be null or empty");
            _httpClient = new HttpClient() { BaseAddress = new Uri(host) };
            _isTrackingEnabledUri = _httpClient.BaseAddress.ToString() + ServicesRelativePathes.UserOpt.IsTrackingEnabled;
            _updateTrackingUri = _httpClient.BaseAddress.ToString() + ServicesRelativePathes.UserOpt.UpdateTracking;
            _updateAudienceListUri = _httpClient.BaseAddress.ToString() + ServicesRelativePathes.AudienceList.UpdateAudienceList;
        }
        public async Task<bool> IsTrackingEnabled(string userId)
        {
            var response = await GetAsync(string.Format(_isTrackingEnabledUri, userId));
            return bool.Parse(response);
        }

        public async Task UpdateAudienceList(UpdateAudienceListRequest request)
        {
            await PostAsync(_updateAudienceListUri, request);

        }

        public async Task<string> UpdateTracking(UpdateTrackingRequest request)
        {
            var response = await PostAsync(_updateTrackingUri, request);
            return response;
        }

        private async Task<string> PostAsync<TRequest>(string requestUrl, TRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
            {
                var httpResponse = await _httpClient.PostAsync(requestUrl, content);
                httpResponse.EnsureSuccessStatusCode();
                return await httpResponse.Content.ReadAsStringAsync();
            }
        }

        private async Task<string> GetAsync(string requestUrl)
        {
            var httpResponse = await _httpClient.GetAsync(requestUrl);
            httpResponse.EnsureSuccessStatusCode();
            return await httpResponse.Content.ReadAsStringAsync();
        }
    }
}
