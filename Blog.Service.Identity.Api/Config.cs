using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Blog.Service.Identity.Api
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile(),
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api1", "api1")
                {
                    Scopes = {"api1"}
                }
            };
        }

        public static List<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope
                {
                    Name = "api1",
                    Emphasize=true,
                }
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                new Client {
                    RequireConsent = false,
                    ClientId = "postman",
                    ClientName = "Postman",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes =
                    {
                        "api1"
                    },
                    AllowAccessTokensViaBrowser = true,
                    AccessTokenLifetime = 3600
                }
            };
        }
    }
}
