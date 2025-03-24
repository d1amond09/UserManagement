using UserManagement.Application.ErrorModels;
using UserManagement.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace UserManagement.API.Controllers;

public class ApiControllerBase : ControllerBase
{
	[HttpHead]
	public IActionResult ProccessError(ApiBaseResponse baseResponse)
	{
		return baseResponse switch
		{
			ApiNotFoundResponse => NotFound(new ErrorDetails
			{
				Message = ((ApiNotFoundResponse)baseResponse).Message,
				Status = StatusCodes.Status404NotFound
			}),
			ApiBadRequestResponse response => BadRequest(new ErrorDetails
			{
				Message = response.Message,
				Status = StatusCodes.Status400BadRequest
			}),
			_ => throw new NotImplementedException()
		};
	}
}
