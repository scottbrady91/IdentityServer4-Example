using System.Collections.Generic;
using IdentityServer4.Core.Models;

namespace ScottBrady91.IdentityServer4.Example.Config
{
    internal class Clients
    {
        public static IEnumerable<Client> Get()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "implicitClient",
                    ClientName = "Exaple Implicit Client Application",
                    RedirectUris = new List<string> {"https://localhost:44050/"},
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedScopes = new List<string>
                    {
                        StandardScopes.OpenId.Name,
                        StandardScopes.Profile.Name,
                        StandardScopes.Email.Name,
                        StandardScopes.Roles.Name
                    }
                },
                new Client
                {
                    ClientId = "clientCredentialsClient",
                    ClientName = "Example Client Credentials Client Application",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = new List<Secret> {new Secret("superSecretPassword".Sha256())},
                    AllowedScopes = new List<string> {"customAPI"}
                }
            };
        }
    }
}