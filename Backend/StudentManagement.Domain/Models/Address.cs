﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Domain.Models
{
    public class Address
    {
        [Key]
        [Column("address_id", TypeName = "int")]
        public int Id { get; set; }

        [Required]
        [Column("permanent_address", TypeName = "nvarchar(100)")]
        public string? PermanentAddress { get; set; }

        [Required]
        [Column("temporary_address", TypeName = "nvarchar(100)")]
        public string? TemporaryAddress { get; set; }

        [Required]
        [Column("mailing_address", TypeName = "nvarchar(100)")]
        public string? MailingAddress { get; set; }

        [Required]
        [Column("student_id", TypeName = "varchar(8)")]
        public string StudentId { get; set; } = string.Empty;
           
        public Student Student { get; set; } = null!;
    }
}
