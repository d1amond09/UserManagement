namespace UserManagement.Application.Responses;

public class NonUniqueEmailBadRequestResponse() : ApiBadRequestResponse("This email has already used")
{
}
