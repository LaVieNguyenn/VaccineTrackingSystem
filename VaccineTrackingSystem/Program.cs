using VaccineTrakingSystem.BLL.AppointmentService;
using VaccineTrakingSystem.BLL.ChildService;
using VaccineTrakingSystem.BLL.Services;
using VaccineTrakingSystem.BLL.ServicesService;
using VaccineTrakingSystem.BLL.VaccinationRecordService;
using VaccineTrakingSystem.DAL.DAOs;
using VaccineTrakingSystem.DAL.DAOs.ServicesDAO;
using VaccineTrakingSystem.DAL.Models;
using VaccineTrakingSystem.DAL.Repositories;

namespace VaccineTrackingSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddHttpContextAccessor();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //Service
            builder.Services.AddScoped<IGenericDAO<Service>, ServiceDAO>();
            builder.Services.AddScoped<IGenericRepository<Service>, GenericRepository<Service>>();
            builder.Services.AddScoped<IServicesService,  ServicesService>();

            builder.Services.AddScoped<IGenericDAO<Child>, ChildDAO>();
            builder.Services.AddScoped<IGenericRepository<Child>, GenericRepository<Child>>();
            builder.Services.AddScoped<IChildService, ChildService>();

            builder.Services.AddScoped<IGenericDAO<Appointment>, AppointmentDAO>();
            builder.Services.AddScoped<IGenericRepository<Appointment>, GenericRepository<Appointment>>();
            builder.Services.AddScoped<IAppointmentService, AppointmentService>();

            builder.Services.AddScoped<IGenericRepository<VaccinationRecord>, GenericRepository<VaccinationRecord>>();
            builder.Services.AddScoped<IVaccinationRecordService, VaccinationRecordService>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
