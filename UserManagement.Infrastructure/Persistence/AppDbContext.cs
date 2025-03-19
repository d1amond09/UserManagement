using Microsoft.EntityFrameworkCore;
using UserManagement.Domain.Entities;

namespace UserManagement.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
	public virtual DbSet<User> Users { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<User>(entity =>
		{
			entity
				.HasNoKey()
				.ToTable("users");

			entity.HasIndex(e => e.Id, "id").IsUnique();

			entity.HasIndex(e => e.Email, "unique_email").IsUnique();

			entity.Property(e => e.Email).HasColumnName("email");

			entity.Property(e => e.Id)
				.HasMaxLength(16)
				.IsFixedLength()
				.HasColumnName("id");

			entity.Property(e => e.IsBlocked)
				.HasDefaultValueSql("'0'")
				.HasColumnName("is_blocked");

			entity.Property(e => e.LastLogin)
				.HasColumnType("datetime")
				.HasColumnName("last_login");

			entity.Property(e => e.Name)
				.HasMaxLength(255)
				.HasColumnName("name");

			entity.Property(e => e.Password)
				.HasMaxLength(255)
				.HasColumnName("password");

			entity.Property(e => e.RegistrationTime)
				.HasDefaultValueSql("CURRENT_TIMESTAMP")
				.HasColumnType("datetime")
				.HasColumnName("registration_time");
		});

	}

}
