using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.DAL.Data.Repositories.SettingRepository
{
    public interface ISettingRepository
    {
        Task<string> GetEmailDomainAsync();
        Task<string> GetEmailPatternAsync();
        Task<bool> SetEmailDomainAsync(string domain);
        Task<bool> SetEmailPatternAsync(string pattern);
    }
}
