﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.DTOs
{
    public class AddressDTO
    {

        public string? Ward { get; set; }

        public string? District { get; set; }

        public string? City { get; set; }

        public string? Country { get; set; }
    }
}
