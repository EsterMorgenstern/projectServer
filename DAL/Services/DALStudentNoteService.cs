using DAL.Api;
using DAL.Models;

namespace DAL.Services
{
    public class DALStudentNoteService : IDALStudentNote
    {
        dbcontext dbcontext;
        public DALStudentNoteService(dbcontext data)
        {
            dbcontext = data;
        }


        public void Create(StudentNote studentNote)
        {
            dbcontext.StudentNotes.Add(studentNote);
            dbcontext.SaveChanges();
        }

        public void Delete(int noteId)
        {
            throw new NotImplementedException();
        }

        public List<StudentNote> Get()
        {
            return dbcontext.StudentNotes.ToList();
        }

        public List<StudentNote> GetById(int id)
        {
            var sNote = dbcontext.StudentNotes.Where(x => x.StudentId == id).ToList();
            if (sNote == null)
            {
                throw new KeyNotFoundException($"Branch with ID {id} not found.");
            }
            return sNote;
        }

        public void Update(StudentNote studentNote)
        {
            throw new NotImplementedException();
        }
    }
}