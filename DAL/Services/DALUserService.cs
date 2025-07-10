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
            try
            {
                if (dbcontext.Users == null || !dbcontext.Users.Any())
                {
                    throw new Exception("No User records found.");
                }

                return dbcontext.Users.ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while retrieving User records.", ex);
            }
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
