using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Blog.Service.Identity.Api
{
    public class Config
    {
        private const string SHARED_SECRET = "TYYvyjrJ4GThugsYGwnyEh2m63Tr7yyPk6YVwBb6";

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile(),
            };
        }

        //Resources are something you want to protect with IdentityServer - either identity data of your users, or APIs.
        //Identity data:Identity information (aka claims) about a user, e.g. name or email address.
        //APIs: APIs resources represent functionality a client wants to invoke - typically modelled as Web APIs, but not necessarily.
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource(
                "blogapi",
                "Blog.Service.BlogApi",
                new[] { 
                    JwtClaimTypes.Subject, 
                    JwtClaimTypes.Email, 
                    JwtClaimTypes.Role, 
                    JwtClaimTypes.PreferredUserName
                })
                {
                    Scopes = {"blogapi.read", "blogapi.write"}
                }
            };
        }

        public static List<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("blogapi.read", "Read Access to BlogApi"),
                new ApiScope("blogapi.write", "Write Access to BlogApi")
            };
        }

        //IdentityServer needs to know what client applications are allowed to use it
        //Client Access Control List
        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                new Client {
                    RequireConsent = false,
                    ClientId = "blogapp",
                    ClientName = "blogapp",
                    ClientSecrets = { new Secret(SHARED_SECRET.Sha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials, // this grat type is used as blogapi with be owned client application.
                    AlwaysIncludeUserClaimsInIdToken = true,
                    //You should not use  GrantTypes.ResourceOwnerPassword for third party app access to your data. Instead use Authorization code
                    //AllowOfflineAccess = true,//Enables refresh token
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        //IdentityServerConstants.StandardScopes.OfflineAccess,
                        "blogapi.read", "blogapi.write"
                    },
                    AllowAccessTokensViaBrowser = true,
                    //AccessTokenLifetime = 3600
                }
            };
        }
    }
}
