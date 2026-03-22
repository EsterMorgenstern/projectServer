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
            try
            {
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
            var trackedAttendance = dbcontext.Attendances.SingleOrDefault(x => x.AttendanceId == attendanceId);
            if (trackedAttendance != null)
            {
                dbcontext.Attendances.Remove(trackedAttendance);
                dbcontext.SaveChanges();
            }
        }

        public void DeleteByGroupAndDate(int groupId, DateOnly date)
        {
            throw new NotImplementedException();
        }

        //public void DeleteByGroupAndDate(int groupId, DateOnly date)
        //{
        //    var attendanceToDelete = dbcontext.Attendances.Where(x => x.GroupId == groupId && x.Date == date).ToList();
        //    if (attendanceToDelete.Any())
        //    {
        //        dbcontext.Attendances.RemoveRange(attendanceToDelete);
        //        dbcontext.SaveChanges();
        //    }
        //}

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

        public List<Attendance> GetAttendanceByGroup(int groupId)
        {
            throw new NotImplementedException();
        }

        public List<Attendance> GetAttendanceByGroupAndDate(int groupId, DateOnly date)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Attendance>> GetAttendanceByStudent(int studentId)
        {
            return await Task.Run(() => dbcontext.Attendances.Where(x => x.StudentId == studentId).ToList());
        }

        public List<Attendance> GetAttendanceByStudentAndDateRange(int studentId, DateOnly startDate, DateOnly endDate)
        {
            throw new NotImplementedException();
        }

        public List<Attendance> GetByGroupAndDateRange(int groupId, DateOnly startDate, DateOnly endDate)
        {
            throw new NotImplementedException();
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

        /// <summary>
        /// עדכון נוכחות 
        /// </summary>
        /// <param name="attendance"></param>
        public void Update(Attendance attendance)
        {
            dbcontext.Attendances.Update(attendance);
            dbcontext.SaveChanges();
        }
        /// <summary>
        /// עדכון רשימת נוכחויות 
        /// </summary>
        /// <param name="attendances"></param>
        public void BatchUpdateAttendances(List<Attendance> attendances)
        {
            foreach (var att in attendances)
            {
                var existing = GetByLessonAndStudent(att.LessonId, att.StudentId);
                if (existing != null)
                {
                    att.AttendanceId = existing.AttendanceId;
                    Update(att);
                }
                else
                {
                    Create(att);
                }
            }
        }

        /// <summary>
        /// פונקציה עזר לשליפת נוכחות לפי שיעור ותלמיד
        /// </summary>
        /// <param name="lessonId"></param>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public Attendance? GetByLessonAndStudent(int lessonId, int studentId)
        {
            return dbcontext.Attendances.FirstOrDefault(a => a.LessonId == lessonId && a.StudentId == studentId);
        }


    }
}