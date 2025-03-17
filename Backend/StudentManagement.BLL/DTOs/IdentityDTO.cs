using StudentManagement.Domain.Enums;
using System;

namespace StudentManagement.Application.DTOs
{
    public class IdentityDTO
    {

        public IdentityType IdentityType { get; set; } // Loại giấy tờ (CMND/CCCD/Hộ chiếu)

        public string IdentityNumber { get; set; } = string.Empty;

        public DateTime IssueDate { get; set; } // Ngày cấp

        public DateTime ExpiryDate { get; set; } // Ngày hết hạn

        public string Country { get; set; } = string.Empty; // Quốc gia cấp

        public bool HasChip { get; set; } // Có gắn chip hay không (dành cho CCCD)

        public string? Notes { get; set; } // Ghi chú (nếu có)
    }
}