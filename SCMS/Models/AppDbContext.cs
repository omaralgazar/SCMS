using Microsoft.EntityFrameworkCore;

namespace SCMS.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Patient> Patients { get; set; } = null!;
        public DbSet<Staff> Staff { get; set; } = null!;
        public DbSet<Doctor> Doctors { get; set; } = null!;
        public DbSet<Radiologist> Radiologists { get; set; } = null!;
        public DbSet<Receptionist> Receptionists { get; set; } = null!;
        public DbSet<Appointment> Appointments { get; set; } = null!;
        public DbSet<AppointmentBooking> AppointmentBookings { get; set; } = null!;
        public DbSet<Invoice> Invoices { get; set; } = null!;
        public DbSet<Prescription> Prescriptions { get; set; } = null!;
        public DbSet<MedicalRecord> MedicalRecords { get; set; } = null!;
        public DbSet<Feedback> Feedbacks { get; set; } = null!;
        public DbSet<RadiologyRequest> RadiologyRequests { get; set; } = null!;
        public DbSet<RadiologyResult> RadiologyResults { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Patient)
                .WithOne(p => p.User)
                .HasForeignKey<Patient>(p => p.UserId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Staff)
                .WithOne(s => s.User)
                .HasForeignKey<Staff>(s => s.UserId);

            modelBuilder.Entity<Staff>()
                .HasOne(s => s.Doctor)
                .WithOne(d => d.Staff)
                .HasForeignKey<Doctor>(d => d.StaffId);

            modelBuilder.Entity<Staff>()
                .HasOne(s => s.Radiologist)
                .WithOne(r => r.Staff)
                .HasForeignKey<Radiologist>(r => r.StaffId);

            modelBuilder.Entity<Staff>()
                .HasOne(s => s.Receptionist)
                .WithOne(rc => rc.Staff)
                .HasForeignKey<Receptionist>(rc => rc.StaffId);

            modelBuilder.Entity<AppointmentBooking>()
                .HasOne(b => b.Appointment)
                .WithMany(a => a.Bookings)
                .HasForeignKey(b => b.AppointmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AppointmentBooking>()
                .HasOne(b => b.Patient)
                .WithMany(p => p.AppointmentBookings)
                .HasForeignKey(b => b.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.AppointmentBooking)
                .WithOne(b => b.Invoice)
                .HasForeignKey<Invoice>(i => i.BookingId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Patient)
                .WithMany(p => p.Feedbacks)
                .HasForeignKey(f => f.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Doctor)
                .WithMany(d => d.Feedbacks)
                .HasForeignKey(f => f.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Prescription>()
                .HasOne(p => p.Patient)
                .WithMany(pt => pt.Prescriptions)
                .HasForeignKey(p => p.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RadiologyRequest>()
                .HasOne(r => r.Patient)
                .WithMany(p => p.RadiologyRequests)
                .HasForeignKey(r => r.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RadiologyRequest>()
                .HasOne(r => r.Result)
                .WithOne(res => res.Request)
                .HasForeignKey<RadiologyResult>(res => res.RequestId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MedicalRecord>()
                .HasOne(m => m.Patient)
                .WithMany(p => p.MedicalRecords)
                .HasForeignKey(m => m.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MedicalRecord>()
                .HasOne(m => m.RadiologyResult)
                .WithMany(r => r.MedicalRecords)
                .HasForeignKey(m => m.RadiologyResultId)
                .OnDelete(DeleteBehavior.Restrict);

            foreach (var fk in modelBuilder.Model
                         .GetEntityTypes()
                         .SelectMany(e => e.GetForeignKeys()))
            {
                if (!fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade)
                {
                    fk.DeleteBehavior = DeleteBehavior.Restrict;
                }
            }
        }
    }
}
