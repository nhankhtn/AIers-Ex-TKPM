using StudentManagement.Domain.Enums;
using System;

namespace StudentManagement.Application.DTOs
{
    public class IdentityDTO
    {
        public int Id { get; set; } // Mã định danh

        public IdentityType IdentityType { get; set; } // Loại giấy tờ (CMND/CCCD/Hộ chiếu)

        public DateTime IssueDate { get; set; } // Ngày cấp

        public DateTime ExpiryDate { get; set; } // Ngày hết hạn

        public string Country { get; set; } = string.Empty; // Quốc gia cấp

        public bool HasChip { get; set; } // Có gắn chip hay không (dành cho CCCD)

        public string? Notes { get; set; } // Ghi chú (nếu có)
    }
}