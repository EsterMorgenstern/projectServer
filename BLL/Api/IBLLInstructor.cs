using BLL.Models;

namespace BLL.Api
{
    public interface IBLLInstructor
    {
        List<BLLInstructor> Get();
        void Create(BLLInstructor instructor);
        public BLLInstructor GetById(int id);
        public void Delete(int id);
        public void Update(BLLInstructor instructor);
    }
}
