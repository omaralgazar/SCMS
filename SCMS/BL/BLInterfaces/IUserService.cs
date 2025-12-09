using System.Collections.Generic;
using SCMS.Models;

namespace SCMS.BL.BLInterfaces
{
    public interface IUserService
    {
        User? GetById(int userId);
        User? GetByUsername(string username);
        List<User> GetAll();

        bool ActivateUser(int userId);
        bool DeactivateUser(int userId);

        bool ChangeRole(int userId, string newRole);
        bool UpdateBasicInfo(int userId, string fullName, string phone);
    }
}
