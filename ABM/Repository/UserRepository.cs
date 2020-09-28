using ABM.Interfaces;
using ABM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
        /// Deletes user
        /// </summary>
        /// <param name="userId">Id from user</param>
        public void DeleteUser(int userId)
        {
            User user = _context.User.FirstOrDefault(x => x.id == userId);
            user.isActive = false;
            _context.Entry(user).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
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
            var checkUserName = from u in GetUsers()
                                where u.username == user.username || u.email == user.email
                                select u;
            if (checkUserName == null)
            {
                user.pass = Encrypt.GetSHA256(user.pass);
                user.typeUserId = 2;
                user.isActive = true;
                _context.User.Add(user);
                Save();
            }
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

        partial class Encrypt
        {
            public static string GetSHA256(string str)
            {
                SHA256 sha256 = SHA256Managed.Create();
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] stream;
                StringBuilder sb = new StringBuilder();
                stream = sha256.ComputeHash(encoding.GetBytes(str));
                for (int i = 0; i < stream.Length; i++)
                    sb.AppendFormat("{0:x2}", stream[i]);
                return sb.ToString();
            }
        }
    }
}