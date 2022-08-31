using System;
using System.Reflection;
using AspNetCoreWorkshop.Api.Jobs;
using AspNetCoreWorkshop.Api.Jobs.GetJob;
using AspNetCoreWorkshop.Api.Jobs.GetJobs;
using AutoMapper;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AspNetCoreWorkshop.Api
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers()
                .AddFluentValidation(options =>
                {
                    options.RegisterValidatorsFromAssemblyContaining<Startup>();
                });

            services.AddAutoMapper(config =>
            {
                config.CreateMap<Job, GetJobsResponse>();
                config.CreateMap<Job, GetJobResponse>();
            });
            services.AddMediatR(typeof(Startup).Assembly);
            services.AddDbContext<WorkshopDbContext>(options =>
            {
                options.UseInMemoryDatabase("test");
            });
        }
        
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseAuthentication();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(builder =>
            {
                builder.MapControllers();
            });
        }
    }
}


