﻿using System;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using sXb_service.EF;
using sXb_service.Helpers;
using sXb_service.Models;
using sXb_service.Repos;
using sXb_service.Repos.Interfaces;
using sXb_service.Services;

namespace sXb_service
{
    public class Startup
    {
        readonly string AllowAnywhere = "_AllowAnywhere";
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            var databaseConfig = Configuration.GetSection("Db").Get<DatabaseConfig>();
            var corsSection = Configuration.GetSection("Cors");
            var corsConfig = corsSection.Get<CorsConfig>();

            services.AddCors(options =>
           {
               options.AddPolicy(AllowAnywhere,
                   builder =>
                   {
                       builder.WithOrigins(corsConfig.AllDomains)
                       .AllowAnyHeader()
                       .WithExposedHeaders("*")
                       .AllowCredentials()
                       .AllowAnyMethod();
                   });
           });

            services.AddDbContext<TxtXContext>(options =>
              options.UseSqlServer(databaseConfig.Connection));



            services.AddIdentity<User, IdentityRole>(config =>
               { config.SignIn.RequireConfirmedEmail = true; }
            ).AddEntityFrameworkStores<TxtXContext>()
                .AddDefaultTokenProviders();

            services.AddAutoMapper(typeof(Startup));
            services.AddScoped<IListingRepo, ListingRepo>();
            services.AddScoped<IBookRepo, BookRepo>();
            services.AddScoped<IUserBookRepo, UserBookRepo>();
            services.AddScoped<IUserRepo, UserRepo>();


            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 6;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            services.AddAutoMapper(typeof(Startup));
            services.AddScoped<IListingRepo, ListingRepo>();
            services.AddScoped<IBookRepo, BookRepo>();
            services.AddScoped<IUserBookRepo, UserBookRepo>();
            services.AddScoped<IUserRepo, UserRepo>();

            services.Configure<IdentityOptions>(options =>
            {


                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.Cookie.Expiration = TimeSpan.FromDays(150);
                // If the LoginPath isn't set, ASP.NET Core defaults
                // the path to /Account/Login.
                options.LoginPath = "/Account/Login";
                // If the AccessDeniedPath isn't set, ASP.NET Core defaults
                // the path to /Account/AccessDenied.
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, TxtXContext context)
        {
            if (env.IsDevelopment())
            {
                DbInitializer.InitializeData(context);
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseHsts();
            }


            app.UseHttpsRedirection();

            app.UseCors(AllowAnywhere);

            app.UseAuthentication();

            app.UseMvc();

        }
    }
}