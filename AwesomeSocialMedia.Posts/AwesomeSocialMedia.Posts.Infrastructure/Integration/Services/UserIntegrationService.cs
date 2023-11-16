using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AwesomeSocialMedia.Posts.Infrastructure.Integration.Models;
using Consul;
using Newtonsoft.Json;

namespace AwesomeSocialMedia.Posts.Infrastructure.Integration.Services
{
    public class UserIntegrationService : IUserIntegrationService
    {
        private readonly HttpClient _httpClient;
        private readonly IConsulClient _consulClient;

        public UserIntegrationService(HttpClient httpClient, IConsulClient consulClient)
        {
            _httpClient = httpClient;
            _consulClient = consulClient;
        }

        public async Task<BaseResult<GetUserByIdViewModel>> GetById(Guid id) 
        {
            var queryResult = await _consulClient.Agent.Services();

            var keyValuePair = queryResult.Response.FirstOrDefault(r => r.Value.Service == "Users");
            var agentService = keyValuePair.Value;

            var fullUrl = $"http://{agentService.Address}:{agentService.Port}/api/users/{id}";

            var response = await _httpClient.GetAsync(fullUrl);

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<BaseResult<GetUserByIdViewModel>>(result);
        }   
    }
}