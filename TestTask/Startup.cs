using Mapster;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestTask.DTOs;
using TestTask.Models;

namespace TestTask
{
    public class Startup
    {
        private IConfigurationRoot _config;
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            var ConfigBuilder = new ConfigurationBuilder().SetBasePath(env.ContentRootPath)
                        .AddJsonFile("appsettings.json");
            _config = ConfigBuilder.Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DbContext>(options =>
                options.UseSqlServer(connection));
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            TypeAdapterConfig<Product, ProductDto>
                .NewConfig()
                .Map(dest => dest.ProductName, src => src.ProductName)
                .Map(dest => dest.Url, src => src.Url)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.Price, src => src.Price)
                .Map(dest => dest.Weight, src => src.Weight)
                .Map(dest => dest.CategoryName, src => src.Category.CategoryName)
                .IgnoreNullValues(true);

            TypeAdapterConfig<ProductParameter, ProductParameterDto>
                .NewConfig()
                .Map(dest => dest.Name, src => src.CustomParameter.Name)
                .Map(dest => dest.Value, src => src.Value)
                .IgnoreNullValues(true);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
