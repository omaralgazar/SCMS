using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using SCMS.BL;
using SCMS.BL.BLClasses;
using SCMS.BL.BLInterfaces;
using SCMS.Models;
using Microsoft.Extensions.DependencyInjection;

namespace SCMS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ================== DbContext ==================
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
            );

            // ================== Services ==================
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IStaffService, StaffService>();
            builder.Services.AddScoped<IDoctorService, DoctorService>();
            builder.Services.AddScoped<IReceptionService, ReceptionService>();
            builder.Services.AddScoped<IPatientService, PatientService>();
            builder.Services.AddScoped<IPatientProfileService, PatientProfileService>();
            builder.Services.AddScoped<IDoctorProfileService, DoctorProfileService>();
            builder.Services.AddScoped<IAppointmentService, AppointmentService>();
            builder.Services.AddScoped<IAppointmentBookingService, AppointmentBookingService>();
            builder.Services.AddScoped<IInvoiceService, InvoiceService>();
            builder.Services.AddScoped<IFeedbackService, FeedbackService>();
            builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();
            builder.Services.AddScoped<IMedicalRecordService, MedicalRecordService>();
            builder.Services.AddScoped<IRadiologyService, RadiologyService>();
            builder.Services.AddScoped<IAdminService, AdminService>();

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // ================== Middleware ==================
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}"
            );

            // ================== Seed Admin ==================
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                if (!context.Users.OfType<Admin>().Any())
                {
                    string password = "Admin@123";

                    // Hash password
                    byte[] salt = RandomNumberGenerator.GetBytes(16);
                    string hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password,
                        salt,
                        KeyDerivationPrf.HMACSHA256,
                        100000,
                        32
                    ));
                    string passwordHash = $"{Convert.ToBase64String(salt)}.{hash}";

                    var admin = new Admin
                    {
                        FullName = "System Admin",
                        Email = "admin@scms.com",
                        Username = "admin",
                        Phone = "01000000000",   // مهم جدًا
                        PasswordHash = passwordHash,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    };

                    context.Users.Add(admin);
                    context.SaveChanges();
                }
            }

            app.Run();
        }
    }
}
