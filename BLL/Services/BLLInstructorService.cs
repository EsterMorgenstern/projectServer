using BLL.Api;
using BLL.Models;
using DAL.Api;

namespace BLL.Services
{
    public class BLLInstructorService : IBLLInstructor
    {
        IDAL dal;
        public BLLInstructorService(IDAL dal)
        {
            this.dal = dal;
        }

        public void Create(BLLInstructor student)
        {
            throw new NotImplementedException();
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
