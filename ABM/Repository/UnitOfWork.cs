using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ABM.Models;

namespace ABM.Repository
{
    public class UnitOfWork : IDisposable
    {
        private BlogEntities context = new BlogEntities();
        private GenericRepository<post> postRepository;
        private GenericRepository<category> categoryRepository;

        public GenericRepository<post> PostRepository
        {
            get
            {

                if (this.postRepository == null)
                {
                    this.postRepository = new GenericRepository<post>(context);
                }
                return postRepository;
            }
        }

        public GenericRepository<category> CategoryRepository
        {
            get
            {

                if (this.categoryRepository == null)
                {
                    this.categoryRepository = new GenericRepository<category>(context);
                }
                return categoryRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}