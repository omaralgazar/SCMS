using System.Collections.Generic;
using System.Linq;
using SCMS.BL.BLInterfaces;
using SCMS.Models;

namespace SCMS.BL.BLClasses
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public User? GetById(int userId)
        {
            return _context.Users.FirstOrDefault(u => u.UserId == userId);
        }

        public User? GetByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }

        public List<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public bool ActivateUser(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null) return false;

            user.IsActive = true;
            _context.SaveChanges();
            return true;
        }

        public bool DeactivateUser(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null) return false;

            user.IsActive = false;
            _context.SaveChanges();
            return true;
        }

        public bool UpdateBasicInfo(int userId, string fullName, string phone)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null) return false;

            user.FullName = fullName;
            user.Phone = phone;
            _context.SaveChanges();
            return true;
        }
    }
}
