namespace UserManagement.Application.DTO.User;

public record UserDto
{
	public Guid Id { get; init; }
	public string Name { get; init; } = string.Empty;
	public string Email { get; init; } = string.Empty;
	public DateTime? LastLogin { get; init; }
	public bool IsBlocked { get; init; }
	public DateTime? RegistrationTime { get; init; }
}
