namespace UserManagement.Application.Responses;

public abstract class ApiNotFoundResponse(string message) : ApiBaseResponse(false)
{
	public string Message { get; set; } = message;
}
