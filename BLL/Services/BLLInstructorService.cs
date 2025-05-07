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

        public void Delete(int id)
        {
          dal.Instructors.Delete(id);
        }

        public List<BLLInstructor> Get()
        {
            var pList = dal.Instructors.Get();
            List<BLLInstructor> list = new();
            pList.ForEach(p => list.Add(new BLLInstructor()
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                City = p.City ?? string.Empty,
                Email = p.Email=string.Empty,
                Phone = p.Phone 
            }));
            return list;
        }

        public BLLInstructor GetById(int id)
        {
            Instructor? p = dal.Instructors.GetById(id); 
            if (p != null)
            {
                BLLInstructor t = new BLLInstructor()
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    City = p.City ?? string.Empty, // Null-coalescing operator to handle null
                    Email = p.Email ?? string.Empty, // Null-coalescing operator to handle null
                    Phone = p.Phone,
                };
                return t;
            }
            return new BLLInstructor()
            {
                Id = 0,
                FirstName = string.Empty,
                LastName = string.Empty,
                City = string.Empty,
                Email = string.Empty,
                Phone = string.Empty    
            };
        }

        public void Update(BLLInstructor instructor)
        {
            throw new NotImplementedException();
        }
    }
}
