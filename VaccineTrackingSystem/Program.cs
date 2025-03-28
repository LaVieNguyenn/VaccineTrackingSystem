using VaccineTrackingSystem.Controllers;
using VaccineTrakingSystem.BLL.ServicesService;
using VaccineTrakingSystem.BLL.VaccineRecordService;
using VaccineTrakingSystem.BLL.VaccineScheduleService;
using VaccineTrakingSystem.DAL.DAOs;
using VaccineTrakingSystem.DAL.DAOs.ServicesDAO;
using VaccineTrakingSystem.DAL.DAOs.VaccineRecordDAO;
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

            builder.Services.AddScoped<IGenericDAO<VaccineSchedule>, VaccineSchedulesDAO>();
            builder.Services.AddScoped<IGenericRepository<VaccineSchedule>, GenericRepository<VaccineSchedule>>();
            builder.Services.AddScoped<IVaccineScheduleService,VaccineScheduleService>();

            builder.Services.AddScoped<IGenericDAO<VaccinationRecord>, VaccineRecordDAO>();
            builder.Services.AddScoped<IGenericRepository<VaccinationRecord>, GenericRepository<VaccinationRecord>>();
            builder.Services.AddScoped<IVaccineRecordService, VaccineRecordService>();
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
