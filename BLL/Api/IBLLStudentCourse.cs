using BLL.Models;

namespace BLL.Api
{
    public interface IBLLStudentCourse
    {
        List<BLLStudentCourse> Get();
        void Create(BLLStudentCourse studentCourses);
        public BLLStudentCourse GetById(int cId,int sId);
        public BLLStudentCourse GetByIdCourse(int cId); 
        public List<BLLStudentCoursePerfect> GetByIdStudent(int sId);
        public void Delete(BLLStudentCourse studentCourses);
        public void Update(BLLStudentCourse studentCourses);
    }
}
