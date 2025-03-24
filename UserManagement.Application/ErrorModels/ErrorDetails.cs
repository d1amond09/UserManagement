using System.Text.Json;

namespace UserManagement.Application.ErrorModels;

public class ErrorDetails
{
	public int Status { get; set; }
	public string? Message { get; set; }
	public override string ToString() => JsonSerializer.Serialize(this);
}
