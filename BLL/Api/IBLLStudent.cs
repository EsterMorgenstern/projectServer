using BLL.Models;

namespace BLL.Api
{
    public interface IBLLStudent
    {
        List<BLLStudent> Get();
        void Create(BLLStudent student);
        public BLLStudent GetById(int id);
        public void Delete(BLLStudent student);
        public void Update(BLLStudent student);
    }
}
