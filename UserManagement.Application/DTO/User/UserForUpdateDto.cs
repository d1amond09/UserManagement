namespace UserManagement.Application.DTO.User;

public record UserForUpdateDto
{
	public Guid Id { get; init; }
	public string Name { get; init; } = string.Empty;
	public string Password { get; init; } = string.Empty;
	public DateTime? LastLogin { get; init; }
	public bool IsBlocked { get; init; }
	public string? RefreshToken { get; init; }
	public DateTime? RefreshTokenExpiryTime { get; init; }
}
