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

        public BLLStudent? GetById(int id)
        {
            var p = dal.Students.GetById(id);
            if (p != null)
            {
                BLLStudent t2 = new BLLStudent()
                { Id = p.Id, FirstName = p.FirstName ?? "", LastName = p.LastName ?? "", Phone = p.Phone, BirthDate = (DateTime)p.BirthDate, City = p.City, School = p.School, HealthFund = p.HealthFund };
                return t2;
            }
            return null;
        }
        public void Delete(BLLStudent student)
        {
            var m = dal.Students.GetById(student.Id);
            //List<BLLCourse> courses = GetCourses(student.Id);
            //if (courses != null)
            //{
            //    foreach (var item in courses)
            //    {
            //        BLLManager blm = new BLLManager();
            //        blm.Marks.Delete(item);

            //    }
            //}
            dal.Students.Delete(m);
        }


        public void Update(BLLStudent student)
        {
            var m = dal.Students.GetById(student.Id);
            m.Id = student.Id;
            m.FirstName = student.FirstName;
            m.LastName = student.LastName;
            m.Phone = student.Phone;
            m.BirthDate = student.BirthDate;
            m.City = student.City;
            m.School = student.School;
            m.HealthFund = student.HealthFund;

            dal.Students.Update(m);
        }


    }
}
