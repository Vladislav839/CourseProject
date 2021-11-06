using CourseProject.Data;
using CourseProject.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject.BusinessLogic.Interfaces
{
    public interface IUsersService
    {
        User GetUserById(string id);
        List<User> GetAll();

        Task AddUser(User user);
        User GetUserByUserName(string name);
        Task UpdateUser(User newUser);

        Task DeleteUser(string id);
    }
}
