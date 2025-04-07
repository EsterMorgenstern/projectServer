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

        public void Delete(BLLInstructor instructor)
        {
            throw new NotImplementedException();
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
                City = p.City ?? string.Empty, // Fix for CS8601
                Email = p.Email,
                Phone = p.Phone ?? string.Empty // Fix for CS8601

            }));
            return list;
        }

        public BLLInstructor? GetById(int id)
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
                    Phone = p.Phone ?? string.Empty
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
