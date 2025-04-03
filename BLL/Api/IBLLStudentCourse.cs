using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Models;

namespace BLL.Api
{
    public interface IBLLStudentCourse
    {
        List<BLLStudentCourse> Get();
        void Create(BLLStudentCourse studentCourses);
        public BLLStudentCourse GetById(int id);
        public void Delete(BLLStudentCourse studentCourses);
        public void Update(BLLStudentCourse studentCourses);
    }
}
