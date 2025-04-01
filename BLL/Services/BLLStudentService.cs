using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                BirthDate = student.BirthDate,
                City = student.City,
                School = student.School,
                HealthFund = student.HealthFund

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
            { Id = p.Id, FirstName = p.FirstName ?? "", LastName = p.LastName ?? "", Phone = p.Phone, BirthDate = (DateTime)p.BirthDate, City = p.City, School = p.School, HealthFund = p.HealthFund }));
            return list;
        }
    }
}
