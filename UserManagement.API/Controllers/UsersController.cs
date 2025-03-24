using System.Text.Json;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Common;
using UserManagement.API.Extensions;
using UserManagement.Application.DTO.User;
using UserManagement.Application.Requests;
using UserManagement.Application.UseCases.Users.BlockUsers;
using UserManagement.Application.UseCases.Users.DeleteUser;
using UserManagement.Application.UseCases.Users.GetUser;
using UserManagement.Application.UseCases.Users.GetUsers;
using UserManagement.Application.UseCases.Users.UnblockUsers;
using UserManagement.Domain.RequestFeatures;

namespace UserManagement.API.Controllers;

[Route("api/users")]
[ApiController]
[Authorize]
public class UsersController(ISender sender) : ApiControllerBase
{
	private readonly ISender _sender = sender;

	[HttpGet]
	public async Task<IActionResult> GetUsers([FromQuery] UserParameters userParams)
	{
		var baseResult = await _sender.Send(new GetUsersUseCase(userParams, TrackChanges: false));

		if (!baseResult.Success) 
			return ProccessError(baseResult);

		var response = baseResult.GetResult<IEnumerable<UserDto>>();

		return Ok(response);
	}

	[HttpGet("{id:guid}")]
	public async Task<IActionResult> GetUser(Guid id)
	{
		var baseResult = await _sender.Send(new GetUserUseCase(id, TrackChanges: false));

		if (!baseResult.Success)
			return ProccessError(baseResult);

		var response = baseResult.GetResult<UserDto>();

		return Ok(response);
	}

	[HttpPut("block")]	
	public async Task<IActionResult> BlockUsers([FromBody] IEnumerable<string> userIds)
	{
		var baseResult = await _sender.Send(new BlockUsersUseCase(userIds, TrackChanges: false));

		if (!baseResult.Success)
			return ProccessError(baseResult);

		var response = baseResult.GetResult<List<Guid>>();

		return Ok(response);
	}


	[HttpPut("unblock")]
	public async Task<IActionResult> UnblockUsers([FromBody] IEnumerable<string> userIds)
	{
		var baseResult = await _sender.Send(new UnblockUsersUseCase(userIds, TrackChanges: false));

		if (!baseResult.Success)
			return ProccessError(baseResult);

		var response = baseResult.GetResult<List<Guid>>();

		return Ok(response); 
	}

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> DeleteUser(Guid id)
	{
		var baseResult = await _sender.Send(new DeleteUserUseCase(id, TrackChanges: false));

		if (!baseResult.Success)
			return ProccessError(baseResult);

		return NoContent();
	}
}
