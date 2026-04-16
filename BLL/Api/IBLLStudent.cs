using BLL.Models;

namespace BLL.Api
{
    public interface IBLLStudent
    {
        List<BLLStudent> Get();
        Task CreateAsync(BLLStudent student);
        public BLLStudent GetById(int id);
        public Task Delete(int id);
        public void Update(BLLStudent student);
        public List<BLLStudentWithNotesDto> GetStudentsWithoutActiveGroupWithNotes();
    }
}
