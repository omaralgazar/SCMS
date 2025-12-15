using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SCMS.BL.BLInterfaces;
using SCMS.Models;

namespace SCMS.BL.BLClasses
{
    public class StaffService : IStaffService
    {
        private readonly AppDbContext _context;

        public StaffService(AppDbContext context)
        {
            _context = context;
        }

        public Staff Create(Staff staff)
        {
            _context.Staff.Add(staff);
            _context.SaveChanges();
            return staff;
        }

        public bool Update(Staff staff)
        {
            if (!_context.Staff.Any(s => s.UserId == staff.UserId))
                return false;

            _context.Staff.Update(staff);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int staffUserId)
        {
            var staff = _context.Staff.FirstOrDefault(s => s.UserId == staffUserId);
            if (staff == null) return false;

            bool hasRoles =
                _context.Doctors.Any(d => d.UserId == staffUserId) ||
                _context.Radiologists.Any(r => r.UserId == staffUserId) ||
                _context.Receptionists.Any(rc => rc.UserId == staffUserId) ||
                _context.Users.OfType<Admin>().Any(a => a.UserId == staffUserId);

            if (hasRoles) return false;

            _context.Staff.Remove(staff);
            _context.SaveChanges();
            return true;
        }

        public Staff? GetById(int staffUserId)
        {
            return _context.Staff.FirstOrDefault(s => s.UserId == staffUserId);
        }

        public List<Staff> GetAll()
        {
            return _context.Staff.ToList();
        }

        public List<Staff> GetByDepartment(string departmentName)
        {
            return _context.Staff
                .Where(s => s.DepartmentName == departmentName)
                .ToList();
        }
    }
}
