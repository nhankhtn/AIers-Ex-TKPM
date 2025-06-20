using Microsoft.AspNetCore.Mvc;
using StudentManagement.BLL.Services.AddressService;
using StudentManagement.BLL.Services.EmailService;
using StudentManagement.BLL.Services.FacultyService;
using StudentManagement.BLL.Services.ProgramService;
using StudentManagement.BLL.Services.StudentService;
using StudentManagement.BLL.Services.StudentStatusService;
using StudentManagement.DAL.Data.Repositories.FacultyRepo;
using StudentManagement.DAL.Data.Repositories.ProgramRepo;
using StudentManagement.DAL.Data.Repositories.SettingRepository;
using StudentManagement.DAL.Data.Repositories.StudentRepo;
using StudentManagement.DAL.Data.Repositories.StudentStatusRepo;
using StudentManagement.DAL.Data;
using StudentManagement.Domain.Models;
using StudentManagement.BLL.Validators;
using Microsoft.EntityFrameworkCore;
using StudentManagement.BLL.Checker;
using StudentManagement.BLL.Services.CourseService;
using StudentManagement.DAL.Data.Repositories.CourseRepo;
using StudentManagement.DAL.Data.Repositories.ClassRepo;
using StudentManagement.DAL.Data.Repositories.ClassStudentRepo;
using StudentManagement.DAL.Data.Repositories.RegisterCancellationHistoryRepo;
using StudentManagement.BLL.Services.ClassService;
using StudentManagement.BLL.Services.ClassStudentService;
using StudentManagement.BLL.Mapping;

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


            builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

            //add http client
            builder.Services.AddHttpClient<IAddressService, AddressService>();

            // Services

            builder.Services.AddScoped<IProgramService, ProgramService>();
            builder.Services.AddScoped<IFacultyService, FacultyService>();
            builder.Services.AddScoped<IStudentStatusService, StudentStatusService>();
            builder.Services.AddScoped<IStudentService, StudentService>();
            builder.Services.AddScoped<IAddressService, AddressService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<ICourseService, CourseService>();
            builder.Services.AddScoped<IClassService, ClassService>();
            builder.Services.AddScoped<IClassStudentService, ClassStudentService>();
            builder.Services.AddScoped<IRegisterCancellationHistoryRepository, RegisterCancellationHistoryRepository>();


            // Repo
            builder.Services.AddScoped<IStudentRepository, StudentRepository>();
            builder.Services.AddScoped<IProgramRepository, ProgramRepository>();
            builder.Services.AddScoped<IFacultyRepository, FacultyRepository>();
            builder.Services.AddScoped<IStudentStatusRepository, StudentStatusRepository>();
            builder.Services.AddScoped<ISettingRepository, SettingRepository>();
            builder.Services.AddScoped<ICourseRepository, CourseRepository>();
            builder.Services.AddScoped<IClassRepository, ClassRepositry>();
            builder.Services.AddScoped<IClassStudentRepository, ClassStudentRepository>();
            builder.Services.AddScoped<IRegisterCancellationHistoryRepository, RegisterCancellationHistoryRepository>();

            // Validators
            builder.Services.AddScoped<IStudentValidator, StudentValidator>();
            builder.Services.AddScoped<IStudentChecker, StudentChecker>();


            builder.Services.AddControllers();
            builder.Services.AddOpenApiServices();

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            builder.Services.AddCorsServices();
        }
    }
}
