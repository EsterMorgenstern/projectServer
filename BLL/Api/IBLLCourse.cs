using BLL.Models;

namespace BLL.Api
{
    public interface IBLLCourse
    {
        List<BLLCourse> Get();
        void Create(BLLCourse course);
        public BLLCourse GetById(int id);
        public void Delete(BLLCourse course);
        public void Update(BLLCourse course);

    }
}
