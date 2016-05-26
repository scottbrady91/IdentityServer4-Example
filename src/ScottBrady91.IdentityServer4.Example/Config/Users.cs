using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4.Core.Services.InMemory;

namespace ScottBrady91.IdentityServer4.Example.Config
{
    internal class Users
    {
        public static List<InMemoryUser> Get()
        {
            return new List<InMemoryUser>
            {
                new InMemoryUser
                {
                    Subject = "5BE86359-073C-434B-AD2D-A3932222DABE",
                    Username = "scott@scottbrady91.com",
                    Password = "password",
                    Claims = new List<Claim>
                    {
                        new Claim(JwtClaimTypes.Email, "scott@scottbrady91.com"),
                        new Claim(JwtClaimTypes.Role, "Badmin")
                    }
                }
            };
        }
    }
}