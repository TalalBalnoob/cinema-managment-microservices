using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Yarp.ReverseProxy;
using System.Text;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
		.AddJwtBearer(options => {
			options.TokenValidationParameters = new TokenValidationParameters {
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateLifetime = true,
				ValidateIssuerSigningKey = true,
				ValidIssuer = builder.Configuration["Jwt:Issuer"],
				ValidAudience = builder.Configuration["Jwt:Audience"],
				IssuerSigningKey = new SymmetricSecurityKey(
							Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
			};
		});

builder.Services.AddAuthorization();
builder.Services.AddReverseProxy()
		.LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

// app.MapWhen(ctx =>
// 		ctx.Request.Path.StartsWithSegments("/auth/register") ||
// 		ctx.Request.Path.StartsWithSegments("/auth/login"),
// 		branch =>
// 		{
// 			branch.UseRouting();
// 			branch.UseEndpoints(endpoints =>
// 			{
// 				endpoints.MapReverseProxy(); // no auth here
// 			});
// 		});

app.UseAuthentication();
app.UseAuthorization();
app.MapReverseProxy();
// .RequireAuthorization();

app.Run();
