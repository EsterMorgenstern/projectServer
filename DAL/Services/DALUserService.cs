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
            dbcontext.Users.Add(user);
            dbcontext.SaveChanges();

        }

        public void Delete(int userId)
        {
            throw new NotImplementedException();
        }

        public List<User> Get()
        {
            return dbcontext.Users.ToList();
        }

        public User GetById(int id)
        {
            var user = dbcontext.Users.SingleOrDefault(x => x.Id == id);
            if (user == null)
            {
                throw new KeyNotFoundException($"user with ID {id} not found.");
            }
            return user;
        }

        public void Update(User user)
        {
            throw new NotImplementedException();
        }
    }
}
