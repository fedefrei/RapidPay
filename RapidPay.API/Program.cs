using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RapidPay.API.Services;
using RapidPay.Core;
using RapidPay.Core.Repositories;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Log.Logger = new LoggerConfiguration()
	.MinimumLevel.Debug()
	.WriteTo.Console()
	.CreateLogger();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setupAction =>
{
	var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

	setupAction.IncludeXmlComments(xmlCommentsFullPath);
});

builder.Host.UseSerilog();

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); //To avoid claims to be transformed 

builder.Services
	.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.Authority = builder.Configuration["IDP:URI"];
		options.Audience = "RapidPayAPI";
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuerSigningKey = true,
			ValidTypes = new[] { "at+jwt" },
			ClockSkew = TimeSpan.Zero
		};
	});

builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("ApiScope", policy =>
	{
		policy.RequireAuthenticatedUser();
		policy.RequireClaim("scope", "RapidPayAPI.FullAccess");
	});
});


builder.Services.AddSwaggerGen(options =>
{
	options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
	{
		Type = SecuritySchemeType.OAuth2,
		Flows = new OpenApiOAuthFlows
		{
			AuthorizationCode = new OpenApiOAuthFlow
			{
				AuthorizationUrl = new Uri($"{builder.Configuration["IDP:URI"]}/connect/authorize"),
				TokenUrl = new Uri($"{builder.Configuration["IDP:URI"]}/connect/token"),
				Scopes = new Dictionary<string, string>
				{
					{"RapidPayAPI.FullAccess", "API - full access"}
				}
			}
		}
	});

	options.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
			},
			new[] { "RapidPayAPI.FullAccess" }
		}
	});
});

builder.Services.AddDbContext<RapidPayContext>(dbContextOptions =>
	dbContextOptions.UseSqlServer(builder.Configuration["ConnectionStrings:DB"])
);

builder.Services.AddScoped<ICardRepository, CardRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<CardService>();
builder.Services.AddScoped<CardManagementService>();
builder.Services.AddSingleton<UFEService>();

var app = builder.Build();

await EnsureDb(app.Services, Log.Logger, builder.Configuration["ConnectionStrings:DB"]);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(options =>
	{
		options.OAuthUsePkce();
	});
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers().RequireAuthorization("ApiScope");

app.Run();

async Task EnsureDb(IServiceProvider services, Serilog.ILogger logger, string connectionString)
{
	using var db = services.CreateScope().ServiceProvider.GetRequiredService<RapidPayContext>();
	logger.Information("Ensuring database exists and is up to date at connection string '{connectionString}'", connectionString);
	await db.Database.MigrateAsync();
}
