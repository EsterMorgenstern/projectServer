using BLL.Api;
using BLL.Models;
using Microsoft.AspNetCore.Mvc;

namespace server.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentNotesController
    {
        IBLLStudentNote studentNote;
        public StudentNotesController(IBLL manager)
        {
            studentNote = manager.Notes;
        }
        [HttpGet("GetAll")]
        public List<BLLStudentNote> Get()
        {
            return studentNote.Get();
        }


        [HttpGet("getById/{id}")]
        public List<BLLStudentNote> GetById(int id)
        {
            return studentNote.GetById(id);
        }

        [HttpGet("getByUserId/{id}")]
        public List<BLLStudentNote> GetByUserId(int id)
        {
            return studentNote.GetByUserId(id);
        }
        [HttpGet("getByRegistrationTracking")]
        public List<BLLStudentNote> GetByRegistrationTracking()
        {
            return studentNote.GetByRegistrationTracking();
        }
        [HttpGet("getByPaymentsNotes")]
        public List<BLLStudentNote> GetByPaymentsNotes()
        {
            return studentNote.GetByPaymentsNotes();
        }

        [HttpPost("Add")]
        public void Create([FromBody] BLLStudentNote sNote)
        {
            studentNote.Create(sNote);
        }
        [HttpPut("Update")]
        public void Update(BLLStudentNote sNote)
        {
            studentNote.Update(sNote);
        }
        [HttpDelete("Delete")]
        public void Delete(int noteId)
        {
            studentNote.Delete(noteId);
        }
    }
}
