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
                Email = p.Email,
                Phone = p.Phone ?? 0 // Use null-coalescing operator to handle nullable Phone
            }));
            return list;
        }

        public BLLInstructor GetById(int id)
        {
            Instructor p = dal.Instructors.GetById(id);
            if (p != null)
            {
                BLLInstructor t = new BLLInstructor()
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    City = p.City ?? string.Empty,
                    Email = p.Email,
                    Phone = p.Phone ?? 0,
                };
                return t;
            }
            return null; 
        }

        public void Update(BLLInstructor instructor)
        {
            throw new NotImplementedException();
        }
    }
}
