using BLL.Api;
using BLL.Models;
using DAL.Api;
using DAL.Models;

namespace BLL.Services
{
    public class BLLGroupService : IBLLGroup
    {
        private readonly IDAL dal;
        public BLLGroupService(IDAL dal)
        {
            this.dal = dal;
        }
        public void Create(BLLGroup group)
        {
            Group g = new Group()
            {
                GroupId = group.GroupId,
                CourseId = group.CourseId,
                BranchId = group.BranchId,
                AgeRange = group.AgeRange,
                DayOfWeek = group.DayOfWeek,
                GroupName = group.GroupName,
                Hour = group.Hour,
                MaxStudents = group.MaxStudents,
                Sector = group.Sector,
                InstructorId = group.InstructorId,
                NumOfLessons = group.NumOfLessons,
                LessonsCompleted=group.LessonsCompleted,
                StartDate = group.StartDate

            };
            dal.Groups.Create(g);
        }

        public void Delete(int id)
        {
         var groupStudents = dal.GroupStudents.Get().Where(x => x.GroupId == id);
            foreach (var item in groupStudents)
            {
                dal.GroupStudents.Delete(item);
            }
            dal.Groups.Delete(id);
        }

       
        public List<BLLGroupDetailsPerfect> Get()
        {
            return dal.Groups.Get().Select(c => new BLLGroupDetailsPerfect()
            {
                GroupId = c.GroupId,
                CourseId = c.CourseId,
                AgeRange = c.AgeRange,
                BranchId = c.BranchId,
                DayOfWeek = c.DayOfWeek,
                GroupName = c.GroupName,
                Hour = c.Hour,
                InstructorId = c.InstructorId,
                MaxStudents = c.MaxStudents,
                Sector = c.Sector,
                NumOfLessons = c.NumOfLessons,
                LessonsCompleted = c.LessonsCompleted,
                StartDate = c.StartDate,
                BranchName= dal.Branches.GetById(  c.BranchId).Name,
                CourseName=dal.Courses.GetById(c.CourseId).CouresName

            }).ToList();
        }

        public BLLGroup GetById(int id)
        {
            Group group = dal.Groups.GetById(id);
            BLLGroup blg = new BLLGroup()
            {
                GroupId = group.GroupId,
                CourseId = group.CourseId,
                BranchId = group.BranchId,
                AgeRange = group.AgeRange,
                DayOfWeek = group.DayOfWeek,
                GroupName = group.GroupName,
                Hour = group.Hour,
                MaxStudents = group.MaxStudents,
                Sector = group.Sector,
                InstructorId = group.InstructorId,
                NumOfLessons = group.NumOfLessons,
                LessonsCompleted=group.LessonsCompleted,
                StartDate = group.StartDate
            };
            return blg;
        }

        public List<BLLGroup> GetGroupsByCourseId(int courseId)
        {
            List<Group> lg = dal.Groups.Get();
            List<BLLGroup> bls = new List<BLLGroup>();
            foreach (var group in lg)
            {
                if (group.CourseId == courseId)
                {
                    BLLGroup bl = new BLLGroup()
                    {
                        GroupId = group.GroupId,
                        CourseId = group.CourseId,
                        BranchId = group.BranchId,
                        AgeRange = group.AgeRange,
                        DayOfWeek = group.DayOfWeek,
                        GroupName = group.GroupName,
                        Hour = group.Hour,
                        MaxStudents = group.MaxStudents,
                        Sector = group.Sector,
                        InstructorId = group.InstructorId,
                        StartDate = group.StartDate,
                        NumOfLessons = group.NumOfLessons,
                        LessonsCompleted=group.LessonsCompleted
                    };
                    bls.Add(bl);
                }
            }
            return bls;

        }
        public List<BLLGroupDetails> GetGroupsByDayOfWeek(string dayOfWeek)
        {
            var groups = dal.Groups.GetGroupsByDayOfWeek(dayOfWeek);
            return groups.Select(g => new BLLGroupDetails
            {
                GroupId = g.GroupId,
                GroupName = g.GroupName,
                DayOfWeek = g.DayOfWeek,
                CourseName = dal.Courses.GetById(g.CourseId).CouresName,
                BranchName = dal.Branches.GetById(g.BranchId).Name,
                Hour = g.Hour,
                AgeRange = g.AgeRange,
                MaxStudents = g.MaxStudents,
                Sector = g.Sector,
                StartDate = g.StartDate,
                NumOfLessons = g.NumOfLessons,
                LessonsCompleted=g.LessonsCompleted
            }).ToList();
        }

        public List<BLLGroup> GetGroupsByInstructorId(int groupId)
        {
            throw new NotImplementedException();
        }

        public List<BLLGroupStudentPerfect> GetStudentsByGroupId(int groupId)
        {
            var lst = dal.Groups.GetStudentsByGroupId(groupId);
            List<BLLGroupStudentPerfect> lstgp = new List<BLLGroupStudentPerfect>();
            foreach (var item in lst)
            {
                var d = dal.Groups.GetById(item.GroupId);

                BLLGroupStudentPerfect gspl = new BLLGroupStudentPerfect()
                {
                    StudentId = item.StudentId,
                    StudentName = dal.Students.GetById(item.StudentId).FirstName + " " + dal.Students.GetById(item.StudentId).LastName,
                    EnrollmentDate = item.EnrollmentDate,
                    IsActive = item.IsActive,
                    DayOfWeek = d.DayOfWeek,
                    Hour = d.Hour,
                    GroupName = d.GroupName,
                    BranchName = dal.Branches.GetById(d.BranchId).Name,
                    InstructorName = dal.Instructors.GetById(d.InstructorId).FirstName + " " + dal.Instructors.GetById(d.InstructorId).LastName,
                    CourseName = dal.Courses.GetById(d.CourseId).CouresName
                };
                lstgp.Add(gspl);
            }
            return lstgp;
        }


        public void Update(BLLGroup group)
        {
            Group existingGroup = dal.Groups.GetById(group.GroupId);
            if (existingGroup == null)
            {
                throw new KeyNotFoundException($"Group with ID {group.GroupId} not found.");
            }
            existingGroup.GroupId = group.GroupId;
            existingGroup.CourseId = group.CourseId;
            existingGroup.BranchId = group.BranchId;
            existingGroup.AgeRange = group.AgeRange;
            existingGroup.DayOfWeek = group.DayOfWeek;
            existingGroup.GroupName = group.GroupName;
            existingGroup.Hour = group.Hour;
            existingGroup.MaxStudents = group.MaxStudents;
            existingGroup.Sector = group.Sector;
            existingGroup.InstructorId = group.InstructorId;
            existingGroup.StartDate = group.StartDate;
            existingGroup.NumOfLessons = group.NumOfLessons;
            existingGroup.LessonsCompleted = group.LessonsCompleted;

            dal.Groups.Update(existingGroup);
        }


    }
}
