using Microsoft.AspNetCore.Mvc;
using StudentManagement.BLL.Services.AddressService;
using StudentManagement.BLL.Services.EmailService;
using StudentManagement.BLL.Services.FacultyService;
using StudentManagement.BLL.Services.ProgramService;
using StudentManagement.BLL.Services.StudentService;
using StudentManagement.BLL.Services.StudentStatusService;
using StudentManagement.BLL;
using StudentManagement.DAL.Data.Repositories.FacultyRepo;
using StudentManagement.DAL.Data.Repositories.ProgramRepo;
using StudentManagement.DAL.Data.Repositories.SettingRepository;
using StudentManagement.DAL.Data.Repositories.StudentRepo;
using StudentManagement.DAL.Data.Repositories.StudentStatusRepo;
using StudentManagement.DAL.Data;
using StudentManagement.Domain.Models;
using StudentManagement.BLL.Validators;
using Microsoft.EntityFrameworkCore;

namespace StudentManagement.API.Startup
{
    public static class DepedenciesConfig
    {
        public static void AddDepedencies(this WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            Console.WriteLine(connectionString);

            builder.Services.AddScoped<List<AuditEntry>>();

            // Đăng ký AuditInterceptor và inject List<AuditEntry> vào nó
            builder.Services.AddScoped<AuditInterceptor>(sp =>
            {
                var auditEntries = sp.GetRequiredService<List<AuditEntry>>();
                return new AuditInterceptor(auditEntries);
            });

            // Đăng ký DbContext với AuditInterceptor lấy từ DI container
            builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            {
                var auditInterceptor = serviceProvider.GetRequiredService<AuditInterceptor>();
                options.UseSqlServer(connectionString).AddInterceptors(auditInterceptor);
            });




            builder.Services.AddAutoMapper(typeof(MappingProfile));

            //add http client
            builder.Services.AddHttpClient<IAddressService, AddressService>();

            // Services

            builder.Services.AddScoped<IProgramService, ProgramService>();
            builder.Services.AddScoped<IFacultyService, FacultyService>();
            builder.Services.AddScoped<IStudentStatusService, StudentStatusService>();
            builder.Services.AddScoped<IStudentService, StudentService>();
            builder.Services.AddScoped<IAddressService, AddressService>();
            builder.Services.AddScoped<IEmailService, EmailService>();

            // Repo
            builder.Services.AddScoped<IStudentRepository, StudentRepository>();
            builder.Services.AddScoped<IProgramRepository, ProgramRepository>();
            builder.Services.AddScoped<IFacultyRepository, FacultyRepository>();
            builder.Services.AddScoped<IStudentStatusRepository, StudentStatusRepository>();
            builder.Services.AddScoped<ISettingRepository, SettingRepository>();

            // Validators
            builder.Services.AddScoped<IUserValidator, UserValidator>();


            builder.Services.AddControllers();
            builder.Services.AddOpenApiServices();

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
        }
    }
}
