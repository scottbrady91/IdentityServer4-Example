using System.IO;
using System.Security.Cryptography.X509Certificates;
using IdentityServer4.Core.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ScottBrady91.IdentityServer4.Example.Config;

namespace ScottBrady91.IdentityServer4.Example
{
    public class Startup
    {
        private readonly IHostingEnvironment environment;

        public Startup(IHostingEnvironment env)
        {
            environment = env;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            var cert = new X509Certificate2(Path.Combine(environment.ContentRootPath, "idsrv3test.pfx"), "idsrv3test");

            services.AddIdentityServer(new IdentityServerOptions {SigningCertificate = cert})
                .AddInMemoryClients(Clients.Get())
                .AddInMemoryScopes(Scopes.Get())
                .AddInMemoryUsers(Users.Get());
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseIdentityServer();
        }
    }
}