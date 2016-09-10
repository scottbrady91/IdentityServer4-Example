using System.Collections.Generic;
using IdentityServer4.Models;
using IdentityServer4.Quickstart;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ScottBrady91.IdentityServer4.Example
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentityServer()
                .AddInMemoryStores()
                .SetTemporarySigningCredential()
                .AddInMemoryClients(new List<Client>())
                .AddInMemoryScopes(new List<Scope>())
                .AddInMemoryUsers(new List<InMemoryUser>());
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            app.UseDeveloperExceptionPage();

            app.UseIdentityServer();
        }
    }
}