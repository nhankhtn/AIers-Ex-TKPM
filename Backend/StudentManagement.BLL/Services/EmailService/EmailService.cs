using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StudentManagement.DAL.Data.Repositories.SettingRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StudentManagement.BLL.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly ISettingRepository _settingRepository;
        public EmailService(ISettingRepository settingRepository)
        {
            _settingRepository = settingRepository;
        }
        public async Task<string> GetEmailDomainAsync()
        {
            return await _settingRepository.GetEmailDomainAsync();
        }

        public async Task<bool> SetEmailDomainAsync(string domain)
        {
            try
            {
                if(!Regex.IsMatch(domain, @"^@[A-Za-z0-9.-]+(?<!-)\.[A-Za-z]{2,}$") || string.IsNullOrWhiteSpace(domain))
                {
                    throw new ArgumentException("Invalid domain.");
                }
                return await _settingRepository.SetEmailDomainAsync(domain);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> ValidateEmailAsync(string email)
        {
            string emailPattern = await _settingRepository.GetEmailPatternAsync();
            string emailDomain = await _settingRepository.GetEmailDomainAsync();
            if(!Regex.IsMatch(email, emailPattern))
            {
                return false;
            }
            string domain = "@" + email.Split('@')[1];
            if(domain != emailDomain)
            {
                return false;
            }
            return true;
        }
    }
}
