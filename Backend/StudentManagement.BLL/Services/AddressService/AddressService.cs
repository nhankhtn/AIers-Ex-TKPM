using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.Services.AddressService
{
    public class Country
    {
        public Name name { get; set; }
    }

    public class Name
    {
        public string common { get; set; }
    }
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

        public async Task<string> GetCountriesAsync()
        {
            try
            {
                string responseBody = await _httpClient.GetStringAsync(_configuration["AddressApi:CountriesUrl"]);

                List<Country> countries = System.Text.Json.JsonSerializer.Deserialize<List<Country>>(responseBody);
                var result = countries.Select(c => new { name = c.name.common });
                string json = JsonSerializer.Serialize(result);

                return json;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Failed to retrieve countries", ex);
            }
        }
    }
}