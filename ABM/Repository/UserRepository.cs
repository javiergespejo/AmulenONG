using ABM.Interfaces;
using ABM.Models;
using ABM.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ABM.Repository
{
    public class UserRepository : GenericRepository<User>, IDisposable
    {
        private AmulenEntities _context;
        private bool _disposed = false;

        public UserRepository(AmulenEntities context) : base(context)
        {

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
                                      .Where(x => (x.username.Equals(username)) && (x.pass.Equals(pass)));
        }

        public void InsertUser(UserViewModel model)
        {
            User user = new User
            {
                name = model.Name,
                username = model.UserName,
                email = model.Email,
                pass = Encrypt.GetSHA256(model.Pass),
                typeUserId = 2,
                isActive = true
            };
            base.context.User.Add(user);
            Save();
        }

        public bool CheckMail(UserViewModel user)
        {
            var userMail = from u in GetActiveUsers()
                           where u.email == user.Email
                           select u;

            if (userMail.Count() == 1)
            {
                return true;
            }

            return false;
        }


        public bool CheckUserName(UserViewModel user)
        {
            var userName = from u in GetActiveUsers()
                           where u.username == user.UserName
                           select u;

            if (userName.Count() == 1)
            {
                return true;
            }

            return false;
        }


        public void DeleteUser(int userId)
        {
            User user = base.context.User.FirstOrDefault(x => x.id == userId);
            user.isActive = false;
            base.context.Entry(user).State = System.Data.Entity.EntityState.Modified;
            Save();
        }

        /// <summary>
        /// Saves changes in the database
        /// </summary>
        public void Save()
        {            
            base.context.SaveChanges();
        }

        public void UpdateUser(UserEditViewModel userViewModel)
        {
            var user = userViewModel.ToUserEntity();
            user.pass = string.Empty;
            base.context.Entry(user).State = EntityState.Modified;

            // Excludes properties to not modify.
            base.context.Entry(user).Property(x => x.typeUserId).IsModified = false;
            base.context.Entry(user).Property(x => x.isActive).IsModified = false;
            base.context.Entry(user).Property(x => x.pass).IsModified = false;
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




        partial class Encrypt
        {
            public static string GetSHA256(string str)
            {
                SHA256 sha256 = SHA256.Create();
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