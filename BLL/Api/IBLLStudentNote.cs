using BLL.Models;

namespace BLL.Api
{
    public interface IBLLStudentNote
    {
        List<BLLStudentNote> Get();
        void Create(BLLStudentNote studentNote);
        public List<BLLStudentNote> GetById(int id);
        public void Delete(int id);
        public void Update(BLLStudentNote studentNote);
    }

}