using ABM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABM.Interfaces
{
    public interface IUserRepository : IDisposable
    {
        IEnumerable<User> GetUsers();

        User GetUserById(int userId);

        void InsertUser(User user);

        void DeleteUser(int userId);

        void UpdateUser(User user);

        void Save();
    }
}
