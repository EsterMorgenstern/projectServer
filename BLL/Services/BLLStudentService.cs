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

        public async Task CreateAsync(BLLStudent student)
        {
            Student p = new Student()
            {
                Id = student.Id,
                FirstName = student.FirstName,
                OfficialFirstName = string.IsNullOrWhiteSpace(student.OfficialFirstName) ? student.FirstName : student.OfficialFirstName,
                LastName = student.LastName,
                Age = student.Age,
                City = student.City,
                School = student.School,
                Phone = student.Phone,
                SecondaryPhone = student.SecondaryPhone,
                Class = student.Class,
                Sector = student.Sector,
                LastActivityDate = DateOnly.FromDateTime(student.LastActivityDate),
                Email = student.Email,
                CreatedBy = student.CreatedBy,
                IdentityCard = student.IdentityCard,
                HealthFundId = student.HealthFundId,
            };

            dal.Students.Create(p);

            if (student.HealthFundId != 0)
            {
                var existingBilling = await dal.StudentHealthFunds.GetActiveByStudentId(student.Id);

                if (existingBilling == null)
                {
                    await dal.StudentHealthFunds.Create(new StudentHealthFund
                    {
                        StudentId = student.Id,
                        HealthFundId = student.HealthFundId,
                        StartDate = DateTime.Now,
                        IsActive = true,
                        EndDate = null,
                        Notes = null,
                        ReferralFilePath = null,
                        CommitmentFilePath = null
                    });
                }
            }
        }

        public List<BLLStudent> Get()
        {
            try
            {
                var students = dal.Students.Get();
                if (students == null || !students.Any())
                    return new List<BLLStudent>();

                // שלוף את כל ההערות מסוג "מעקב רישום" בפעם אחת
                var regNotes = dal.StudentNotes.GetByRegistrationTracking();

                // קבץ לפי StudentId, קח את ההערה הראשונה (או null)
                var regNoteByStudent = regNotes
                    .GroupBy(n => n.StudentId)
                    .ToDictionary(g => g.Key, g => g.OrderBy(n => n.CreatedDate).FirstOrDefault());

                // שלוף את כל הקשרים לקבוצות
                var allGroupLinks = dal.GroupStudents.Get();
                var groupLinksByStudent = allGroupLinks
                    .GroupBy(gs => gs.StudentId)
                    .ToDictionary(g => g.Key, g => g.ToList());

                var list = new List<BLLStudent>();

                foreach (var p in students)
                {
                    groupLinksByStudent.TryGetValue(p.Id, out var studentGroupLinks);
                    regNoteByStudent.TryGetValue(p.Id, out var regNote);

                    list.Add(new BLLStudent()
                    {
                        Id = p.Id,
                        FirstName = p.FirstName ?? "",
                        OfficialFirstName = p.OfficialFirstName ?? p.FirstName ?? "",
                        LastName = p.LastName ?? "",
                        Phone = p.Phone.ToString(),
                        SecondaryPhone = p.SecondaryPhone?.ToString() ?? "",
                        Age = p.Age,
                        City = p.City ?? "",
                        School = p.School ?? "",
                        Class = p.Class ?? "",
                        Sector = p.Sector ?? "",
                        LastActivityDate = p.LastActivityDate != null
                            ? p.LastActivityDate.Value.ToDateTime(TimeOnly.MinValue)
                            : DateTime.MinValue,
                        Status = GetStudentStatus(studentGroupLinks),
                        Email = p.Email ?? "",
                        CreatedBy = p.CreatedBy ?? "",
                        IdentityCard = p.IdentityCard ?? "",
                        HealthFundId = p.HealthFundId,
                        HealthFundName = p.HealthFundForStudent != null ? p.HealthFundForStudent.Name : "",
                        HealthFundPlan = p.HealthFundForStudent != null ? p.HealthFundForStudent.FundType : "",
                        RegistrationTrackingDate = regNote?.CreatedDate // אם אין, יהיה null
                    });
                }

                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching students: {ex.Message}");
                return new List<BLLStudent>();
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
                        OfficialFirstName = p.OfficialFirstName ?? p.FirstName ?? "",
                        LastName = p.LastName ?? "",
                        Phone = p.Phone.ToString(),
                        SecondaryPhone = p.SecondaryPhone?.ToString() ?? "",
                        Age = p.Age,
                        City = p.City ?? "",
                        School = p.School ?? "",
                        Class = p.Class ?? "",
                        Sector = p.Sector ?? "",
                        LastActivityDate = p.LastActivityDate != null
                            ? p.LastActivityDate.Value.ToDateTime(TimeOnly.MinValue)
                            : DateTime.MinValue,
                        Status = GetStudentStatus(p.Id),
                        Email = p.Email ?? "",
                        CreatedBy = p.CreatedBy ?? "",
                        IdentityCard = p.IdentityCard ?? "",
                        HealthFundId = p.HealthFundId,
                    };
                }

                Console.WriteLine($"Student with ID {id} not found.");
                return new BLLStudent()
                {
                    Id = id,
                    FirstName = "",
                    OfficialFirstName = "",
                    LastName = "",
                    Phone = "",
                    SecondaryPhone = "",
                    Age = 0,
                    City = "",
                    School = "",
                    Class = "",
                    Sector = "",
                    LastActivityDate = DateTime.MinValue,
                    Status = "",
                    Email = "",
                    CreatedBy = "",
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
                    OfficialFirstName = "",
                    LastName = "",
                    Phone = "",
                    SecondaryPhone = "",
                    Age = 0,
                    City = "",
                    School = "",
                    Class = "",
                    Sector = "",
                    LastActivityDate = DateTime.MinValue,
                    Status = "",
                    Email = "",
                    CreatedBy = "",
                    IdentityCard = "",
                    HealthFundId = 0,
                };
            }
        }

        public async Task Delete(int id)
        {
            // מחיקת נוכחויות
            var attendances = await dal.Attendances.GetAttendanceByStudent(id);
            foreach (var item in attendances)
            {
                dal.Attendances.Delete(item.AttendanceId);
            }

            // מחיקת הערות
            var notes = dal.StudentNotes.GetById(id);
            if (notes != null)
            {
                foreach (var item in notes)
                {
                    dal.StudentNotes.Delete(item.NoteId);
                }
            }

            // מחיקת קשרי קבוצות
            var groupStudents = dal.GroupStudents.GetByStudentId(id);
            if (groupStudents != null)
            {
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

            // מחיקת ק חולים והתחייבויות
            var studentHealthFund = await dal.StudentHealthFunds.GetActiveByStudentId(id);

            if (studentHealthFund != null)
            {
                // מחיקת התחייבויות לכל קופת חולים
                var commitments = dal.HealthFundCommitments.GetByStudentHealthFundId(studentHealthFund.Id);
                if (commitments != null)
                {
                    foreach (var commitment in commitments)
                    {
                        await dal.HealthFundCommitments.Delete(commitment.Id);
                    }
                }
                await dal.StudentHealthFunds.Delete(studentHealthFund.Id);
            }

            // מחיקת התלמיד עצמו
            dal.Students.Delete(id);
        }

        public void Update(BLLStudent student)
        {
            var m = dal.Students.GetById(student.Id);
            m.Id = student.Id;
            m.FirstName = student.FirstName;
            m.OfficialFirstName = string.IsNullOrWhiteSpace(student.OfficialFirstName) ? student.FirstName : student.OfficialFirstName;
            m.LastName = student.LastName;
            m.Phone = student.Phone;
            m.SecondaryPhone = student.SecondaryPhone;
            m.Age = student.Age;
            m.City = student.City;
            m.School = student.School;
            m.Class = student.Class;
            m.Sector = student.Sector;
            m.LastActivityDate = DateOnly.FromDateTime(student.LastActivityDate);
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
                    var notesDal = dal.StudentNotes.GetById(student.Id);
                    var notes = notesDal?.Select(ToBLLStudentNote).ToList() ?? new List<BLLStudentNote>();

                    var dto = ToBLLStudentWithNotesDto(student, notes);
                    result.Add(dto);
                }
            }

            return result;
        }

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

        public BLLStudentWithNotesDto ToBLLStudentWithNotesDto(Student p, List<BLLStudentNote> notes)
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
                Status = GetStudentStatus(p.Id),
                Email = p.Email ?? "",
                CreatedBy = p.CreatedBy ?? "",
                IdentityCard = p.IdentityCard ?? "",
                HealthFundId = p.HealthFundId,
                HealthFundName = p.HealthFundForStudent != null ? p.HealthFundForStudent.Name : "",
                HealthFundPlan = p.HealthFundForStudent != null ? p.HealthFundForStudent.FundType : "",
                Notes = notes
            };
        }

        public string GetStudentStatus(int studentId)
        {
            var groupLinks = dal.GroupStudents.GetByStudentId(studentId)?.ToList();
            return GetStudentStatus(groupLinks);
        }

        /// <summary>
        /// overload מהיר לחישוב על נתונים שכבר נטענו
        /// </summary>
        /// <param name="groupLinks"></param>
        /// <returns></returns>
        private string GetStudentStatus(List<GroupStudent>? groupLinks)
        {
            if (groupLinks == null || !groupLinks.Any())
                return "ליד";

            if (groupLinks.Any(gs => gs.IsActive == 1))
                return "פעיל";

            if (groupLinks.Any(gs => gs.IsActive == 2))
                return "ליד";

            if (groupLinks.All(gs => gs.IsActive == 0))
                return "לא פעיל";

            return "לא ידוע";
        }


    }
}