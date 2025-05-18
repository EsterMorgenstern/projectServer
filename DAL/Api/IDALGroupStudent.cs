using DAL.Models;

namespace DAL.Api
{
    public interface IDALGroupStudent
    {
        List<GroupStudent> Get();
        void Create(GroupStudent groupStudent);
        GroupStudent GetById(int id);
        List<GroupStudent> GetByStudentId(int id);
        void Delete(GroupStudent groupStudent);
        void Update(GroupStudent groupStudent);
    }
}
