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
            try
            {
                if (dbcontext.Students == null || !dbcontext.Students.Any())
                {
                    throw new Exception("No Student records found.");
                }

                return dbcontext.Students.ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while retrieving Student records.", ex);
            }
        }


        public Student GetById(int id) 
        {
            var student = dbcontext.Students.ToList().Find(x => x.Id == id);
            if (student == null)
            {
                throw new KeyNotFoundException($"Student with ID {id} not found."); 
            }
            return student;
        }

        public void Delete(int id)
        {
            var student = dbcontext.Students.SingleOrDefault(x => x.Id == id);
            if (student == null)
            {
                throw new KeyNotFoundException($"Student with ID {id} not found.");
            }

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
