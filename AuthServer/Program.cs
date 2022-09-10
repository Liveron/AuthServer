using AuthServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using AuthServer.Models;
using AuthServer.Services;
using MediatR;
using System.Reflection;
using AuthServer.Data.Tokens;
using AuthServer.Authorization.Configuration;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddDbContext<AuthDbContext>(options =>
{
	options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection"));
});

builder.Services.AddAuthentication()
	.AddJwtBearerConfiguration(configuration);

builder.Services.AddIdentity<AppUser, IdentityRole<Guid>>(config =>
{
	config.Password.RequireNonAlphanumeric = false;
})
	.AddEntityFrameworkStores<AuthDbContext>()
	.AddDefaultTokenProviders();

builder.Services.AddSingleton<ITokenGenerator, TokenGenerator>();

builder.Services.AddControllers();

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddDbContext<TokensDbContext>(options =>
{
	options.UseNpgsql(configuration.GetConnectionString("DbTokensConnection"));
});
builder.Services.AddScoped<ITokensDbContext>(provider =>
	provider.GetService<TokensDbContext>());

var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
	try
	{
		var context = serviceProvider.GetRequiredService<AuthDbContext>();
		var tokensContext = serviceProvider.GetRequiredService<TokensDbContext>();
		TokensDbInitializer.Initialize(tokensContext);
		DbInitializer.Initialize(context);
	}
	catch (Exception exception)
	{

		var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
		logger.LogError(exception, "An error occured while app initialization");
	}
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
