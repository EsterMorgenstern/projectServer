using DAL.Api;
using DAL.Models;


namespace DAL.Services
{
    public class DALStudentService : IDALStudent
    {
        dbcontext dbcontext;
        public DALStudentService(dbcontext data)
        {
            dbcontext = data;
        }

        public void Create(Student item)
        {
            dbcontext.Students.Add(item);
            dbcontext.SaveChanges();
        }



        public List<Student> Get()
        {
            return dbcontext.Students.ToList();
        }

        public Student GetById(int id)
        {
            return dbcontext.Students.ToList().Find(x => x.Id == id);
        }

        public void Delete(Student student)
        {
            dbcontext.Students.Remove(student);
            dbcontext.SaveChanges();

        }
        public void Update(Student student)
        {
            dbcontext.Students.Update(student);
            dbcontext.SaveChanges();

        }
    }
}
