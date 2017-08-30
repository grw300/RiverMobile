using JsonApiSerializer;
using Newtonsoft.Json;
using RiverMobile.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using JsonApiSerializer.JsonConverters;
using JsonApiSerializer.ContractResolvers;
using JsonApiSerializer.Util.JsonApiConverter.Util;
using Newtonsoft.Json.Serialization;
using Xamarin.Forms;

namespace RiverMobile.Services
{
    public class RiverAPIService : IRiverAPIService
    {
        static readonly HttpClient httpClient = new HttpClient
        {
            BaseAddress = new Uri(Application.Current.Properties["RiverServiceBaseAddress"] as string)
        };
        static readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
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
            var jsonContent = JsonConvert.SerializeObject(model, new JsonApiSerializerSettings());
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
                                               JsonConvert.DeserializeObject<T>(content, new JsonApiSerializerSettings())
            );
        }
    }
}
