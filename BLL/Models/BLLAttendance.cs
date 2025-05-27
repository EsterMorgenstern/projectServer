using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class BLLAttendance
    {
        public int AttendanceId { get; set; }

        public int? GroupId { get; set; }

        public int? StudentId { get; set; }

        public DateOnly? Date { get; set; }

        public bool? WasPresent { get; set; }
    }
}
