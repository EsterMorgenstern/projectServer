using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Api
{
    public interface IDALAttendance
    {
        List<Attendance> Get();
        void Create(Attendance attendance);
        Attendance GetById(int id);
        void Delete(Attendance attendance);
        void Update(Attendance attendance);
    }
}
