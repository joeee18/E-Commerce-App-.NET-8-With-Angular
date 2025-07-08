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
            services.AddSingleton<IImageManagementService, ImageManagementService>();
            services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
            // Apply DbContext
            services.AddDbContext<AppDbContext>(op =>
            {
                op.UseSqlServer(Configuration.GetConnectionString("ECommerce"));
            });

            return services;
        }

    }
}
