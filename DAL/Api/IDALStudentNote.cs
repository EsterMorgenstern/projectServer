﻿using DAL.Models;

namespace DAL.Api
{
    public interface IDALStudentNote
    {
        List<StudentNote> Get();
        void Create(StudentNote studentNote);
        List<StudentNote> GetById(int id);
        StudentNote GetByNoteId(int id);
        void Delete(int noteId);
        void Update(StudentNote studentNote);
    }
}
