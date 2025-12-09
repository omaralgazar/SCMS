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
            if (!_context.Staff.Any(s => s.StaffId == staff.StaffId))
                return false;

            _context.Staff.Update(staff);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int staffId)
        {
            var staff = _context.Staff.FirstOrDefault(s => s.StaffId == staffId);
            if (staff == null)
                return false;

            bool hasRoles =
                _context.Doctors.Any(d => d.StaffId == staffId) ||
                _context.Radiologists.Any(r => r.StaffId == staffId) ||
                _context.Receptionists.Any(rc => rc.StaffId == staffId);

            if (hasRoles)
                return false;

            _context.Staff.Remove(staff);
            _context.SaveChanges();
            return true;
        }

        public Staff? GetById(int staffId)
        {
            return _context.Staff
                .Include(s => s.User)
                .FirstOrDefault(s => s.StaffId == staffId);
        }

        public Staff? GetByUserId(int userId)
        {
            return _context.Staff
                .Include(s => s.User)
                .FirstOrDefault(s => s.UserId == userId);
        }

        public List<Staff> GetAll()
        {
            return _context.Staff
                .Include(s => s.User)
                .ToList();
        }

        public List<Staff> GetByDepartment(string departmentName)
        {
            return _context.Staff
                .Include(s => s.User)
                .Where(s => s.DepartmentName == departmentName)
                .ToList();
        }
    }
}
