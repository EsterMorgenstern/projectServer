using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class BLLAttendanceRecord
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public bool WasPresent { get; set; }
    }
}
