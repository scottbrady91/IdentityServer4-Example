using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace ScottBrady91.IdentityServer4.Example
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .UseUrls("http://localhost:5000")
                .UseIISIntegration()
                .UseKestrel()
                .Build();

            host.Run();
        }
    }
}