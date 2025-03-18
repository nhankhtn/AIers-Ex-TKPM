using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.BLL.Services.AddressService;

namespace StudentManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet("provinces")]
        public async Task<IActionResult> GetProvinces()
        {
            var provinces = await _addressService.GetProvincesAsync();
            return Ok(provinces);
        }

        [HttpGet("districts/{provinceCode:int}")]
        public async Task<IActionResult> GetDistricts([FromRoute]int provinceCode, [FromQuery] int depth = 2)
        {
            var districts = await _addressService.GetDistrictsAsync(provinceCode, depth);
            return Ok(districts);
        }

        [HttpGet("wards{districtCode}")]
        public async Task<IActionResult> GetWards([FromRoute] int districtCode, [FromQuery] int depth = 2)
        {
            var wards = await _addressService.GetWardsAsync(districtCode, depth);
            return Ok(wards);
        }

        [HttpGet("countries")]
        public async Task<IActionResult> GetCountries()
        {
            var countries = await _addressService.GetCountriesAsync();
            return Ok(countries);
        }
    }
}