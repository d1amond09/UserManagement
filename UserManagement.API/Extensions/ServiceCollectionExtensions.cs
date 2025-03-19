using Microsoft.EntityFrameworkCore;
using UserManagement.Infrastructure.Persistence;

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
}
