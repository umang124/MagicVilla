using MagicVilla_Utility;
using MagicVilla_VillaAPI.DTOs;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services
{
    public class VillaNumberService : BaseService, IVillaNumberService
    {
        private string villaUrl;
        public VillaNumberService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
        {
            villaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI") ?? "https://localhost:7001";
        }

        public Task<T> CreateAsync<T>(VillaNumberDTO dto)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = ApiType.POST,
                Data = dto,
                Url = villaUrl + "/api/villanumber"
            });
        }

        public Task<T> DeleteAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = ApiType.DELETE,
                Url = villaUrl + "/api/villanumber/deletevillanumber/" + id
            });
        }

        public Task<T> GetAllAsync<T>()
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = ApiType.GET,
                Url = villaUrl + "/api/villanumber/"
            });
        }

        public Task<T> GetAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = ApiType.GET,
                Url = villaUrl + "/api/villanumber/getvillanumber/" + id
            });
        }

        public Task<T> UpdateAsync<T>(VillaNumberUpdateDTO dto)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = ApiType.PUT,
                Data = dto,
                Url = villaUrl + "/api/villanumber/updatevillanumber/" + dto.VillaNo
            });
        }
    }
}
