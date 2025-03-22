namespace UserManagement.Application.Responses;

public class ApiNotFoundResponse(string message) : ApiBaseResponse(false)
{
	public string Message { get; set; } = message;
}
