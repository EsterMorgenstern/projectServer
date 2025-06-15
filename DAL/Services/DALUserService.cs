using DAL.Api;
using DAL.Models;

namespace DAL.Services
{
    public class DALUserService : IDALUser
    {

        dbcontext dbcontext;
        public DALUserService(dbcontext data)
        {
            dbcontext = data;
        }

        public void Create(User user)
        {
            throw new NotImplementedException();
        }

        public void Delete(int userId)
        {
            throw new NotImplementedException();
        }

        public List<User> Get()
        {
            throw new NotImplementedException();
        }

        public User GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(User user)
        {
            throw new NotImplementedException();
        }
    }
}
