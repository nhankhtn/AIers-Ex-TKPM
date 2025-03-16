using StudentManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.DAL.Data.Repositories.FacultyRepo
{
    public class FacultyRepository : IFacultyRepository
    {
        public Task<IEnumerable<Faculty>> GetAllFacultiesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Faculty?> GetFacultyByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Faculty?> GetFacultyByNameAsync(string name)
        {
            throw new NotImplementedException();
        }
    }
}
