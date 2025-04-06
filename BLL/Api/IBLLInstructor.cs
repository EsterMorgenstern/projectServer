using BLL.Models;

namespace BLL.Api
{
    public interface IBLLInstructor
    {
        List<BLLInstructor> Get();
        void Create(BLLInstructor student);
        public BLLInstructor GetById(int id);
        public void Delete(BLLInstructor student);
        public void Update(BLLInstructor student);
    }
}
