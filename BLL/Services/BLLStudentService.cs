using System.Diagnostics.CodeAnalysis;
using BLL.Api;
using BLL.Models;
using DAL.Api;
using DAL.Models;

namespace BLL.Services
{
    public class BLLStudentService : IBLLStudent
    {
        private readonly IDAL dal;
        public BLLStudentService(IDAL dal)
        {
            this.dal = dal;
        }
        /// <summary>
        /// הוספת תלמיד
        /// </summary>
        /// <param name="item"></param>
        public void Create(BLLStudent student)
        {
            Student p = new Student()
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Age = student.Age,
                City = student.City,
                School = student.School,
                HealthFund = student.HealthFund,
                Phone = student.Phone,
                SecondaryPhone = student.SecondaryPhone,
                Class = student.Class,
                Sector = student.Sector,
                LastActivityDate = DateOnly.FromDateTime(student.LastActivityDate),
                Status=student.Status,
                Email=student.Email,
                CreatedBy=student.CreatedBy,
                IdentityCard = student.IdentityCard
            };
            dal.Students.Create(p);
        }

        /// <summary>
        /// get לתלמידים
        /// </summary>
        /// <returns>List  של התלמידים</returns>
        public List<BLLStudent> Get()
        {
            try
            {
                var pList = dal.Students.Get();
                if (pList == null || !pList.Any())
                {
                    Console.WriteLine("No students found.");
                    return new List<BLLStudent>(); // מחזיר מערך ריק
                }

                List<BLLStudent> list = new();
                pList.ForEach(p => list.Add(new BLLStudent()
                {
                    Id = p.Id,
                    FirstName = p.FirstName ?? "",
                    LastName = p.LastName ?? "",
                    Phone = p.Phone.ToString(),
                    SecondaryPhone = p.SecondaryPhone?.ToString() ?? "",
                    Age = p.Age,
                    City = p.City ?? "",
                    School = p.School ?? "",
                    HealthFund = p.HealthFund ?? "",
                    Class = p.Class ?? "",
                    Sector = p.Sector ?? "",
                    LastActivityDate = p.LastActivityDate != null ? p.LastActivityDate.Value.ToDateTime(TimeOnly.MinValue) : DateTime.MinValue,
                    Status=p.Status?? "",
                    Email=p.Email ?? "",
                    CreatedBy=p.CreatedBy ?? "",
                    IdentityCard = p.IdentityCard ?? ""
                }));
                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching students: {ex.Message}");
                return new List<BLLStudent>(); // מחזיר מערך ריק במקרה של שגיאה
            }
        }
        [return: NotNullIfNotNull("id")]
        public BLLStudent GetById(int id)
        {
            try
            {
                var p = dal.Students.GetById(id);
                if (p != null)
                {
                    return new BLLStudent()
                    {
                        Id = p.Id,
                        FirstName = p.FirstName ?? "",
                        LastName = p.LastName ?? "",
                        Phone = p.Phone.ToString(),
                        SecondaryPhone = p.SecondaryPhone?.ToString() ?? "",
                        Age = p.Age,
                        City = p.City ?? "",
                        School = p.School ?? "",
                        HealthFund = p.HealthFund ?? "",
                        Class = p.Class ?? "",
                        Sector = p.Sector ?? "",
                        LastActivityDate = p.LastActivityDate != null ? p.LastActivityDate.Value.ToDateTime(TimeOnly.MinValue) : DateTime.MinValue,
                        Status=p.Status ?? "",
                        Email=p.Email ?? "",
                        CreatedBy=p.CreatedBy??"",
                        IdentityCard = p.IdentityCard ?? ""
                    };
                }

                Console.WriteLine($"Student with ID {id} not found.");
                return new BLLStudent()
                {
                    Id = id,
                    FirstName = "",
                    LastName = "",
                    Phone = "",
                    SecondaryPhone="",
                    Age = 0,
                    City = "",
                    School = "",
                    HealthFund = "",
                    Class = "",
                    Sector = "",
                    LastActivityDate = DateTime.MinValue,
                    Status="",
                    Email="",
                    CreatedBy="",
                    IdentityCard = ""
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching student with ID {id}: {ex.Message}");
                return new BLLStudent()
                {
                    Id = id,
                    FirstName = "",
                    LastName = "",
                    Phone = "",
                    SecondaryPhone = "",
                    Age = 0,
                    City = "",
                    School = "",
                    HealthFund = "",
                    Class = "",
                    Sector = "",
                    LastActivityDate = DateTime.MinValue,
                    Status="",
                    Email="",
                    CreatedBy="",
                    IdentityCard = ""

                };
            }
        }


        public void Delete(int id)
        {
            var attendances= dal.Attendances.GetAttendanceByStudent(id);
            foreach (var item in attendances)
            {
                dal.Attendances.Delete(item.AttendanceId);
            }
            var notes = dal.StudentNotes.GetById(id);
            foreach (var item in notes)
            {
                dal.StudentNotes.Delete(item.NoteId);
            }
            var groupStudents=dal.GroupStudents.GetByStudentId(id);
            foreach (var item in groupStudents)
            {
                dal.GroupStudents.Delete(item);
                var group = dal.Groups.GetById(item.GroupId);
                group.MaxStudents = (group.MaxStudents ?? 0) + 1;
                dal.Groups.Update(group);
            }
           
            dal.Students.Delete(id);
        }

        public void Update(BLLStudent student)
        {
            var m = dal.Students.GetById(student.Id);
            m.Id = student.Id;
            m.FirstName = student.FirstName;
            m.LastName = student.LastName;
            m.Phone = student.Phone; 
            m.SecondaryPhone = student.SecondaryPhone;
            m.Age = student.Age; 
            m.City = student.City;
            m.School = student.School;
            m.HealthFund = student.HealthFund;
            m.Class = student.Class;
            m.Sector = student.Sector;
            m.LastActivityDate = DateOnly.FromDateTime(student.LastActivityDate);
            m.Status = student.Status;
            m.Email = student.Email;
            m.CreatedBy = student.CreatedBy;
            m.IdentityCard = student.IdentityCard;

            dal.Students.Update(m);
        }
    }
}
