using System.Text.Json;
using DAL.Api;
using DAL.Models;

namespace DAL.Services
{
    public class DALAttendanceService : IDALAttendance
    {
        dbcontext dbcontext;

        public DALAttendanceService(dbcontext data)
        {
            dbcontext = data;
        }

        public void Create(Attendance attendance)
        {
            try { 
            dbcontext.Attendances.Add(attendance);
            dbcontext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTime.UtcNow:O}] ERROR DAL.Attendances.Create — entity: {ex}");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public void Delete(int attendanceId)
        {
            var trackedAttendance = dbcontext.Attendances.SingleOrDefault(x => x.AttendanceId ==attendanceId);
            if (trackedAttendance != null)
            {
                dbcontext.Attendances.Remove(trackedAttendance);
                dbcontext.SaveChanges();
            }
        }

        public void DeleteByGroupAndDate(int groupId, DateOnly date)
        {
            var attendanceToDelete = dbcontext.Attendances.Where(x => x.GroupId == groupId && x.Date == date).ToList();
            if (attendanceToDelete.Any())
            {
                dbcontext.Attendances.RemoveRange(attendanceToDelete);
                dbcontext.SaveChanges();
            }
        }

        public List<Attendance> Get()
        {
            try
            {
                if (dbcontext.Attendances == null || !dbcontext.Attendances.Any())
                {
                    throw new Exception("No attendance records found.");
                }

                return dbcontext.Attendances.ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while retrieving attendance records.", ex);
            }
        }
        /// <summary>
        /// GetById לפי AttendanceId
        /// </summary>
        /// <param name="id">AttendanceId</param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>

        public Attendance GetById(int id)
        {
            var attendance = dbcontext.Attendances.SingleOrDefault(x => x.AttendanceId == id);
            if (attendance == null)
            {
                throw new KeyNotFoundException($"Attendance with ID {id} not found.");
            }
            return attendance;
        }

        public List<Attendance> GetAttendanceByGroupAndDate(int groupId, DateOnly date)
        {
            return dbcontext.Attendances.Where(x => x.Date == date && x.GroupId == groupId).ToList();
        }

        public List<Attendance> GetByGroupAndDateRange(int groupId, DateOnly startDate, DateOnly endDate)
        {
            return dbcontext.Attendances.Where(x => x.GroupId == groupId &&
                                                   x.Date >= startDate &&
                                                   x.Date <= endDate).ToList();
        }

        public async Task<List<Attendance>> GetAttendanceByStudent(int studentId)
        {
            return dbcontext.Attendances.Where(x => x.StudentId == studentId).ToList();
        }

        public List<Attendance> GetAttendanceByStudentAndDateRange(int studentId, DateOnly startDate, DateOnly endDate)
        {
            return dbcontext.Attendances.Where(x => x.StudentId == studentId &&
                                                   x.Date >= startDate &&
                                                   x.Date <= endDate).ToList();
        }

        public List<Attendance> GetAttendanceByGroup(int groupId)
        {
            return dbcontext.Attendances.Where(x => x.GroupId == groupId).ToList();
        }

        public void Update(Attendance attendance)
        {
            dbcontext.Attendances.Update(attendance);
            dbcontext.SaveChanges();
        }
    }
}