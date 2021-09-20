using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WorldEvents.API.Data;
using WorldEvents.API.Services.CategoryService;
using WorldEvents.API.Services.ContinentService;
using WorldEvents.API.Services.CountryService;
using AutoMapper;
using WorldEvents.API.Services.EventService;
using WorldEvents.API.Helpers.DataShaper;
using WorldEvents.API.Models;
using WorldEvents.API.DTOs.Country;
using WorldEvents.API.DTOs.Category;
using WorldEvents.API.DTOs.Continent;
using WorldEvents.API.DTOs.Event;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace WorldEvents.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMvc();
            services.AddAutoMapper(typeof(Startup));
           
            services.AddDbContext<WorldEventsContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IContinentService, ContinentService>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IEventService, EventService>();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new Microsoft.OpenApi.Models.OpenApiInfo
                    {
                        Title = "World Events API",
                        Description = "Demo API for showing World events",
                        Version = "v1"
                    });
            });

            services.AddScoped<IDataShaper<GetCategoryDto>, DataShaper<GetCategoryDto>>();
            services.AddScoped<IDataShaper<GetContinentDto>, DataShaper<GetContinentDto>>();
            services.AddScoped<IDataShaper<GetCountryDto>, DataShaper<GetCountryDto>>();
            services.AddScoped<IDataShaper<GetEventDto>, DataShaper<GetEventDto>>();

            //
            services.AddCors();
            //

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseStaticFiles();

            app.UseCors(builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithExposedHeaders("X-Pagination");
                
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(options => {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "World Events API");
            });
        }
    }
}
