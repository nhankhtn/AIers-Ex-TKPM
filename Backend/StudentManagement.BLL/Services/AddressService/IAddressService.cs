using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.Services.AddressService
{
    public interface IAddressService
    {
        public Task<string> GetProvincesAsync();
        public Task<string> GetDistrictsAsync(int provinceCode, int depth);
        public Task<string> GetWardsAsync(int districtCode, int depth);
        public Task<List<object>> GetCountriesAsync();
    }
}
