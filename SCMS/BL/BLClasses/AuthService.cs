using System;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using SCMS.BL.BLInterfaces;
using SCMS.Models;

namespace SCMS.BL.BLClasses
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public User? Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null) return null;

            if (!user.IsActive) return null;

            return VerifyPassword(password, user.PasswordHash) ? user : null;
        }

        public User Register(User user, string password)
        {
            user.PasswordHash = HashPassword(password);
            user.CreatedAt = DateTime.UtcNow;
            user.IsActive = true;

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public bool UserExists(string username)
        {
            return _context.Users.Any(u => u.Username == username);
        }

        public string HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(16);

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password,
                salt,
                KeyDerivationPrf.HMACSHA256,
                100000,
                32
            ));

            return $"{Convert.ToBase64String(salt)}.{hashed}";
        }

        public bool VerifyPassword(string password, string hash)
        {
            if (string.IsNullOrWhiteSpace(hash)) return false;

            var parts = hash.Split('.');
            if (parts.Length != 2) return false;

            byte[] salt;
            try { salt = Convert.FromBase64String(parts[0]); }
            catch { return false; }

            var storedHash = parts[1];

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password,
                salt,
                KeyDerivationPrf.HMACSHA256,
                100000,
                32
            ));

            return hashed == storedHash;
        }
    }
}
