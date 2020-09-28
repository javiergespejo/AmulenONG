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

        public IEnumerable<Proyect> GetActiveProjects()
        {
            try
            {
                var activeProjects = from p in unitOfWork.ProjectRepository.Get()
                                     where p.StateId == 1
                                     select p;
                return activeProjects;
            }
            catch(Exception)
            {
                return Enumerable.Empty<Proyect>();
            }
            
        }
    }
}