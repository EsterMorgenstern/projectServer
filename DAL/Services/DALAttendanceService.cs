using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Api;
using DAL.Models;

namespace DAL.Services
{
    public class DALAttendanceService:IDALAttendance
    {
        dbcontext dbcontext;
        public DALAttendanceService(dbcontext data)
        {
            dbcontext = data;
        }
        public void Create(Attendance attendance)
        {
            dbcontext.Attendances.Add(attendance);
            dbcontext.SaveChanges();
        }
        public void Delete(int id)
        {
            var trackedAttendance = dbcontext.Attendances.Find(id);
            if (trackedAttendance != null)
            {
                dbcontext.Attendances.Remove(trackedAttendance);
                dbcontext.SaveChanges();
            }
        }

        public void Delete(Attendance attendance)
        {
            throw new NotImplementedException();
        }

        public List<Attendance> Get()
        {
            return dbcontext.Attendances.ToList();
        }
        public Attendance GetById(int id)
        {
            var attendance = dbcontext.Attendances.SingleOrDefault(x => x.AttendanceId == id);
            if (attendance == null)
            {
                throw new KeyNotFoundException($"Attendance with ID {id} not found.");
            }
            return attendance;
        }
        public void Update(Attendance attendance)
        {
            dbcontext.Attendances.Update(attendance);
            dbcontext.SaveChanges();
        }   
    }
}
