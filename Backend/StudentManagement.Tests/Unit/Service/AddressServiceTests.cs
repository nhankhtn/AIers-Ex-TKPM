using Microsoft.Extensions.Configuration;
using Moq;
using StudentManagement.BLL.Services.AddressService;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace StudentManagement.Tests.Unit.Service
{
    public class AddressServiceTests
    {
        private readonly Mock<IAddressService> _addressServiceMock;

        public AddressServiceTests()
        {
            _addressServiceMock = new Mock<IAddressService>();
        }

        [Fact]
        public async Task GetProvincesAsync_ReturnsProvinces()
        {
            // Arrange  
            var mockResponse = "[\"Province1\", \"Province2\"]";
            _addressServiceMock.Setup(service => service.GetProvincesAsync())
                .ReturnsAsync(mockResponse);

            // Act  
            var result = await _addressServiceMock.Object.GetProvincesAsync();

            // Assert  
            Assert.NotNull(result);
            Assert.Contains("Province1", result);
        }

        [Fact]
        public async Task GetDistrictsAsync_ReturnsDistricts()
        {
            // Arrange  
            int provinceCode = 1;
            int depth = 2;
            var mockResponse = "[\"District1\", \"District2\"]";
            _addressServiceMock.Setup(service => service.GetDistrictsAsync(provinceCode, depth))
                .ReturnsAsync(mockResponse);

            // Act  
            var result = await _addressServiceMock.Object.GetDistrictsAsync(provinceCode, depth);

            // Assert  
            Assert.NotNull(result);
            Assert.Contains("District1", result);
        }

        [Fact]
        public async Task GetWardsAsync_ReturnsWards()
        {
            // Arrange  
            int districtCode = 1;
            int depth = 2;
            var mockResponse = "[\"Ward1\", \"Ward2\"]";
            _addressServiceMock.Setup(service => service.GetWardsAsync(districtCode, depth))
                .ReturnsAsync(mockResponse);

            // Act  
            var result = await _addressServiceMock.Object.GetWardsAsync(districtCode, depth);

            // Assert  
            Assert.NotNull(result);
            Assert.Contains("Ward1", result);
        }

        [Fact]
        public async Task GetCountriesAsync_ReturnsCountries()
        {
            // Arrange  
            var mockResponse = "[\"Country1\", \"Country2\"]";
            _addressServiceMock.Setup(service => service.GetCountriesAsync())
                .ReturnsAsync(mockResponse);

            // Act  
            var result = await _addressServiceMock.Object.GetCountriesAsync();

            // Assert  
            Assert.NotNull(result);
            Assert.Contains("Country1", result);
        }
    }

   
}