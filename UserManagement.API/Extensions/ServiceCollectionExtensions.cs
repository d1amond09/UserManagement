﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserManagement.Application;
using UserManagement.Domain.ConfigurationModels;
using UserManagement.Domain.Contracts.Persistence;
using UserManagement.Domain.Entities;
using UserManagement.Infrastructure.Persistence;
using UserManagement.Infrastructure.Persistence.Repositories;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using UserManagement.API.GlobalException;

namespace UserManagement.API.Extensions;

public static class ServiceCollectionExtensions
{
	private const string CONFIG_CONNECTION_STRING = "DefaultConnection";
	private const string JWT_SECRET_KEY = "JWT_SECRET_KEY";

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
		builder.Services.AddProblemDetails();

		builder.Services.AddAutoMapper(x => x.AddProfile(new MappingProfile()));
		builder.Services.AddMediatR(cfg =>
			cfg.RegisterServicesFromAssembly(typeof(MappingProfile).Assembly));

		builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
		builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

		builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
		return builder;
	}

	public static WebApplicationBuilder ConfigureJWT(this WebApplicationBuilder builder)
	{
		var jwtConfiguration = new JwtConfiguration();
		builder.Configuration.Bind(jwtConfiguration.Section, jwtConfiguration);

		var secretKey = builder.Configuration.GetValue<string>(JWT_SECRET_KEY) ?? Environment.GetEnvironmentVariable(JWT_SECRET_KEY);
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

		return builder;
	}

	public static WebApplicationBuilder AddSwaggerConfig(this WebApplicationBuilder builder)
	{
		builder.Services.AddSwaggerGen(s =>
		{
			s.SwaggerDoc("v1", new OpenApiInfo
			{
				Title = "UserManagement API",
				Version = "v1"
			});
			s.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
			s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
			{
				In = ParameterLocation.Header,
				Description = "Place to add JWT with Bearer",
				Name = "Authorization",
				Type = SecuritySchemeType.ApiKey,
				Scheme = "Bearer"
			});
			s.AddSecurityRequirement(new OpenApiSecurityRequirement() { {
				new OpenApiSecurityScheme {
					Reference = new OpenApiReference {
						Type = ReferenceType.SecurityScheme,
						Id = "Bearer"
					},
					Name = "Bearer",
				},
				new List<string>()
			} });
		});

		return builder;
	}
}
