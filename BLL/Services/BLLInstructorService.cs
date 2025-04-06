using BLL.Api;
using BLL.Models;
using DAL.Api;
using DAL.Models;

namespace BLL.Services
{
    public class BLLInstructorService : IBLLInstructor
    {
        IDAL dal;
        public BLLInstructorService(IDAL dal)
        {
            this.dal = dal;
        }

        public void Create(BLLInstructor instructor)
        {
            Instructor p = new Instructor()
            {
                Id = instructor.Id,
                City = instructor.City,
                Email = instructor.Email,
                FirstName = instructor.FirstName,
                LastName = instructor.LastName,
                Phone = instructor.Phone
            };  
            dal.Instructors.Create(p);
        }

        public void Delete(BLLInstructor student)
        {
            throw new NotImplementedException();
        }

        public List<BLLInstructor> Get()
        {
            throw new NotImplementedException();
        }

        public BLLInstructor GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(BLLInstructor student)
        {
            throw new NotImplementedException();
        }
    }
}
