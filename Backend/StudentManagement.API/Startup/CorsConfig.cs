using Microsoft.Extensions.Options;

namespace StudentManagement.API.Startup
{
    public static class CorsConfig
    {
        private const string ALLOWALLPOLICY = "AllowAll";
        public static void AddCorsServices(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(ALLOWALLPOLICY, builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
        }
        public static void ApplyCorsConfig(this WebApplication app)
        {
            app.UseCors(ALLOWALLPOLICY);
        }
    }
}
