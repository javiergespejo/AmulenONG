using ABM.Interfaces;
using ABM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABM.Repository
{
    public class UserRepository : GenericRepository<User>, IDisposable
    {
        private bool _disposed = false;

        public UserRepository(AmulenEntities context): base(context)
        {
        }
        
        public override User GetByID(object id)
        {
            return base.context.User.Where(x => x.isActive == true).FirstOrDefault(x => x.id == (int)id);
        }
        

        public IEnumerable<User> GetActiveUsers()
        {
            return base.context.User.Where(x => x.isActive == true);
        }
        

        /// <summary>
        /// Given an username and password, returns the user that corresponds, if it exists.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pass">Decoded password.</param>
        /// <returns>Return found user</returns>
        public User GetUserByLogin(string username, string pass)
        {
            return (User)base.context.User.Where(x => x.isActive == true)
                                      .Where(x => (x.username.Equals(username)) && (x.pass.Equals(pass)) );
        }

        /// <summary>
        /// Saves changes in the database
        /// </summary>
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