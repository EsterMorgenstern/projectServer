using BLL.Api;
using BLL.Models;
using Microsoft.AspNetCore.Mvc;

namespace server.controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class LessonController : ControllerBase
    {
        IBLLLesson lessons;
        public LessonController(IBLL manager)
        {
            lessons = manager.Lessons;
        }
        [HttpPost("Add")]
        public void GenerateLessonsForGroup(BLLGroup group)
        {
            lessons.GenerateLessonsForGroup(group.GroupId, (DateOnly)group.StartDate, (int)group.NumOfLessons, group.DayOfWeek, (TimeOnly)group.Hour, group.InstructorId, "system");
        }
    }
}

