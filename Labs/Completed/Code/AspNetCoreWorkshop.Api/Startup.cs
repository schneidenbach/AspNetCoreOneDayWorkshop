using System;
using System.Reflection;
using AspNetCoreWorkshop.Api.JobPhases.CreateJobPhaseForJob;
using AspNetCoreWorkshop.Api.JobPhases.GetJobPhasesForJob;
using AspNetCoreWorkshop.Api.Jobs;
using AspNetCoreWorkshop.Api.Jobs.CreateJob;
using AspNetCoreWorkshop.Api.Jobs.GetJob;
using AspNetCoreWorkshop.Api.Jobs.GetJobs;
using AspNetCoreWorkshop.Api.Jobs.UpdateJob;
using AspNetCoreWorkshop.Api.Security;
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
using NSwag;
using NSwag.Generation.Processors.Security;

namespace AspNetCoreWorkshop.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers()
                .AddFluentValidation(options => { options.RegisterValidatorsFromAssemblyContaining<Startup>(); });

            services.AddAutoMapper(config =>
            {
                config.CreateMap<JobPhase, GetJobPhasesForJobResponse>();
                config.CreateMap<CreateJobPhaseForJobRequest, JobPhase>()
                    .ForMember(m => m.Id, options => options.Ignore())
                    .ForMember(m => m.Job, options => options.Ignore());
                config.CreateMap<Job, GetJobsResponse>();
                config.CreateMap<Job, GetJobResponse>();
                config.CreateMap<Job, UpdateJobResponse>();
                config.CreateMap<CreateJobRequest, Job>()
                    .ForMember(m => m.Id, options => options.Ignore())
                    .ForMember(m => m.JobPhases, options => options.Ignore());
            });
            services.AddMediatR(typeof(Startup).Assembly);
            services.AddDbContext<WorkshopDbContext>(options => { options.UseInMemoryDatabase("test"); });
            services.AddJwtBearerAuthentication(Configuration);

            services.AddSwaggerDocument(options =>
            {
                options.DocumentName = "Project Management API";
                options.OperationProcessors.Add(new OperationSecurityScopeProcessor("jwt-token"));
                options.DocumentProcessors.Add(new SecurityDefinitionAppender(
                    "jwt-token", new[] {""}, new OpenApiSecurityScheme
                    {
                        Type = OpenApiSecuritySchemeType.ApiKey,
                        Name = "Authorization",
                        Description =
                            "Add a bearer token to your request. Use POST https://localhost:5001/security/generateToken to generate a token.",
                        In = OpenApiSecurityApiKeyLocation.Header
                    })
                );
            });
            services.AddOpenApiDocument();
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseEndpoints(builder =>
            {
                builder.MapControllers();
            });
        }
    }
}