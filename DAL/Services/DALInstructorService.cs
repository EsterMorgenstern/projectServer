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
            if (string.IsNullOrWhiteSpace(instructor.FirstName) || string.IsNullOrWhiteSpace(instructor.LastName))
            {
                throw new ArgumentException("Instructor's first and last name cannot be empty.");
            }
            dbcontext.Instructors.Add(instructor);
            dbcontext.SaveChanges();
        }

        public void Delete(int id)
        {
            var instructor = dbcontext.Instructors.SingleOrDefault(x => x.Id == id);
            if (instructor == null)
            {
                throw new KeyNotFoundException($"Instructor with ID {id} not found.");
            }

            dbcontext.Instructors.Remove(instructor);
            dbcontext.SaveChanges();
        }

        public List<Instructor> Get()
        {
            try
            {
                if (dbcontext.Instructors == null || !dbcontext.Instructors.Any())
                {
                    throw new Exception("No Instructor records found.");
                }

                return dbcontext.Instructors.ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while retrieving Instructor records.", ex);
            }
        }


        public Instructor GetById(int id) 
        {
            var instructor = dbcontext.Instructors.SingleOrDefault(x => x.Id == id);
            if (instructor == null)
            {
                throw new KeyNotFoundException($"Instructor with ID {id} not found.");
            }
            return instructor;
        }



        public void Update(Instructor instructor)
        {
            dbcontext.Instructors.Update(instructor);
            dbcontext.SaveChanges();
        }

    }
}
