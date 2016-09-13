using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ScottBrady91.IdentityServer4.Example.Configuration;
using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ScottBrady91.IdentityServer4.Example
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            const string connectionString = @"Data Source=(LocalDb)\MSSQLLocalDB;database=Test.IdentityServer4.EntityFramework;trusted_connection=yes;";
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            // ASP.NET Identity DbContext
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            // ASP.NET Identity Registrations
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentityServer()
                //.AddInMemoryStores()
                .AddOperationalStore(
                    builder => builder.UseSqlServer(connectionString, options => options.MigrationsAssembly(migrationsAssembly)))
                //.AddInMemoryClients(Clients.Get())
                //.AddInMemoryScopes(Scopes.Get())
                .AddConfigurationStore(
                    builder => builder.UseSqlServer(connectionString, options => options.MigrationsAssembly(migrationsAssembly)))
                //.AddInMemoryUsers(Users.Get())
                .AddAspNetIdentity<IdentityUser>()
                .SetTemporarySigningCredential();

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            app.UseDeveloperExceptionPage();

            app.UseIdentityServer();

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}