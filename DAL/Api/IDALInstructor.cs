﻿using DAL.Models;
namespace DAL.Api
{
    public interface IDALInstructor
    {
        List<Instructor> Get();
        void Create(Instructor instructor);
        Instructor GetById(int id);
        void Delete(int id);
        void Update(Instructor instructor);
    }
}
