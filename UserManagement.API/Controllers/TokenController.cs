using UserManagement.Application.UseCases.Auth.FindUserByToken;
using UserManagement.Application.UseCases.Auth.RefreshToken;
using UserManagement.Domain.Entities;
using UserManagement.Application.DTO;
using UserManagement.API.Extensions;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace UserManagement.API.Controllers;

[Consumes("application/json")]
[Route("api/token")]
[ApiController]
public class TokenController(ISender sender) : ApiControllerBase
{
	private readonly ISender _sender = sender;

	[HttpPost("refresh")]
	public async Task<IActionResult> Refresh([FromBody] TokenDto tokenDto)
	{
		var baseResultUser = await _sender.Send(new FindUserByTokenUseCase(tokenDto));
		if (!baseResultUser.Success)
			return ProccessError(baseResultUser);
		var user = baseResultUser.GetResult<User>();

		var baseResult = await _sender.Send(new RefreshTokenUseCase(user, PopulateExp: false));
		if (!baseResult.Success)
			return ProccessError(baseResult);
		var tokenDtoToReturn = baseResult.GetResult<TokenDto>();

		return Ok(tokenDtoToReturn);
	}
}
