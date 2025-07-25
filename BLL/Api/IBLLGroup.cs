﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Models;

namespace BLL.Api
{
    public interface IBLLGroup
    {
        List<BLLGroupDetailsPerfect> Get();
        void Create(BLLGroup group);
        public BLLGroup GetById(int id);
        public void Delete(int id);
        public void Update(BLLGroup group);
        public List<BLLGroup> GetGroupsByCourseId(int courseId);
        public List<BLLGroupDetails> GetGroupsByDayOfWeek(string dayOfWeek);

        public List<BLLGroupStudentPerfect> GetStudentsByGroupId(int groupId);
        public List<BLLGroupDetailsPerfect> GetGroupsByInstructorId(int instructorId);
        public BLLGroupDetailsPerfect FindBestGroupForStudent(int studentId);
       public List<BLLGroupDetailsPerfect> FindBestGroupsForStudent(int studentId, int maxResults = 5);
    }
}
