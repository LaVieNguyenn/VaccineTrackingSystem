using Dapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using VaccineTrackingSystem.Controllers;
using VaccineTrakingSystem.BLL.CustomerService;
using VaccineTrakingSystem.BLL.CenterInfoService;
using VaccineTrakingSystem.BLL.Services.UserService;
using VaccineTrakingSystem.BLL.ServicesService;
using VaccineTrakingSystem.BLL.UsersService;
using VaccineTrakingSystem.BLL.VaccineRecordService;
using VaccineTrakingSystem.BLL.VaccineScheduleService;
using VaccineTrakingSystem.BLL.VaccineService;
using VaccineTrakingSystem.DAL.DAOs;
using VaccineTrakingSystem.DAL.DAOs.ChildDAO;
using VaccineTrakingSystem.DAL.DAOs.ServicesDAO;
using VaccineTrakingSystem.DAL.DAOs.UserDAO;
using VaccineTrakingSystem.DAL.DAOs.UsersDAO;
using VaccineTrakingSystem.DAL.DAOs.VaccineDAO;
using VaccineTrakingSystem.DAL.DAOs.VaccineRecordDAO;
using VaccineTrakingSystem.DAL.Helper;
using VaccineTrakingSystem.DAL.Models;
using VaccineTrakingSystem.DAL.Repositories;
using VaccineTrakingSystem.DAL.Repositories.CustomerReposity;
using VaccineTrakingSystem.DAL.Repositories.UserRepository;
using VaccineTrakingSystem.DAL.DAOs.FeedbackDAO;
using VaccineTrakingSystem.BLL.FeedbackService;
using Client.Controllers.V1.ThirdParty;
using Client.Logics.Commons.MomoLogics;
using VaccineTrakingSystem.BLL.PaymentService;
using VaccineTrakingSystem.DAL.DAOs.PaymentDAO;
using VaccineTrakingSystem.DAL.Repositories.PaymentRepository;
using VaccineTrakingSystem.BLL.Services;
using VaccineTrackingSystem.Hubs;
using VaccineTrakingSystem.DAL.DAOs.NotificationDAO;
using VaccineTrakingSystem.BLL.NotificationService;
using VaccineTrakingSystem.DAL.Repositories.NotificationRepository;
using VaccineTrakingSystem.DAL.DAOs.DashboardDao;
using VaccineTrakingSystem.DAL.Repositories.DashboardRepository;
using VaccineTrakingSystem.BLL.DashboardService;
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
            builder.Services.AddSignalR();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Authentication/Login";
        options.AccessDeniedPath = "/";
    });

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

            builder.Services.AddScoped<IGenericDAO<Vaccine>, VaccineDAO>();
            builder.Services.AddScoped<IGenericRepository<Vaccine>, GenericRepository<Vaccine>>();
            builder.Services.AddScoped<IVaccineService, VaccineService>();

            builder.Services.AddScoped<IGenericDAO<User>, UsersDAO>();
            builder.Services.AddScoped<IGenericRepository<User>, GenericRepository<User>>();
            builder.Services.AddScoped<IUsersService, UsersService>();

            builder.Services.AddScoped<IGenericDAO<CenterInfo>, CenterInfoDAO>();
            builder.Services.AddScoped<IGenericRepository<CenterInfo>, GenericRepository<CenterInfo>>();
            builder.Services.AddScoped<ICenterInfoService, CenterInfoService>();

            builder.Services.AddScoped<IGenericDAO<Child>, ChildDAO>();
            builder.Services.AddScoped<IGenericRepository<Child>, GenericRepository<Child>>();
            builder.Services.AddScoped<IChildService, ChildService>();

            builder.Services.AddScoped<IGenericDAO<Appointment>, AppointmentDAO>();
            builder.Services.AddScoped<IGenericRepository<Appointment>, GenericRepository<Appointment>>();
            builder.Services.AddScoped<IAppointmentService, AppointmentService>();

            //User
            builder.Services.AddScoped<IUserDAO, UserDAO>();
            builder.Services.AddScoped<IUserRepo, UserRepo>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IChildDAO, ChildDAO>();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
             //Feedback
            builder.Services.AddScoped<IFeedbackDAO, FeedbackDAO>();
            builder.Services.AddScoped<IFeedbackService, FeedbackService>();

            //Connect MOMO API (NOT CHANGE)
            builder.Services.Configure<MomoOptionModel>(builder.Configuration.GetSection("MomoAPI"));
            builder.Services.AddScoped<IMomoService, MomoService>();

            builder.Services.AddScoped<IPaymentDAO, PaymentDAO>();
            builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();

            builder.Services.AddScoped<INotificationDAO, NotificationDAO>();
            builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<NotificationController>();

            builder.Services.AddScoped<IDashboardDAO, DashboardDao>();
            builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
            builder.Services.AddScoped<IDashboardService, DashboardService>();

            SqlMapper.AddTypeHandler(new VaccineTrakingSystem.DAL.Helper.DateOnlyTypeHandler());


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
            app.MapHub<NotificationHub>("/notificationHub");

            app.Run();
        }
    }
}
