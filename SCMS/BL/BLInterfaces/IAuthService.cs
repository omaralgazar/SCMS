using SCMS.Models;

namespace SCMS.BL.BLInterfaces
{
    public interface IAuthService
    {
        User? Login(string username, string password);
        User Register(User user, string password);
        bool UserExists(string username);
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
    }
}
