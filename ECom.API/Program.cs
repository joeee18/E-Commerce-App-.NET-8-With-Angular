using ECom.API.Middleware;
using ECom.Infrasructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
namespace ECom.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddCors(op =>
            {
                op.AddPolicy("CORSPolicy", builder =>
                {
                    builder.WithOrigins("http://Localhost:4200", "https://Localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });
            });

            // Add Cookie Policy Configuration
            builder.Services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.None; // Allow cross-site
                options.OnAppendCookie = cookieContext =>
                    cookieContext.CookieOptions.SameSite = SameSiteMode.None; // Override Strict
            });



            builder.Services.AddMemoryCache();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //
            builder.Services.InfrastructureConfiguration(builder.Configuration);

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
           
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            
            app.UseCors("CORSPolicy");
            app.UseCookiePolicy(); // Add Cookie Policy Middleware here

            app.UseMiddleware<ExceptionsMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();

            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();

            try
            {
                // This line allows self-signed certificates for development purposes
                System.Net.ServicePointManager.ServerCertificateValidationCallback = (s, c, h, e) => true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }



            app.MapControllers();

            app.Run();
            // No specific problem statement was provided. Please clarify your request or specify the issue to solve.
        }
    }
}

