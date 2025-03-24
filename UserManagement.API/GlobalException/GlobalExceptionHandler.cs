using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Domain.Exceptions;

namespace UserManagement.API.GlobalException;

public class GlobalExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
	private readonly IProblemDetailsService _problemDetailsService = problemDetailsService;

	public async ValueTask<bool> TryHandleAsync(
		HttpContext httpContext, 
		Exception exception, 
		CancellationToken cancellationToken)
	{
		httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
		httpContext.Response.ContentType = "application/json";
		var contextFeature = httpContext.Features.Get<IExceptionHandlerFeature>();

		httpContext.Response.StatusCode = contextFeature?.Error switch
		{
			NotFoundException => StatusCodes.Status404NotFound,
			BadRequestException => StatusCodes.Status400BadRequest,
			_ => StatusCodes.Status500InternalServerError
		};

		return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
		{
			HttpContext = httpContext,
			ProblemDetails =
			{
				Title = "An error occured",
				Detail = exception.Message,
				Type = exception.GetType().Name,
				Status = httpContext.Response.StatusCode
			},
			Exception = exception,
		});
	
	}
}
