using ABM.Interfaces;
using ABM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABM.Repository
{
    public class ProjectRepository : IProjectRepository, IDisposable
    {
        private readonly UnitOfWork unitOfWork = new UnitOfWork();

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Proyect GetById(int id)
        {
            var project = unitOfWork.ProjectRepository.GetByID(id);
            return project;
        }

        public IEnumerable<Proyect> GetActiveProjects()
        {
            var activeProjects = from p in unitOfWork.ProjectRepository.Get()
                                 where p.StateId == 1
                                 select p;
            return activeProjects;
        }
        public void InsertProject(Proyect model)
        {
            try
            {
                unitOfWork.ProjectRepository.Insert(model);
                unitOfWork.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void UpdateProject(Proyect model)
        {
            unitOfWork.ProjectRepository.Update(model);
            unitOfWork.Save();
        }

        public void DeleteProject(int id)
        {
            Proyect p = unitOfWork.ProjectRepository.GetByID(id);
            p.StateId = 2;
            unitOfWork.ProjectRepository.Update(p);
            unitOfWork.Save();
        }
    }
}