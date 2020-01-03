using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApp.IdentityServer
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<ApiResource> GetAllApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("bankApi","Customer Api for Bank")
            };
        }

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId="1",
                    Username="Lokesh",
                    Password="password"
                },
                new TestUser
                {
                    SubjectId="2",
                    Username="Kajol",
                    Password="password"
                }
            };
        }


        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                //Client Credentials Grant Type
                new  Client
                {
                    ClientId="client",
                    AllowedGrantTypes=GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes={ "bankApi" }

                },

                //Resource Owner Password Grant Type
                new Client
                {
                    ClientId="ro.client",
                    AllowedGrantTypes=GrantTypes.ResourceOwnerPassword,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes={"bankApi" }

                },

                //Implicit Flow Grant Type

                new Client
                {
                    ClientId="mvc",
                    ClientName="MVC Client",
                    AllowedGrantTypes=GrantTypes.Implicit,
                    RedirectUris={ "http://localhost:5003/signin-oidc"},
                    PostLogoutRedirectUris={ "http://localhost:5003/signout-callback-oidc"},
                    AllowedScopes=new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }


                },

                new Client
                {
                    ClientId="swaggerApiUI",
                    ClientName="Swagger API UI",
                    AllowedGrantTypes= GrantTypes.Implicit,
                    RedirectUris={ "http://localhost:44304/swagger/oauth2-redirect.html"},
                    PostLogoutRedirectUris={ "http://localhost:44304/swagger"},

                    AllowedScopes={"bankApi" },
                    AllowAccessTokensViaBrowser=true


                }
            };
        }
    }
}
