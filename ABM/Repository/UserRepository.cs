using ABM.Interfaces;
using ABM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABM.Repository
{
    public class UserRepository : IUserRepository, IDisposable
    {
        private AmulenEntities _context;
        private bool _disposed = false;

        public UserRepository(AmulenEntities context)
        {
            this._context = context;
        }
        /// <summary>
        /// Soft deletes an user
        /// </summary>
        /// <param name="userId">Id from user</param>
        public void DeleteUser(int userId)
        {
            User user = _context.User.FirstOrDefault(x => x.id == userId);
            user.isActive = false;
            _context.Entry(user).State = System.Data.Entity.EntityState.Modified;
        }
        /// <summary>
        /// Gets an active user by Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>User if found, else null</returns>
        public User GetUserById(int userId)
        {
            return _context.User.Where(x => x.isActive == true).FirstOrDefault(x => x.id == userId);
        }

        /// <summary>
        /// Gets an IEnumerable of active users
        /// </summary>
        /// <returns></returns>
        public IEnumerable<User> GetUsers()
        {
            return _context.User.Where(x => x.isActive == true);
        }
        /// <summary>
        /// Inserts user
        /// </summary>
        /// <param name="user"></param>
        public void InsertUser(User user)
        {
            _context.User.Add(user);
        }
        /// <summary>
        /// Updates user
        /// </summary>
        /// <param name="user"></param>
        public void UpdateUser(User user)
        {
            _context.Entry(user).State = System.Data.Entity.EntityState.Modified;
        }

        /// <summary>
        /// Given an username and password, returns the user that corresponds, if it exists.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pass">Decoded password.</param>
        /// <returns>Return found user</returns>
        public User GetUserByLogin(string username, string pass)
        {
            return (User)_context.User.Where(x => x.isActive == true)
                                      .Where(x => (x.username.Equals(username)) && (x.pass.Equals(pass)) );
        }

        /// <summary>
        /// Saves changes in the database
        /// </summary>
        public void Save()
        {
            _context.SaveChanges();
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
                    _context.Dispose();
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