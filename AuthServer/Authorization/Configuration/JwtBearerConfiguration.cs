using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

namespace AuthServer.Authorization.Configuration
{
    public static class JwtBearerConfiguration
    {
        public static AuthenticationBuilder AddJwtBearerConfiguration(
            this AuthenticationBuilder builder, IConfiguration configuration)
        {
            IConfiguration _configuration = configuration;

            return builder.AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ClockSkew = new TimeSpan(0, 0, 30),
                    ValidateAudience = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],              
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                    ValidAudiences = new List<string>() { "User", "AFishkaAPI"}
                };
                options.Events = new JwtBearerEvents()
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";

                        if (string.IsNullOrEmpty(context.Error))
                            context.Error = "invalid_token";
                        if (string.IsNullOrEmpty(context.ErrorDescription))
                            context.ErrorDescription = "This request requires a valid JWT access token";

                        return context.Response.WriteAsJsonAsync(new
                        {
                            error = context.Error,
                            error_description = context.ErrorDescription
                        });
                    }
                };
            });
        }
    }
}
