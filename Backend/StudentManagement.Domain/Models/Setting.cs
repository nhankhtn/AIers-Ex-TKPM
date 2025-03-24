using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Domain.Models
{
    public class Setting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("email_domain")]
        public string EmailDomain { get; set; } = "@gmail.com";

        [Column("email_pattern")]
        public string EmailPattern { get; set; } = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
    }
}
