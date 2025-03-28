using Microsoft.EntityFrameworkCore;
using StudentManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.DAL.Data.Repositories.SettingRepository
{
    public class SettingRepository: ISettingRepository
    {
        private readonly ApplicationDbContext _context;
        public SettingRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<string> GetEmailDomainAsync()
        {
            var maxId = await _context.Settings.MaxAsync(s => s.Id);
            var setting = await _context.Settings.FirstOrDefaultAsync(s => s.Id == maxId);
            return setting.EmailDomain;
        }
        public async Task<string> GetEmailPatternAsync()
        {
            var maxId = await _context.Settings.MaxAsync(s => s.Id);
            var setting = await _context.Settings.FirstOrDefaultAsync(s => s.Id == maxId);
            return setting.EmailPattern;
        }
        public async Task<bool> SetEmailDomainAsync(string domain)
        {
            var setting = new Setting
            {
                EmailDomain = domain
            };
            await _context.Settings.AddAsync(setting);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> SetEmailPatternAsync(string pattern)
        {
            var setting = new Setting
            {
                EmailPattern = pattern
            };
            await _context.Settings.AddAsync(setting);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
