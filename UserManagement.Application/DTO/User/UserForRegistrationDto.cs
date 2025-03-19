namespace UserManagement.Application.DTO.User;

public record UserForRegistrationDto
{
	public string Email { get; init; } = string.Empty;
	public string Password { get; init; } = string.Empty;
}
