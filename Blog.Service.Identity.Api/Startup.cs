using Autofac;
using Blog.Service.Identity.Api.Extensions;
using Blog.Service.Identity.Api.Services;
using Blog.Service.Identity.Domain.Role;
using Blog.Service.Identity.Domain.User;
using Blog.Service.Identity.Infrastructure.Contexts;
using IdentityServer4.AccessTokenValidation;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace Blog.Service.Identity.Api
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
            services.AddControllersWithViews();

            //services.AddSameSiteCookiePolicy();

            services.AddDbContext<ApplicationIdentityDbContext>(
                options => options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            //Add custom User and Role entity to ASP.NET Identity
            services.AddIdentity<User, Role>(options =>
            {
                //Disable password constraint
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            }).AddEntityFrameworkStores<ApplicationIdentityDbContext>()
                .AddDefaultTokenProviders();

            //services.AddSwagger();

            // Adds IdentityServer
            services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.IssuerUri = "http://localhost:5010"; // docker uri: http://blog.service.identity
            })
            .AddDeveloperSigningCredential()
            // this adds the operational data from DB (codes, tokens, consents)
            .AddOperationalStore(options =>
                    {
                        options.ConfigureDbContext = builder => builder.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"),
                             sql => sql.MigrationsAssembly("Blog.Service.Identity.Infrastructure"));
                        // this enables automatic token cleanup. this is optional.
                        options.EnableTokenCleanup = true;
                    })
            .AddInMemoryIdentityResources(Config.GetIdentityResources())
            .AddInMemoryApiResources(Config.GetApiResources())
            .AddInMemoryClients(Config.GetClients())
            .AddInMemoryApiScopes(Config.GetApiScopes())
            .AddAspNetIdentity<User>()
            .AddProfileService<ProfileService>();
            //.AddResourceOwnerValidator<ResourceOwnerPasswordValidatorService<User>>(); //here;

            // preserve OIDC state in cache (solves problems with AAD and URL lenghts)
            services.AddOidcStateDataFormatterCache("aad");

            services.AddAuthorization(options =>
            {
                options.AddPolicy("apiread", policy =>
                {
                    
                    policy.RequireClaim("scope","blogapi.read");
                });
                options.AddPolicy("apiwrite", policy =>
                {
                    policy.RequireClaim("scope","blogapi.write");
                });
            });

            //MassTransit new Config setting
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
               {
                   cfg.Host(new Uri("rabbitmq://rabbitmq"), h =>
                   {
                       h.Username("guest");
                       h.Password("guest");
                   });
               });
            });

            services.AddMassTransitHostedService();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new MediatorModule());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Strict }); // solve same site cookie issue
            }

            // uncomment if you want to support static files
            app.UseStaticFiles();

            app.UseAuthentication();// The missing line

            app.UseIdentityServer();

            app.UseHttpsRedirection();

            app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute().RequireAuthorization());

            AutoMigrate(app);
        }

        private void AutoMigrate(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<ApplicationIdentityDbContext>();

            context.Database.Migrate();
        }
    }
}
