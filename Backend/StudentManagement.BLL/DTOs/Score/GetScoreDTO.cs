using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.DTOs.Score
{ 

    public class GetScoreDTO
    {
        public string StudentId { get; set; } = null!;

        public string StudentName { get; set; } = null!;

        public decimal MidTermScore { get; set; }

        public decimal FinalScore { get; set; }

        public decimal TotalScore { get; set; }

        public char Grade { get; set; }

        public bool IsPassed { get; set; }
    }
}
