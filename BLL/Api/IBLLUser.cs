using BLL.Models;

namespace BLL.Api
{
    public interface IBLLUser
    {
        List<BLLUser> Get();
        void Create(BLLUser user);
        public BLLUser GetById(int id);
        public void Delete(int id);
        public void Update(BLLUser user);
    }
}
