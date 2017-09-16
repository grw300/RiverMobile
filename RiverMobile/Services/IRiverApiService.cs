using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RiverMobile.Models;

namespace RiverMobile.Services
{
    public interface IRiverApiService
    {
        Task<IEnumerable<T>> GetRiverModelsAsync<T>(string queryParameters)
            where T : BaseIdentifiable, new();


        Task<T> GetRiverModelByIdAsync<T>(string id, string queryParameters)
            where T : BaseIdentifiable, new();


        Task<T> PostRiverModelAsync<T>(T model)
            where T : BaseIdentifiable;
    }
}
