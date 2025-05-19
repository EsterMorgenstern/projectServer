using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class BLLBranch
    {
        public int BranchId { get; set; }

        public string Name { get; set; } = null!;

        public string? Address { get; set; }

        public int? MaxGroupSize { get; set; }
        public string? City { get; set; }
    }
}
