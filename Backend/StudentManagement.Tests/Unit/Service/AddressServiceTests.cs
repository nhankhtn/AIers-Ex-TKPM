using Microsoft.Extensions.Configuration;
using Moq;
using StudentManagement.BLL.Services.AddressService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Tests.Unit.Service
{
    public class AddressServiceTests
    {
        private readonly HttpClient _httpClient;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly IAddressService _addressService;

        public AddressServiceTests()
        {
            _httpClient = new HttpClient();
            _configurationMock = new Mock<IConfiguration>();
            _addressService = new AddressService(_httpClient, _configurationMock.Object);
        }

        [Fact]
        public async Task GetProvincesAsync_FetchedProvinces_ReturnsNotNull()
        {
            // Arrange
            _configurationMock.Setup(c => c["AddressApi:BaseUrl"]).Returns("https://provinces.open-api.vn/api");

            // Act
            var result = await _addressService.GetProvincesAsync();

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetDistrictsAsync_FetchedDistricts_ReturnsNotNull()
        {
            // Arrange
           
            int provinceCode = 1;
            int depth = 2;
            _configurationMock.Setup(c => c["AddressApi:BaseUrl"]).Returns("https://provinces.open-api.vn/api");

            // Act
            var result = await _addressService.GetDistrictsAsync(provinceCode, depth);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetWardsAsync_FetchedWards_ReturnsWards()
        {
            // Arrange
            
            int districtCode = 1;
            int depth = 2;
            _configurationMock.Setup(c => c["AddressApi:BaseUrl"]).Returns("http://wards.com");
           

            // Act
            var result = await _addressService.GetWardsAsync(districtCode, depth);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetCountriesAsync_FetchedCountries_GetDistrictsAsync_ReturnsNotNull()
        {
            // Arrange
           
            _configurationMock.Setup(c => c["AddressApi:CountriesUrl"]).Returns("https://restcountries.com/v3.1/all");
            

            // Act
            var result = await _addressService.GetCountriesAsync();

            // Assert
            Assert.NotNull(result);
        }
    }
}