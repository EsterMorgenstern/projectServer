using BLL.Api;
using BLL.Models;
using DAL.Api;
using DAL.Models;

namespace BLL.Services
{
    public class BLLInstructorService : IBLLInstructor
    {
        private readonly IDAL dal;
        public BLLInstructorService(IDAL dal)
        {
            this.dal = dal;
        }

        public void Create(BLLInstructor instructor)
        {
            Instructor p = new Instructor()
            {
                Id = instructor.Id,
                FirstName = instructor.FirstName,
                LastName = instructor.LastName,
                Phone = instructor.Phone,
                Email = instructor.Email,
                City = instructor.City,
                Sector =instructor.Sector
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
                Email = p.Email ?? string.Empty,
                Phone = p.Phone,
                Sector=p.Sector??string.Empty
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
                    Sector=p.Sector?? string.Empty
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
                Phone = string.Empty,
                Sector=string.Empty
            };
        }

        public void Update(BLLInstructor instructor)
        {
            var m = dal.Instructors.GetById(instructor.Id);
            m.Id = instructor.Id;
            m.FirstName = instructor.FirstName;
            m.LastName = instructor.LastName;
            m.Phone = instructor.Phone; // Fix for CS0029: Convert string to int
            m.City = instructor.City;
            m.Email = instructor.Email;
            m.Sector = instructor.Sector;
           
            dal.Instructors.Update(m);
        }
    }
}
