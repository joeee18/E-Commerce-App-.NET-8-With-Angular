using ECom.Core.Interfaces;
using ECom.Infrasructure.Data;
using ECom.Infrasructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECom.Infrasructure.Repositories;
using ECom.Core.Services;
using ECom.Infrasructure.Service;
using Microsoft.Extensions.FileProviders;
using StackExchange.Redis;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using ECom.Core.Entites;
using Microsoft.AspNetCore.Identity;


namespace ECom.Infrasructure
{
    public static class InfrastructureRegistertion
    {
        public static IServiceCollection InfrastructureConfiguration(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            //services.AddScoped<ICategoryRepository,CategoryRepository>();
            //services.AddScoped<IProductRepository, ProductRepository>();
            //services.AddScoped<IPhotoRepository,PhotoRepository>();

            // Apply Unit Of Work
            services.AddScoped<IUnitOfWork, UintOfWork>();
            // Register Email Sender
            services.AddScoped<IEmailServices, EmailServices>();
            // register IOrderService
            services.AddScoped<IOrderService, OrderService>();
            // register Token
            services.AddScoped<IGenrationToken,GenrationToken>();
            //apply redis connection
            services.AddSingleton<IConnectionMultiplexer>(i => 
            {
                var config = ConfigurationOptions.Parse(Configuration.GetConnectionString("redis"));
                return ConnectionMultiplexer.Connect(config);
            });
            services.AddSingleton<IImageManagementService, ImageManagementService>();
            services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
            // Apply DbContext
            services.AddDbContext<AppDbContext>(op =>
            {
                op.UseSqlServer(Configuration.GetConnectionString("ECommerce"));
            });

            services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            services.AddAuthentication(op =>
            {
                op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                op.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddCookie(o => {
                o.Cookie.Name = "token";
                o.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                };
            }).AddJwtBearer(op =>
            {
                op.RequireHttpsMetadata = false;
                op.SaveToken = true;
                op.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Token:Secret"])),
                    ValidateIssuer = true,
                    ValidIssuer = Configuration["Token:Issuer"], 
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero,
                };
                op.Events = new JwtBearerEvents() 
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["token"];
                        return Task.CompletedTask;
                    }
                };
            });
            return services;
        }


    }
}
