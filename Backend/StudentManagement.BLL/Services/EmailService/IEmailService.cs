using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.Services.EmailService
{
    public interface IEmailService
    {
        Task<string> GetEmailDomainAsync();
        Task<bool> ValidateEmailAsync(string email);
        Task<bool> SetEmailDomainAsync(string domain);
    }
}
