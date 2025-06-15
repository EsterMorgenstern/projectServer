using BLL.Api;
using BLL.Models;
using DAL.Api;
using DAL.Models;

namespace BLL.Services
{
    public class BLLUserService : IBLLUser
    {

        private readonly IDAL dal;
        public BLLUserService(IDAL dal)
        {
            this.dal = dal;
        }

        public void Create(BLLUser user)
        {
            User u = new User()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
            };
            dal.Users.Create(u);
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<BLLUser> Get()
        {
            throw new NotImplementedException();
        }

        public BLLUser GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(BLLUser user)
        {
            throw new NotImplementedException();
        }
    }
}
