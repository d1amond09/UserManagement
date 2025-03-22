using UserManagement.Application.Responses;

namespace UserManagement.API.Extensions;

public static class ApiBaseResponseExtensions
{
	public static TResultType GetResult<TResultType>(this ApiBaseResponse apiBaseResponse) =>
	   ((ApiOkResponse<TResultType>)apiBaseResponse).Result;

}
