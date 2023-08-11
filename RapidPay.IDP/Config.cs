using Duende.IdentityServer.Models;

namespace RapidPay.IDP;

public static class Config
{
	private static readonly string RapidPayAPI = "RapidPayAPI";

	public static IEnumerable<IdentityResource> IdentityResources =>
		new IdentityResource[]
		{
			new IdentityResources.OpenId(),
			new IdentityResources.Profile(),
			new IdentityResources.Email()
		};

	public static IEnumerable<ApiResource> ApiResources =>
		new ApiResource[]
		{
			new ApiResource(RapidPayAPI, "RapidPay API") //On the 3rd parameter we can add claims that we need for the access token to call the API
			{
				Scopes = { $"{RapidPayAPI}.FullAccess" }
			}
		};

	public static IEnumerable<ApiScope> ApiScopes =>
		new ApiScope[]
		{
			new ApiScope($"{RapidPayAPI}.FullAccess", "RapidPay API - Full access")
		};

	public static IEnumerable<Client> Clients(IConfiguration configuration) =>
		new Client[]
			{ 
				// Swagger client
				new Client
				{
					ClientId = "APISwagger",
					ClientName = "Swagger UI for API",
					ClientSecrets = {new Secret(configuration["Clients:Swagger:ClientSecret"].Sha256())},

					AllowedGrantTypes = GrantTypes.Code,

					RedirectUris = { configuration["Clients:Swagger:RedirectUri"] },
					AllowedCorsOrigins = { configuration["Clients:Swagger:SwaggerURI"] },
					AllowedScopes = new List<string>
					{
						$"{RapidPayAPI}.FullAccess"
					}
				}
			};
}