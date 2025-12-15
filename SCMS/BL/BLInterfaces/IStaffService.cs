using System.Collections.Generic;
using SCMS.Models;

namespace SCMS.BL.BLInterfaces
{
    public interface IStaffService
    {
        Staff Create(Staff staff);
        bool Update(Staff staff);
        bool Delete(int staffId);

        Staff? GetById(int staffId);
        List<Staff> GetAll();
        List<Staff> GetByDepartment(string departmentName);
    }
}
