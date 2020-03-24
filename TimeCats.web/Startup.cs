﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TimeCats.Services;

namespace TimeCats
{
    public class Startup
    {
        private readonly string LocalHostOrigins = "_localhostOrigin";
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => { 
                options.AddPolicy(LocalHostOrigins, builder => {
                    builder.AllowAnyOrigin().AllowAnyHeader();
                }); 
            });
            services.AddDbContext<TimeTrackerContext>(options =>
                options.UseNpgsql(Configuration["ConnectionString:TimeTrackerDB"]));
            services.AddMvc(options => { options.EnableEndpointRouting = false; });
            services.AddSession(options => { options.IdleTimeout = TimeSpan.FromHours(1); });
            services.AddScoped<StudentTimeTrackerService>();
            services.AddScoped<CourseService>();
            services.AddScoped<EvalService>();
            services.AddScoped<GroupService>();
            services.AddScoped<ProjectService>();
            services.AddScoped<TimeService>();
            services.AddScoped<UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                //app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler("/Home/Error");

            app.UseCors(LocalHostOrigins);
            
            app.UseStaticFiles();
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}