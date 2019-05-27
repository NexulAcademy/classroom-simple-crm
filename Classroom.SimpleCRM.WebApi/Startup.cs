using Classroom.SimpleCRM.WebApi.Services;
using Classroom.SimpleCRM.SqlDbServices;
using Classroom.SimpleCRM.WebApi.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IO;
using System.Text;
using NSwag.SwaggerGeneration.Processors.Security;
using NSwag;
using NSwag.AspNetCore;
using System.Collections.Generic;

namespace Classroom.SimpleCRM.WebApi
{
    public class Startup
    {
        private const string SecretKey = "sdkdhsHOQPdjspQNSHsjsSDQWJqzkpdnf";
        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

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

            var jwtOptions = Configuration.GetSection(nameof(JwtIssuerOptions));
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });
            var tokenValidationPrms = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtOptions[nameof(JwtIssuerOptions.Issuer)],
                ValidateAudience = true,
                ValidAudience = jwtOptions[nameof(JwtIssuerOptions.Audience)],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,
                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = jwtOptions[nameof(JwtIssuerOptions.Issuer)];
                configureOptions.TokenValidationParameters = tokenValidationPrms;
                configureOptions.SaveToken = true;
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiUser", policy => policy.RequireClaim(Constants.JwtClaimIdentifiers.Rol, Constants.JwtClaims.ApiAccess));
            });

            var identityBuilder = services.AddIdentityCore<CrmIdentityUser>(o => {
                //TODO: override any default password rules here.
            });
            identityBuilder = new IdentityBuilder(identityBuilder.UserType, typeof(IdentityRole<string>), identityBuilder.Services);
            identityBuilder.AddEntityFrameworkStores<CrmIdentityDbContext>();
            identityBuilder.AddRoleValidator<RoleValidator<IdentityRole<string>>>();
            identityBuilder.AddRoleManager<RoleManager<IdentityRole<string>>>();
            identityBuilder.AddSignInManager<SignInManager<CrmIdentityUser>>();
            identityBuilder.AddDefaultTokenProviders();

            // Add application services.
            services.AddSingleton<IJwtFactory, JwtFactory>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<ICustomerData, SqlCustomerData>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper, UrlHelper>(factory =>
            {
                var context = factory.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(context);
            });

            services.AddMvc();
            services.AddOpenApiDocument(options =>
            {
                options.DocumentName = "v1";
                options.Title = "Simple CRM";
                options.Version = "1.0";
                options.DocumentProcessors.Add(new SecurityDefinitionAppender("JWT token",
                    new List<string>(), //no scope names to add
                    new SwaggerSecurityScheme
                    {
                        Type = SwaggerSecuritySchemeType.ApiKey,
                        Name = "Authorization",
                        Description = "Type into the textbox: 'Bearer {your_JWT_token}'. You can get a JWT from endpoints: '/auth/register' or '/auth/login'",
                        In = SwaggerSecurityApiKeyLocation.Header
                    }
                ));
                options.OperationProcessors.Add(
                    new OperationSecurityScopeProcessor("JWT token"));
            });
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
            app.UseSwagger();
            app.UseSwaggerUi3(settings =>
            {
                var microsoftOptions = Configuration.GetSection(nameof(MicrosoftAuthSettings));
                settings.OAuth2Client = new OAuth2ClientSettings
                {
                    ClientId = microsoftOptions[nameof(MicrosoftAuthSettings.ClientId)],
                    ClientSecret = microsoftOptions[nameof(MicrosoftAuthSettings.ClientSecret)],
                    AppName = "Simple CRM",
                    Realm = "Nexul Academy"
                };
            });

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
