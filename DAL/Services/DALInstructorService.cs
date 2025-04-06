using DAL.Api;
using DAL.Models;

namespace DAL.Services
{
    public class DALInstructorService : IDALInstructor
    {
        dbcontext dbcontext;
        public DALInstructorService(dbcontext data)
        {
            dbcontext = data;
        }

        public void Create(Instructor instructor)
        {
           dbcontext.Instructors.Add(instructor);
           dbcontext.SaveChanges();
        }

        public void Delete(Instructor instructor)
        {
            throw new NotImplementedException();
        }

        public List<Instructor> Get()
        {
            throw new NotImplementedException();
        }

        public Instructor GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Instructor instructor)
        {
            throw new NotImplementedException();
        }
    }
}
