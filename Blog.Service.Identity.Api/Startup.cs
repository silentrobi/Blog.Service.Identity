using Autofac;
using Blog.Service.Identity.Api.Extensions;
using Blog.Service.Identity.Api.Services;
using Blog.Service.Identity.Domain.Role;
using Blog.Service.Identity.Domain.User;
using Blog.Service.Identity.Infrastructure.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                // this adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore(options =>
                    {
                        options.ConfigureDbContext = builder => builder.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"),
                             sql => sql.MigrationsAssembly("Blog.Service.Identity.Infrastructure"));
                        // this enables automatic token cleanup. this is optional.
                        options.EnableTokenCleanup = true;
                        options.TokenCleanupInterval = 30; // interval in seconds
                    })
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients())
                .AddAspNetIdentity<User>();
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

            app.UseHttpsRedirection();

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
