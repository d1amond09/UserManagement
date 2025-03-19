using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using UserManagement.Domain.Entities;

namespace UserManagement.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder
			.ToTable("users")
			.HasKey(e => e.Id);

		builder.HasIndex(e => e.Id, "id").IsUnique();

		builder.HasIndex(e => e.Email, "unique_email").IsUnique();

		builder.Property(e => e.Email)
			.HasColumnName("email")
			.IsRequired();

		builder.Property(e => e.Id)
				  .HasColumnType("BINARY(16)")
				  .HasConversion(
					  v => v.ToByteArray(), 
					  v => new Guid(v));

		builder.Property(e => e.IsBlocked)
			.HasDefaultValueSql("'0'")
			.HasColumnName("is_blocked");

		builder.Property(e => e.LastLogin)
			.HasColumnType("datetime")
			.HasColumnName("last_login");

		builder.Property(e => e.Name)
			.HasMaxLength(255)
			.HasColumnName("name")
			.IsRequired();

		builder.Property(e => e.Password)
			.HasMaxLength(255)
			.HasColumnName("password")
			.IsRequired();

		builder.Property(e => e.RegistrationTime)
			.HasDefaultValueSql("CURRENT_TIMESTAMP")
			.HasColumnType("datetime")
			.HasColumnName("registration_time");

		builder.Property(e => e.RefreshToken)
			.HasMaxLength(255)
			.HasColumnName("refresh_token");

		builder.Property(e => e.RefreshTokenExpiryTime)
			.HasColumnType("datetime")
			.HasColumnName("refresh_token_expiry_time");
	}
}