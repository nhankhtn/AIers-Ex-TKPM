using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.DTOs.Score
{
    public class UpdateScoreDTO
    {
        public string StudentId { get; set; } = null!;
        public decimal? MidTermScore { get; set; }
        public decimal? FinalScore { get; set; }
    }
}
