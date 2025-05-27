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
                BirthDate = DateOnly.FromDateTime(student.BirthDate),
                City = student.City,
                School = student.School,
                HealthFund = student.HealthFund,
                Phone = student.Phone,
                Gender = student.Gender,
                Sector = student.Sector,
                LastActivityDate = DateOnly.FromDateTime(student.LastActivityDate)
            };
            dal.Students.Create(p);
        }

        /// <summary>
        /// get לתלמידים
        /// </summary>
        /// <returns>List  של התלמידים</returns>
        public List<BLLStudent> Get()
        {
            var pList = dal.Students.Get();
            List<BLLStudent> list = new();
            pList.ForEach(p => list.Add(new BLLStudent()
            {
                Id = p.Id,
                FirstName = p.FirstName ?? "", // Fix for CS8601: Possible null reference assignment
                LastName = p.LastName ?? "", // Fix for CS8601: Possible null reference assignment
                Phone = p.Phone.ToString(), // Fix for CS0029: Convert int to string
                BirthDate = p.BirthDate.ToDateTime(TimeOnly.MinValue), // Fix for CS1503: Convert DateOnly to DateTime
                City = p.City ?? "", // Fix for CS8601: Possible null reference assignment
                School = p.School ?? "", // Fix for CS8601: Possible null reference assignment
                HealthFund = p.HealthFund ?? "", // Fix for CS8601: Possible null reference assignment
                Gender = p.Gender ?? "",
                Sector = p.Sector ?? "",
                LastActivityDate = p.LastActivityDate != null ? p.LastActivityDate.Value.ToDateTime(TimeOnly.MinValue) : DateTime.MinValue // Fix for CS1503: Convert DateOnly? to DateTime
            }));
            return list;
        }
        [return: NotNullIfNotNull("id")]
        public BLLStudent? GetById(int id)
        {
            var p = dal.Students.GetById(id);
            if (p != null)
            {
                BLLStudent t2 = new BLLStudent()
                {
                    Id = p.Id,
                    FirstName = p.FirstName ?? "", // Fix for CS8601: Possible null reference assignment  
                    LastName = p.LastName ?? "", // Fix for CS8601: Possible null reference assignment  
                    Phone = p.Phone.ToString(), // Fix for CS0029: Convert int to string  
                    BirthDate = p.BirthDate.ToDateTime(TimeOnly.MinValue), // Fix for CS1503: Convert DateOnly to DateTime
                    City = p.City ?? "", // Fix for CS8601: Possible null reference assignment  
                    School = p.School ?? "", // Fix for CS8601: Possible null reference assignment  
                    HealthFund = p.HealthFund ?? "", // Fix for CS8601: Possible null reference assignment  
                    Gender = p.Gender ?? "",
                    Sector = p.Sector ?? "",
                    LastActivityDate = p.LastActivityDate != null ? p.LastActivityDate.Value.ToDateTime(TimeOnly.MinValue) : DateTime.MinValue // Fix for CS1503: Convert DateOnly? to DateTime
                };
                return t2;
            }

            return new BLLStudent()
            {
                Id = id,
                FirstName = "",
                LastName = "",
                Phone = "",
                BirthDate = DateTime.MinValue, // Fix for CS1001 and CS0117: Use DateTime.MinValue as a default value  
                City = "",
                School = "",
                HealthFund = "",
                Gender = "",
                Sector = "",
                LastActivityDate = DateTime.MinValue
            };
        }

        public void Delete(int id)
        {
            dal.Students.Delete(id);
        }

        public void Update(BLLStudent student)
        {
            var m = dal.Students.GetById(student.Id);
            m.Id = student.Id;
            m.FirstName = student.FirstName;
            m.LastName = student.LastName;
            m.Phone = student.Phone; // Fix for CS0029: Convert string to int
            m.BirthDate = DateOnly.FromDateTime(student.BirthDate); // Fix for CS0029: Convert DateTime to DateOnly
            m.City = student.City;
            m.School = student.School;
            m.HealthFund = student.HealthFund;
            m.Gender = student.Gender;
            m.Sector = student.Sector;
            m.LastActivityDate = DateOnly.FromDateTime(student.LastActivityDate);

            dal.Students.Update(m);
        }
    }
}
