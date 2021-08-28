using System.Linq;
using System.Reflection;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityProvider
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            // using local db (assumes Visual Studio has been installed)
            const string connectionString = @"Data Source=(LocalDb)\MSSQLLocalDB;database=Test.IdentityServer.EntityFramework;trusted_connection=yes;";
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<ApplicationDbContext>(builder =>
                builder.UseSqlServer(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            var identityServerBuilder = services.AddIdentityServer(options => options.KeyManagement.Enabled = true);
            
            /*identityServerBuilder.AddInMemoryClients(Clients.Get())
                .AddInMemoryIdentityResources(Resources.GetIdentityResources())
                .AddInMemoryApiResources(Resources.GetApiResources())
                .AddInMemoryApiScopes(Resources.GetApiScopes());*/

            identityServerBuilder.AddOperationalStore(options =>
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)))
                .AddConfigurationStore(options =>
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)));

            identityServerBuilder.AddAspNetIdentity<IdentityUser>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            InitializeDbTestData(app);
            
            app.UseStaticFiles();
            app.UseRouting();
	
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
        }
        
        /// <summary>
        /// A small bootstrapping method that will run EF migrations against the database
        /// and create your test data.
        /// </summary>
        private static void InitializeDbTestData(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
                serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>().Database.Migrate();
                serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

                if (!context.Clients.Any())
                {
                    foreach (var client in Clients.Get())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in Resources.GetIdentityResources())
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiScopes.Any())
                {
                    foreach (var scope in Resources.GetApiScopes())
                    {
                        context.ApiScopes.Add(scope.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in Resources.GetApiResources())
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                if (!userManager.Users.Any())
                {
                    foreach (var testUser in Users.Get())
                    {
                        var identityUser = new IdentityUser(testUser.Username)
                        {
                            Id = testUser.SubjectId
                        };

                        userManager.CreateAsync(identityUser, "Password123!").Wait();
                        userManager.AddClaimsAsync(identityUser, testUser.Claims.ToList()).Wait();
                    }
                }
            }
        }
    }
}
