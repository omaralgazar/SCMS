namespace SCMS.ViewModels
{
    public class AdminDashboard
    {
        public string AdminName { get; set; } = string.Empty;
        public int TotalUsers { get; set; }
        public int TotalDoctors { get; set; }
        public int TotalPatients { get; set; }
        public int TodayAppointmentsCount { get; set; }
        public List<UserSummaryVm> RecentUsers { get; set; } = new List<UserSummaryVm>();
    }

    public class User
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserType { get; set; } = string.Empty;
        public DateTime DateAdded { get; set; }
    }
}
