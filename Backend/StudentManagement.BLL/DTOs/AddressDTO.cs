using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Application.DTOs
{
    public class AddressDTO
    {
        public string? PermanentAddress { get; set; }

        public string? TemporaryAddress { get; set; }

        public string? MailingAddress { get; set; }
    }
}
