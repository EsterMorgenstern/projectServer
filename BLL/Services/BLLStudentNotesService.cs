using BLL.Api;
using BLL.Models;
using DAL.Api;
using DAL.Models;

namespace BLL.Services
{
    public class BLLStudentNoteService : IBLLStudentNote
    {
        private readonly IDAL dal;

        public BLLStudentNoteService(IDAL dal)
        {
            this.dal = dal;
        }

        public void Create(BLLStudentNote studentNote)
        {
            StudentNote s = new StudentNote()
            {
                StudentId = studentNote.StudentId,
                AuthorId = studentNote.AuthorId,
                AuthorName = studentNote.AuthorName ??string.Empty,
                AuthorRole = studentNote.AuthorRole ??string.Empty,
                CreatedDate = studentNote.CreatedDate,
                IsActive = studentNote.IsActive,
                IsPrivate = studentNote.IsPrivate,
                NoteContent = studentNote.NoteContent?? string.Empty     ,
                NoteId = studentNote.NoteId,
                NoteType = studentNote.NoteType,
                Priority = studentNote.Priority,
                UpdatedDate = studentNote.UpdatedDate
            };
            dal.StudentNotes.Create(s);
        }

        public void Delete(int id)
        {
            dal.StudentNotes.Delete(id);
        }

        public List<BLLStudentNote> Get()
        {
            try
            {
                var notes = dal.StudentNotes.Get();
                if (notes == null || !notes.Any())
                {
                    Console.WriteLine("No student notes found.");
                    return new List<BLLStudentNote>(); // מחזיר מערך ריק
                }

                return notes.Select(b => new BLLStudentNote
                {
                    StudentId = b.StudentId,
                    AuthorId = b.AuthorId,
                    AuthorName = b.AuthorName,
                    AuthorRole = b.AuthorRole,
                    CreatedDate = b.CreatedDate ?? DateTime.MinValue,
                    IsActive = b.IsActive,
                    IsPrivate = b.IsPrivate,
                    NoteContent = b.NoteContent,
                    NoteId = b.NoteId,
                    NoteType = b.NoteType,
                    Priority = b.Priority,
                    UpdatedDate = b.UpdatedDate ?? DateTime.MinValue
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching student notes: {ex.Message}");
                return new List<BLLStudentNote>(); // מחזיר מערך ריק במקרה של שגיאה
            }
        }

        public List<BLLStudentNote> GetById(int id)
        {
            List<StudentNote> b = dal.StudentNotes.GetById(id);
            List<BLLStudentNote> blc = new List<BLLStudentNote>();
            foreach (StudentNote s in b)
            {
                BLLStudentNote bl = new BLLStudentNote()
                {
                    StudentId = s.StudentId,
                    AuthorId = s.AuthorId,
                    AuthorName = s.AuthorName,
                    AuthorRole = s.AuthorRole,
                    CreatedDate = (DateTime)s.CreatedDate,
                    IsActive = s.IsActive,
                    IsPrivate = s.IsPrivate,
                    NoteContent = s.NoteContent,
                    NoteId = s.NoteId,
                    NoteType = s.NoteType,
                    Priority = s.Priority,
                    UpdatedDate = (DateTime)s.UpdatedDate
                };
                blc.Add(bl);
            }
            return blc;
        }
        public List<BLLStudentNote> GetByUserId(int userId)
        {
            List<StudentNote> b = dal.StudentNotes.GetByUserId(userId);
            List<BLLStudentNote> blc = new List<BLLStudentNote>();
            foreach (StudentNote s in b)
            {
                BLLStudentNote bl = new BLLStudentNote()
                {
                    StudentId = s.StudentId,
                    AuthorId = s.AuthorId,
                    AuthorName = s.AuthorName,
                    AuthorRole = s.AuthorRole,
                    CreatedDate = (DateTime)s.CreatedDate,
                    IsActive = s.IsActive,
                    IsPrivate = s.IsPrivate,
                    NoteContent = s.NoteContent,
                    NoteId = s.NoteId,
                    NoteType = s.NoteType,
                    Priority = s.Priority,
                    UpdatedDate = (DateTime)s.UpdatedDate
                };
                blc.Add(bl);
            }
            return blc;
        }
        public void Update(BLLStudentNote studentNote)
        {
            var m = dal.StudentNotes.GetByNoteId(studentNote.NoteId);
            m.StudentId = studentNote.StudentId;
            m.AuthorId = studentNote.AuthorId;
            m.AuthorName = studentNote.AuthorName??string.Empty;
            m.AuthorRole = studentNote.AuthorRole ??string.Empty;
            m.CreatedDate = (DateTime)studentNote.CreatedDate;
            m.IsActive = studentNote.IsActive;
            m.IsPrivate = studentNote.IsPrivate;
            m.NoteContent = studentNote.NoteContent ??string.Empty;
            m.NoteId = studentNote.NoteId;
            m.NoteType = studentNote.NoteType;
            m.Priority = studentNote.Priority;
            m.UpdatedDate = (DateTime)studentNote.UpdatedDate;


            dal.StudentNotes.Update(m);

        }
    }
}
