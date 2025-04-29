using System.Diagnostics.CodeAnalysis;
using BLL.Api;
using BLL.Models;
using DAL.Api;
using DAL.Models;

namespace BLL.Services
{
    public class BLLStudentService : IBLLStudent
    {
        IDAL dal;
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
                Phone = student.Phone,// Fix for CS0029: Convert string to int
                Community = student.Community,
                Active = student.Active // Fix for CS8601: Possible null reference assignment   
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
                BirthDate = p.BirthDate.ToDateTime(TimeOnly.MinValue),
                City = p.City ?? "", // Fix for CS8601: Possible null reference assignment
                School = p.School ?? "", // Fix for CS8601: Possible null reference assignment
                HealthFund = p.HealthFund ?? "", // Fix for CS8601: Possible null reference assignment
                Community=p.Community ?? "", // Fix for CS8601: Possible null reference assignment
                Active = p.Active ?? true // Fix for CS8601: Possible null reference assignment 
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
                    BirthDate = p.BirthDate.ToDateTime(TimeOnly.MinValue),
                    City = p.City ?? "", // Fix for CS8601: Possible null reference assignment
                    School = p.School ?? "", // Fix for CS8601: Possible null reference assignment
                    HealthFund = p.HealthFund ?? "", // Fix for CS8601: Possible null reference assignment
                    Community = p.Community ?? "", // Fix for CS8601: Possible null reference assignment  
                    Active=p.Active ?? true // Fix for CS8601: Possible null reference assignment   
                };
                return t2;
            }

           return new BLLStudent()
            {
                Id = id,
                FirstName = "",
                LastName = "",
                Phone = "",
                BirthDate = DateTime.MinValue,
                City = "",
                School = "",
                HealthFund = "",
               Community = "",
               Active = true // Fix for CS8601: Possible null reference assignment
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
            m.BirthDate = DateOnly.FromDateTime(student.BirthDate);
            m.City = student.City;
            m.School = student.School;
            m.HealthFund = student.HealthFund;
            m.Community = student.Community;
            m.Active = student.Active; // Fix for CS8601: Possible null reference assignment    

            dal.Students.Update(m);
        }
    }
}
