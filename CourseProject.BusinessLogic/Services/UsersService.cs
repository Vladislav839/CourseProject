using CourseProject.Data;
using CourseProject.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseProject.BusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseProject.BusinessLogic.Services
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationContext _context;

        public UsersService(ApplicationContext context)
        {
            _context = context;
        }
        public async Task AddUser(User user)
        {
            _context.Users.Add(user);
           await _context.SaveChangesAsync();
        }

        public async Task DeleteUser(string id)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public List<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public User GetUserById(string id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public User GetUserByUserName(string name)
        {
            return _context.Users.FirstOrDefault(u => u.UserName == name);
        }

        public async Task UpdateUser(User newUser)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == newUser.Id);

            user.UserName = newUser.UserName;
            user.Email = newUser.Email;
            user.AvatarUrl = newUser.AvatarUrl;

            await _context.SaveChangesAsync();
        }
    }
}
