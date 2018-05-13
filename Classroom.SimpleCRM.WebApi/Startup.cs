using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Classroom.SimpleCRM.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Classroom.SimpleCRM.SqlDbServices;
using System.IO;
using Classroom.SimpleCRM.WebApi.Auth;

namespace Classroom.SimpleCRM.WebApi
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
            services.AddDbContext<CrmDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            var dbAssemblyName = typeof(CrmIdentityDbContext).Namespace;
            services.AddDbContext<CrmIdentityDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions => sqlOptions.MigrationsAssembly(dbAssemblyName)
                ));

            var googleOptions = Configuration.GetSection(nameof(GoogleAuthSettings));
            services.Configure<GoogleAuthSettings>(options =>
            {
                options.ClientId = googleOptions[nameof(GoogleAuthSettings.ClientId)];
                options.ClientSecret = googleOptions[nameof(GoogleAuthSettings.ClientSecret)];
            });

            var microsoftOptions = Configuration.GetSection(nameof(MicrosoftAuthSettings));
            services.Configure<MicrosoftAuthSettings>(options =>
            {
                options.ClientId = microsoftOptions[nameof(MicrosoftAuthSettings.ClientId)];
                options.ClientSecret = microsoftOptions[nameof(MicrosoftAuthSettings.ClientSecret)];
            });

            services.AddIdentity<CrmIdentityUser, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<CrmIdentityDbContext>()
                .AddDefaultTokenProviders();
            services.AddAuthentication()
                .AddCookie(cfg => cfg.SlidingExpiration = true)
                .AddGoogle(options =>
                {
                    options.ClientId = googleOptions[nameof(GoogleAuthSettings.ClientId)];
                    options.ClientSecret = googleOptions[nameof(GoogleAuthSettings.ClientSecret)];
                })
                .AddMicrosoftAccount(options =>
                {
                    options.ClientId = microsoftOptions[nameof(MicrosoftAuthSettings.ClientId)];
                    options.ClientSecret = microsoftOptions[nameof(MicrosoftAuthSettings.ClientSecret)];
                });

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<ICustomerData, SqlCustomerData>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper, UrlHelper>(factory =>
            {
                var context = factory.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(context);
            });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
                        
            app.UseAuthentication();

            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404
                    && !Path.HasExtension(context.Request.Path.Value)
                    && !context.Request.Path.Value.StartsWith("/api/"))
                {
                    context.Request.Path = "/index.html";
                    await next();
                }
            });

            app.UseMvcWithDefaultRoute();

            app.UseFileServer();
        }
    }
}
