using MediatR;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using System.Data;
using Microsoft.EntityFrameworkCore;
using UserManagement.Application.DTO.User;
using UserManagement.Application;
using UserManagement.Domain.ConfigurationModels;
using UserManagement.Domain.Contracts.Persistence;
using UserManagement.Domain.Entities;
using UserManagement.Infrastructure.Persistence;
using UserManagement.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace UserManagement.API.Extensions;

public static class ServiceCollectionExtensions
{
	private const string CONFIG_CONNECTION_STRING = "DefaultConnection";

	public static WebApplicationBuilder AddDataBase(this WebApplicationBuilder builder)
	{
		string connectionString = builder.Configuration.GetConnectionString(CONFIG_CONNECTION_STRING) ?? string.Empty;

		builder.Services.AddDbContext<AppDbContext>(opts =>
			opts.UseMySQL(connectionString, b =>
			{
				b.EnableRetryOnFailure();
			})
		);
		return builder;
	}

	public static WebApplicationBuilder AddCorsPolicy(this WebApplicationBuilder builder)
	{
		builder.Services.AddCors(options =>
		{
			options.AddPolicy("CorsPolicy", builder =>
			builder.AllowAnyOrigin()
			.AllowAnyMethod()
			.AllowAnyHeader()
			.WithExposedHeaders("X-Pagination"));
		});
		return builder;
	}

	public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
	{
		builder.Services.AddControllers();
		builder.Services.AddAuthentication();

		builder.Services.AddAutoMapper(x => x.AddProfile(new MappingProfile()));
		builder.Services.AddMediatR(cfg =>
			cfg.RegisterServicesFromAssembly(typeof(MappingProfile).Assembly));

		builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
		builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

		return builder;
	}

	public static WebApplicationBuilder ConfigureJWT(this WebApplicationBuilder builder)
	{
		var jwtConfiguration = new JwtConfiguration();
		builder.Configuration.Bind(jwtConfiguration.Section, jwtConfiguration);

		var secretKey = builder.Configuration.GetValue<string>("JWT_SECRET_KEY") ?? Environment.GetEnvironmentVariable("JWT_SECRET_KEY");
		ArgumentNullException.ThrowIfNull(secretKey);

		builder.Services
			.AddAuthentication(opt =>
			{
				opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,

					ValidIssuer = jwtConfiguration.ValidIssuer,
					ValidAudience = jwtConfiguration.ValidAudience,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
				}
			);

		builder.Services.Configure<JwtConfiguration>("JwtSettings", builder.Configuration.GetSection("JwtSettings"));
		return builder;
	}

	public static WebApplicationBuilder AddInfrastructureServices(this WebApplicationBuilder builder)
	{
		//LogManager.Setup().LoadConfigurationFromFile("nlog.config", true);
		//builder.Services.AddSingleton<ILoggingService, LoggingService>();


		return builder;
	}
}
