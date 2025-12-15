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

        bool UpdateBasicInfo(int userId, string fullName, string phone);
    }
}
