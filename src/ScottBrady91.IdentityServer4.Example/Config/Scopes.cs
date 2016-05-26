using System.Collections.Generic;
using IdentityModel;
using IdentityServer4.Core.Models;

namespace ScottBrady91.IdentityServer4.Example.Config
{
    internal class Scopes
    {
        public static IEnumerable<Scope> Get()
        {
            return new List<Scope>
            {
                StandardScopes.OpenId,
                StandardScopes.Profile,
                StandardScopes.Email,
                StandardScopes.Roles,
                StandardScopes.OfflineAccess,
                new Scope
                {
                    Name = "customAPI",
                    DisplayName = "Custom API",
                    Description = "Custom API scope",
                    Type = ScopeType.Resource,
                    Claims = new List<ScopeClaim>
                    {
                        new ScopeClaim(JwtClaimTypes.Role)
                    },
                    ScopeSecrets =  new List<Secret>
                    {
                        new Secret("scopeSecret".Sha256())
                    }
                }
            };
        }
    }
}