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
                Phone = user.Phone,
                Role = user.Role
            };
            dal.Users.Create(u);
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<BLLUser> Get()
        {
            return dal.Users.Get().Select(c => new BLLUser()
            {
               Id = c.Id,
               Email = c.Email,
               FirstName = c.FirstName, 
               LastName = c.LastName,   
               Phone = c.Phone,
               Role = c.Role

            }).ToList();
        }

        public BLLUser GetById(int id)
        {

            User user = dal.Users.GetById(id);
            BLLUser bluser = new BLLUser()
            {
                Id=user.Id,
                FirstName=user.FirstName,
                LastName=user.LastName,
                Email= user.Email,
                Phone = user.Phone,
                Role=user.Role

            };
            return bluser;
        }

        public void Update(BLLUser user)
        {
            throw new NotImplementedException();
        }
    }
}
