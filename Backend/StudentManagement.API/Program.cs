
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.BLL;
using StudentManagement.BLL.Services.FacultyService;
using StudentManagement.BLL.Services.ProgramService;
using StudentManagement.BLL.Services.StudentService;
using StudentManagement.BLL.Services.StudentStatusService;
using StudentManagement.DAL.Data;
using StudentManagement.DAL.Data.Repositories.FacultyRepo;
using StudentManagement.DAL.Data.Repositories.ProgramRepo;
using StudentManagement.DAL.Data.Repositories.StudentRepo;
using StudentManagement.DAL.Data.Repositories.StudentStatusRepo;

namespace StudentManagement.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            builder.Services.AddAutoMapper(typeof(MappingProfile));

            // Services
            builder.Services.AddScoped<IStudentService, StudentService>();
            builder.Services.AddScoped<IProgramService, ProgramService>();
            builder.Services.AddScoped<IFacultyService, FacultyService>();
            builder.Services.AddScoped<IStudentStatusService, StudentStatusService>();

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
