using Autofac;
using Blog.Service.Identity.Api.Extensions;
using Blog.Service.Identity.Api.Services;
using Blog.Service.Identity.Domain.Role;
using Blog.Service.Identity.Domain.User;
using Blog.Service.Identity.Infrastructure.Contexts;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

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
            services.AddControllers();
            services.AddApiVersioning();

            services.AddDbContext<ApplicationIdentityDbContext>(
                options => options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddSwagger();

            // Adds IdentityServer
            services.AddIdentityServer(x =>
            {
                x.IssuerUri = "null";
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
            .AddProfileService<ProfileService>()
            .AddResourceOwnerValidator<ResourceOwnerPasswordValidatorService<User>>(); //here;

            //Configure test api in same project for identity server
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.Authority = "http://localhost:5010";
                o.Audience = "blogapi"; // APi Resource Name
                o.RequireHttpsMetadata = false;
                o.IncludeErrorDetails = true;
                o.MetadataAddress = "http://localhost:5010/.well-known/openid-configuration";
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
            //builder.RegisterModule(new DomainModule());
            // builder.RegisterModule(new ServiceModule());
            builder.RegisterModule(new MediatorModule());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();// The missing line

            app.UseIdentityServer();

            app.UseHttpsRedirection();

            app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            app.UseRouting();

            app.UseAuthorization();

            app.UseErrorHandlingMiddleware();

            app.UseSwaggerDoc();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

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
