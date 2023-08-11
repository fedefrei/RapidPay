using Serilog;

namespace RapidPay.IDP;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        // uncomment if you want to add a UI
        builder.Services.AddRazorPages();

        builder.Services.AddIdentityServer(options =>
            {
                // https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/api_scopes#authorization-based-on-scopes
                options.EmitStaticAudienceClaim = true;
            })
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
			.AddInMemoryApiResources(Config.ApiResources)
			.AddInMemoryClients(Config.Clients(builder.Configuration))
            .AddTestUsers(TestUsers.Users);

		return builder.Build();
    }
    
    public static WebApplication ConfigurePipeline(this WebApplication app)
    { 
        app.UseSerilogRequestLogging();
    
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

		app.UseStaticFiles();
		app.UseRouting();
		app.UseIdentityServer();
		app.UseAuthorization();

		app.MapRazorPages()
			.RequireAuthorization();

		return app;
    }
}
