using System.Collections.Generic;
using IdentityServer4.Core.Services.InMemory;

namespace ScottBrady91.IdentityServer4.Example.Config
{
    internal class Users
    {
        public static List<InMemoryUser> Get()
        {
            return new List<InMemoryUser>();
        }
    }
}