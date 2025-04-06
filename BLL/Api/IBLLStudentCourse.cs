﻿using BLL.Models;

namespace BLL.Api
{
    public interface IBLLStudentCourse
    {
        List<BLLStudentCourse> Get();
        void Create(BLLStudentCourse studentCourses);
        public BLLStudentCourse GetById(int id);
        public void Delete(BLLStudentCourse studentCourses);
        public void Update(BLLStudentCourse studentCourses);
    }
}
