using StudentManagement.Domain.Enums;
using System;

namespace StudentManagement.BLL.DTOs.Identity
{
    public class IdentityDTO
    {

        public string Type { get; set; } = IdentityType.CCCD.ToString();// Loại giấy tờ (CMND/CCCD/Hộ chiếu)

        public string DocumentNumber { get; set; } = string.Empty;

        public DateTime IssueDate { get; set; } // Ngày cấp

        public string IssuePlace { get; set; } = string.Empty;

        public DateTime ExpiryDate { get; set; } // Ngày hết hạn

        public string CountryIssue { get; set; } = string.Empty; // Quốc gia cấp

        public bool IsChip { get; set; } // Có gắn chip hay không (dành cho CCCD)

        public string? Notes { get; set; } // Ghi chú (nếu có)
    }
}