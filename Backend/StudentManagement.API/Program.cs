
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.BLL;
using StudentManagement.BLL.Services.AddressService;
using StudentManagement.BLL.Services.FacultyService;
using StudentManagement.BLL.Services.ProgramService;
using StudentManagement.BLL.Services.StudentService;
using StudentManagement.BLL.Services.StudentStatusService;
using StudentManagement.DAL.Data;
using StudentManagement.DAL.Data.Repositories.FacultyRepo;
using StudentManagement.DAL.Data.Repositories.ProgramRepo;
using StudentManagement.DAL.Data.Repositories.StudentRepo;
using StudentManagement.DAL.Data.Repositories.StudentStatusRepo;
using StudentManagement.Domain.Models;

namespace StudentManagement.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            Console.WriteLine(connectionString);
            // Đăng ký danh sách AuditEntry theo phạm vi Scoped
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

            // Repo
            builder.Services.AddScoped<IStudentRepository, StudentRepository>();
            builder.Services.AddScoped<IProgramRepository, ProgramRepository>();
            builder.Services.AddScoped<IFacultyRepository, FacultyRepository>();
            builder.Services.AddScoped<IStudentStatusRepository, StudentStatusRepository>();


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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


            var app = builder.Build();
            app.UseCors("AllowAll");
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
