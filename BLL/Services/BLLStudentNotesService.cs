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
                AuthorName = studentNote.AuthorName=string.Empty,
                AuthorRole = studentNote.AuthorRole = string.Empty,
                CreatedDate = studentNote.CreatedDate,
                IsActive = studentNote.IsActive,
                IsPrivate = studentNote.IsPrivate,
                NoteContent = studentNote.NoteContent = string.Empty,
                NoteId = studentNote.NoteId,
                NoteType = studentNote.NoteType,
                Priority = studentNote.Priority,
                UpdatedDate = studentNote.UpdatedDate
            };
            dal.StudentNotes.Create(s);
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<BLLStudentNote> Get()
        {
            return dal.StudentNotes.Get().Select(b => new BLLStudentNote()
            {
                StudentId = b.StudentId,
                AuthorId = b.AuthorId,
                AuthorName = b.AuthorName,
                AuthorRole = b.AuthorRole,
                CreatedDate = (DateTime)b.CreatedDate,
                IsActive = b.IsActive,
                IsPrivate = b.IsPrivate,
                NoteContent = b.NoteContent,
                NoteId = b.NoteId,
                NoteType = b.NoteType,
                Priority = b.Priority,
                UpdatedDate = (DateTime)b.UpdatedDate

            }).ToList();
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

        public void Update(BLLStudentNote studentNote)
        {
            throw new NotImplementedException();
        }
    }
}
