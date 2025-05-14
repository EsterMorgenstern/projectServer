using BLL.Models;

namespace BLL.Api
{
    public interface IBLLCourse
    {
        List<BLLCourse> Get();
        void Create(BLLCourse course);
        public BLLCourse GetById(int id);
        public void Delete(int courseId);
        public void Update(BLLCourse course);

    }
}
