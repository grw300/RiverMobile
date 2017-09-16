using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using JsonApiSerializer;
using Newtonsoft.Json;
using RiverMobile.Helpers;
using RiverMobile.Models;

namespace RiverMobile.Services
{
    public class RiverApiService : IRiverApiService
    {
        static readonly HttpClient httpClient = new HttpClient
        {
            BaseAddress = new Uri(Settings.RiverApiBaseAddress)
        };

        static readonly JsonApiSerializerSettings jsonSerializerSettings = new JsonApiSerializerSettings
        {
            DefaultValueHandling = DefaultValueHandling.Ignore,
        };

        public async Task<IEnumerable<T>> GetRiverModelsAsync<T>(string queryParameters = null)
            where T : BaseIdentifiable, new()
        {
            var model = new T();
            var response = await httpClient.GetAsync(model.Type + queryParameters);

            return await HandleResponse<List<T>>(response);
        }

        public async Task<T> GetRiverModelByIdAsync<T>(string id, string queryParameters = null)
            where T : BaseIdentifiable, new()
        {
            var model = new T();
            var response = await httpClient.GetAsync($"{model.Type}/{id}" + queryParameters);

            return await HandleResponse<T>(response);
        }

        public async Task<T> PostRiverModelAsync<T>(T model)
            where T : BaseIdentifiable
        {
            var jsonContent = JsonConvert.SerializeObject(model, jsonSerializerSettings);
            var postContent = new StringContent(jsonContent);
            postContent.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.api+json");

            var response = await httpClient.PostAsync(model.Type, postContent);

            return await HandleResponse<T>(response);
        }

        async Task<T> HandleResponse<T>(HttpResponseMessage response)
        {
            //TODO: Design error scheme for when this fails
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            return await Task.Factory.StartNew(() =>
                                               JsonConvert.DeserializeObject<T>(content, jsonSerializerSettings)
            );
        }
    }
}
