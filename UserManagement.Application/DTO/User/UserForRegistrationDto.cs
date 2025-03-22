namespace UserManagement.Application.DTO.User;

public record UserForRegistrationDto
{
	public string Name { get; set; } = string.Empty;
	public string Email { get; init; } = string.Empty;
	public string Password { get; init; } = string.Empty;
}
