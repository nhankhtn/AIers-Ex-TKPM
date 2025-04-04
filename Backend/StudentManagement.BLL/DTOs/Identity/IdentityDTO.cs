using StudentManagement.Domain.Enums;
using System;

namespace StudentManagement.BLL.DTOs.Identity
{
    public class IdentityDTO
    {

        public string? Type { get; set; }

        public string? DocumentNumber { get; set; }

        public DateTime? IssueDate { get; set; } // Ngày cấp

        public string? IssuePlace { get; set; } 

        public DateTime? ExpiryDate { get; set; } // Ngày hết hạn

        public string? CountryIssue { get; set; } 

        public bool? IsChip { get; set; } // Có gắn chip hay không (dành cho CCCD)

        public string? Notes { get; set; } // Ghi chú (nếu có)
    }
}