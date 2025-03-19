using Microsoft.AspNetCore.Mvc;
using Moq;
using StudentManagement.API.Controllers;
using StudentManagement.BLL.Services.AddressService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Tests.Unit.Controller
{
    public class AddressControllerTests
    {
        private readonly Mock<IAddressService> _mockIAddressService;
        private readonly AddressController _addressController;

        public AddressControllerTests()
        {
            _mockIAddressService = new Mock<IAddressService>();
            _addressController = new AddressController(_mockIAddressService.Object);
        }


        [Fact]
        public async Task GetProvinces_ExistProvinces_ReturnsProvinces()
        {
            // Arrange
            _mockIAddressService.Setup(x => x.GetProvincesAsync()).ReturnsAsync("Hanoi, Ho Chi Minh, Da Nang");
            // Act
            var result = await _addressController.GetProvinces();
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Hanoi, Ho Chi Minh, Da Nang", okResult.Value);
        }

        [Fact]
        public async Task GetDistricts_ExistDistricts_ReturnsDistricts()
        {
            // Arrange
            int provinceCode = 1;
            int depth = 2;
            _mockIAddressService.Setup(x => x.GetDistrictsAsync(provinceCode, depth)).ReturnsAsync("District 1, District 2, District 3");
            // Act
            var result = await _addressController.GetDistricts(provinceCode, depth);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("District 1, District 2, District 3", okResult.Value);
        }

        [Fact]
        public async Task GetWards_ExistWards_ReturnsWards()
        {
            // Arrange
            int districtCode = 1;
            int depth = 2;
            _mockIAddressService.Setup(x => x.GetWardsAsync(districtCode, depth)).ReturnsAsync("Ward 1, Ward 2, Ward 3");
            // Act
            var result = await _addressController.GetWards(districtCode, depth);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Ward 1, Ward 2, Ward 3", okResult.Value);
        }

        [Fact]
        public async Task GetCountries_ExistCountries_ReturnsCountries()
        {
            // Arrange
            _mockIAddressService.Setup(x => x.GetCountriesAsync()).ReturnsAsync("Vietnam, USA, Japan");
            // Act
            var result = await _addressController.GetCountries();
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Vietnam, USA, Japan", okResult.Value);
        }
    }
}
