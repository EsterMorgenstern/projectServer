using System.Diagnostics.CodeAnalysis;
using BLL.Api;
using BLL.Models;
using DAL.Api;
using DAL.Models;

namespace BLL.Services
{
    public class BLLStudentService : IBLLStudent
    {
        private readonly IDAL dal;
        public BLLStudentService(IDAL dal)
        {
            this.dal = dal;
        }
        /// <summary>
        /// הוספת תלמיד
        /// </summary>
        /// <param name="item"></param>
        public void Create(BLLStudent student)
        {
            Student p = new Student()
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Age = student.Age,
                City = student.City,
                School = student.School,
                Phone = student.Phone,
                SecondaryPhone = student.SecondaryPhone,
                Class = student.Class,
                Sector = student.Sector,
                LastActivityDate = DateOnly.FromDateTime(student.LastActivityDate),
                Status=student.Status,
                Email=student.Email,
                CreatedBy=student.CreatedBy,
                IdentityCard = student.IdentityCard,
                HealthFundId = student.HealthFundId,

            };
            dal.Students.Create(p);
        }

        /// <summary>
        /// get לתלמידים
        /// </summary>
        /// <returns>List  של התלמידים</returns>
        public List<BLLStudent> Get()
        {
            try
            {
                var pList = dal.Students.Get();
                if (pList == null || !pList.Any())
                {
                    Console.WriteLine("No students found.");
                    return new List<BLLStudent>(); // מחזיר מערך ריק
                }

                List<BLLStudent> list = new();
                pList.ForEach(p => list.Add(new BLLStudent()
                {
                    Id = p.Id,
                    FirstName = p.FirstName ?? "",
                    LastName = p.LastName ?? "",
                    Phone = p.Phone.ToString(),
                    SecondaryPhone = p.SecondaryPhone?.ToString() ?? "",
                    Age = p.Age,
                    City = p.City ?? "",
                    School = p.School ?? "",
                    Class = p.Class ?? "",
                    Sector = p.Sector ?? "",
                    LastActivityDate = p.LastActivityDate != null ? p.LastActivityDate.Value.ToDateTime(TimeOnly.MinValue) : DateTime.MinValue,
                    Status=p.Status?? "",
                    Email=p.Email ?? "",
                    CreatedBy=p.CreatedBy ?? "",
                    IdentityCard = p.IdentityCard ?? "",
                    HealthFundId = p.HealthFundId ,
                    HealthFundName = p.HealthFundForStudent != null ? p.HealthFundForStudent.Name : "",
                    HealthFundPlan = p.HealthFundForStudent != null ? p.HealthFundForStudent.FundType : ""
                }));
                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching students: {ex.Message}");
                return new List<BLLStudent>(); // מחזיר מערך ריק במקרה של שגיאה
            }
        }

        [return: NotNullIfNotNull("id")]
        public BLLStudent GetById(int id)
        {
            try
            {
                var p = dal.Students.GetById(id);
                if (p != null)
                {
                    return new BLLStudent()
                    {
                        Id = p.Id,
                        FirstName = p.FirstName ?? "",
                        LastName = p.LastName ?? "",
                        Phone = p.Phone.ToString(),
                        SecondaryPhone = p.SecondaryPhone?.ToString() ?? "",
                        Age = p.Age,
                        City = p.City ?? "",
                        School = p.School ?? "",
                        Class = p.Class ?? "",
                        Sector = p.Sector ?? "",
                        LastActivityDate = p.LastActivityDate != null ? p.LastActivityDate.Value.ToDateTime(TimeOnly.MinValue) : DateTime.MinValue,
                        Status=p.Status ?? "",
                        Email=p.Email ?? "",
                        CreatedBy=p.CreatedBy??"",
                        IdentityCard = p.IdentityCard ?? "",
                        HealthFundId = p.HealthFundId ,

                    };
                }

                Console.WriteLine($"Student with ID {id} not found.");
                return new BLLStudent()
                {
                    Id = id,
                    FirstName = "",
                    LastName = "",
                    Phone = "",
                    SecondaryPhone="",
                    Age = 0,
                    City = "",
                    School = "",
                    Class = "",
                    Sector = "",
                    LastActivityDate = DateTime.MinValue,
                    Status="",
                    Email="",
                    CreatedBy="",
                    IdentityCard = "",
                    HealthFundId = 0,

                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching student with ID {id}: {ex.Message}");
                return new BLLStudent()
                {
                    Id = id,
                    FirstName = "",
                    LastName = "",
                    Phone = "",
                    SecondaryPhone = "",
                    Age = 0,
                    City = "",
                    School = "",
                    Class = "",
                    Sector = "",
                    LastActivityDate = DateTime.MinValue,
                    Status="",
                    Email="",
                    CreatedBy="",
                    IdentityCard = "",
                    HealthFundId =0,

                };
            }
        }

        public async Task Delete(int id)
        {
            var attendances=await dal.Attendances.GetAttendanceByStudent(id);
           
            foreach (var item in attendances)
            {
                dal.Attendances.Delete(item.AttendanceId);
            }
           
            var notes = dal.StudentNotes.GetById(id);
            if (notes != null) { 
            foreach (var item in notes)
            {
                dal.StudentNotes.Delete(item.NoteId);
            }
            }
            var groupStudents=dal.GroupStudents.GetByStudentId(id);
            if (groupStudents != null) { 
            foreach (var item in groupStudents)
            {
                dal.GroupStudents.Delete(item);
                var group = dal.Groups.GetById(item.GroupId);
                group.MaxStudents = (group.MaxStudents ?? 0) + 1;
                dal.Groups.Update(group);

                var branch = dal.Branches.Get().ToList().Find(x => x.BranchId == group?.BranchId);
                if (branch != null)
                {
                    branch.MaxGroupSize = (branch.MaxGroupSize ?? 0) - 1;
                    dal.Branches.Update(branch);
                }
            }
            }


            dal.Students.Delete(id);
        }

        public void Update(BLLStudent student)
        {
            var m = dal.Students.GetById(student.Id);
            m.Id = student.Id;
            m.FirstName = student.FirstName;
            m.LastName = student.LastName;
            m.Phone = student.Phone; 
            m.SecondaryPhone = student.SecondaryPhone;
            m.Age = student.Age; 
            m.City = student.City;
            m.School = student.School;
            m.Class = student.Class;
            m.Sector = student.Sector;
            m.LastActivityDate = DateOnly.FromDateTime(student.LastActivityDate);
            m.Status = student.Status;
            m.Email = student.Email;
            m.CreatedBy = student.CreatedBy;
            m.IdentityCard = student.IdentityCard;
            m.HealthFundId = student.HealthFundId;


            dal.Students.Update(m);
        }
       
        public List<BLLStudentWithNotesDto> GetStudentsWithoutActiveGroupWithNotes()
        {
            var allStudents = dal.Students.Get();
            var allGroupStudents = dal.GroupStudents.Get();

            var result = new List<BLLStudentWithNotesDto>();

            foreach (var student in allStudents)
            {
                var groupLinks = allGroupStudents.Where(gs => gs.StudentId == student.Id).ToList();

                bool hasNoGroups = !groupLinks.Any();
                bool notActiveInAnyGroup = groupLinks.Any() && !groupLinks.Any(gs => gs.IsActive == 1);

                if (hasNoGroups || notActiveInAnyGroup)
                {
                    // שליפת הערות עבור התלמיד
                    var notesDal = dal.StudentNotes.GetById(student.Id);
                    var notes = notesDal?.Select(ToBLLStudentNote).ToList() ?? new List<BLLStudentNote>();

                    var dto = ToBLLStudentWithNotesDto(student, notes);
                    result.Add(dto);
                }
            }

            return result;
        }

        // פונקציית עזר להמרה
        private BLLStudentNote ToBLLStudentNote(StudentNote note)
        {
            return new BLLStudentNote
            {
                NoteId = note.NoteId,
                StudentId = note.StudentId,
                AuthorId = note.AuthorId,
                NoteContent = note.NoteContent ?? "",
                NoteType = note.NoteType ?? ""
            };
        }

        private BLLStudentWithNotesDto ToBLLStudentWithNotesDto(Student p, List<BLLStudentNote> notes)
        {
            return new BLLStudentWithNotesDto
            {
                Id = p.Id,
                FirstName = p.FirstName ?? "",
                LastName = p.LastName ?? "",
                Phone = p.Phone.ToString(),
                SecondaryPhone = p.SecondaryPhone?.ToString() ?? "",
                Age = p.Age,
                City = p.City ?? "",
                School = p.School ?? "",
                Class = p.Class ?? "",
                Sector = p.Sector ?? "",
                LastActivityDate = p.LastActivityDate != null ? p.LastActivityDate.Value.ToDateTime(TimeOnly.MinValue) : DateTime.MinValue,
                Status = p.Status ?? "",
                Email = p.Email ?? "",
                CreatedBy = p.CreatedBy ?? "",
                IdentityCard = p.IdentityCard ?? "",
                HealthFundId = p.HealthFundId,
                HealthFundName = p.HealthFundForStudent != null ? p.HealthFundForStudent.Name : "",
                HealthFundPlan = p.HealthFundForStudent != null ? p.HealthFundForStudent.FundType : "",
                Notes = notes
            };
        }



    }
}
