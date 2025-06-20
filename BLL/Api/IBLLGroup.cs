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
        List<BLLGroup> Get();
        void Create(BLLGroup group);
        public BLLGroup GetById(int id);
        public void Delete(int id);
        public void Update(BLLGroup group);
        public List<BLLGroup> GetGroupsByCourseId(int courseId);
        public List<BLLGroupStudentPerfect> GetStudentsByGroupId(int groupId);
        public List<BLLGroup> GetGroupsByInstructorId(int groupId);
    }
}
