using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace StudentManagement.BLL.Services.AddressService
{
    //public class Country
    //{
    //    public string name { get; set; }
    //    public string common { get; set; }
    //}

    //public class Name
    //{
    //    public string common { get; set; }
    //}
    public class AddressService : IAddressService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public AddressService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> GetProvincesAsync()
        {
            try
            {
                string responseBody = await _httpClient.GetStringAsync(_configuration["AddressApi:BaseUrl"] + "/p");
                return responseBody;
            }
            catch(HttpRequestException ex)
            {
                throw new Exception("Failed to retrieve countries", ex);
            }
        }

        public async Task<string> GetDistrictsAsync(int provinceCode, int depth)
        {
            try
            {
                string responseBody = await _httpClient.GetStringAsync(_configuration["AddressApi:BaseUrl"] + $"/p/{provinceCode}?depth={depth}");
                return responseBody;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Failed to retrieve districts", ex);
            }
        }

        public async Task<string> GetWardsAsync(int districtCode, int depth)
        {
            try
            {
                string responseBody = await _httpClient.GetStringAsync(_configuration["AddressApi:BaseUrl"] + $"/d/{districtCode}?depth={depth}");
                return responseBody;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Failed to retrieve wards", ex);
            }
        }

        public async Task<List<object>> GetCountriesAsync()
        {
            try
            {
                string responseBody = await _httpClient.GetStringAsync(_configuration["AddressApi:CountriesUrl"]);

                using var doc = JsonDocument.Parse(responseBody);
                var root = doc.RootElement;

                var transformedList = new List<object>();

                foreach (var country in root.EnumerateArray())
                {
                    if (country.TryGetProperty("name", out JsonElement nameProperty) &&
                        nameProperty.TryGetProperty("common", out JsonElement commonName))
                    {
                        transformedList.Add(new { name = commonName.GetString() });
                    }
                }

                return transformedList;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Failed to retrieve countries", ex);
            }
        }
    }
}