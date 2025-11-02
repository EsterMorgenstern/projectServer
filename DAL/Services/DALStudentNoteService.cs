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
            var note = dbcontext.StudentNotes.SingleOrDefault(x => x.NoteId == noteId);
            if (note == null)
            {
                throw new KeyNotFoundException($"StudentNote with ID {noteId} not found.");
            }

            dbcontext.StudentNotes.Remove(note);
            dbcontext.SaveChanges();
        }

        public List<StudentNote> Get()
        {
            try
            {
                if (dbcontext.StudentNotes == null || !dbcontext.StudentNotes.Any())
                {
                    throw new Exception("No StudentNote records found.");
                }

                return dbcontext.StudentNotes.ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while retrieving StudentNote records.", ex);
            }
        }


        public List<StudentNote> GetById(int id)
        {
            var sNote = dbcontext.StudentNotes.Where(x => x.StudentId == id).ToList();
            if (sNote == null)
            {
                throw new KeyNotFoundException($"StudentNote with ID {id} not found.");
            }
            return sNote;
        }

        public List<StudentNote> GetByUserId(int userId)
        {
            var sNote = dbcontext.StudentNotes.Where(x => x.AuthorId == userId).ToList();
            if (sNote == null)
            {
                throw new KeyNotFoundException($"StudentNote with userId {userId} not found.");
            }
            return sNote;
        }
        public List<StudentNote> GetByRegistrationTracking()
        {
            var sNote = dbcontext.StudentNotes.Where(x => x.NoteType == "מעקב רישום").ToList();
            if (sNote == null)
            {
                throw new KeyNotFoundException($"StudentNote with מעקב רישום not found.");
            }
            return sNote;
        }
        public List<StudentNote> GetByPaymentsNotes()
        {
            var sNote = dbcontext.StudentNotes.Where(x => x.NoteType == "הערת גביה").ToList();
            if (sNote == null)
            {
                throw new KeyNotFoundException($"StudentNote with מעקב רישום not found.");
            }
            return sNote;
        }

        public StudentNote GetByNoteId(int id)
        {
            var sNote = dbcontext.StudentNotes.SingleOrDefault(x => x.NoteId == id);
            if (sNote == null)
            {
                throw new KeyNotFoundException($"StudentNote with ID {id} not found.");
            }
            return sNote;


        }
        public void Update(StudentNote studentNote)
        {
            dbcontext.StudentNotes.Update(studentNote);
            dbcontext.SaveChanges();
        }

       
    }
}