using ABM.Interfaces;
using ABM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace ABM.Repository
{
    public class UserRepository : GenericRepository<User>, IDisposable
    {
       // private AmulenEntities _context;
        private bool _disposed = false;

        public UserRepository(AmulenEntities context): base (context)
        {
          
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public User GetUserById(int userId)
        {
            return base.context.User.Where(x => x.isActive == true).FirstOrDefault(x => x.id == userId);
        }

        /// <summary>
        /// Gets an IEnumerable of active users
        /// </summary>
        /// <returns></returns>

        public User GetUserByUserName(string userName)
        {
            return base.context.User.Where(x => x.isActive == true).FirstOrDefault(x => x.username == userName);
        }

        /// <summary>
        /// Gets an IEnumerable of active users
        /// </summary>
        /// <returns></returns>
        
        public IEnumerable<User> GetUsers()
        {
            return base.context.User.Where(x => x.isActive == true);
        }
        /// <summary>
        /// Inserts user
        /// </summary>
        /// <param name="user"></param>

        public void Save()
        {
            base.context.SaveChanges();
        }
        /// <summary>
        /// Disposes the database context
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    base.context.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}