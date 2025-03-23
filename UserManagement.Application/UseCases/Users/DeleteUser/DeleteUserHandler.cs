using UserManagement.Domain.Contracts.Persistence;
using UserManagement.Application.Responses;
using UserManagement.Domain.Entities;
using MediatR;

namespace UserManagement.Application.UseCases.Users.DeleteUser;

public class DeleteEventHandler(IRepositoryManager rep) : IRequestHandler<DeleteUserUseCase, ApiBaseResponse>
{
	private readonly IRepositoryManager _rep = rep;

	public async Task<ApiBaseResponse> Handle(DeleteUserUseCase request, CancellationToken cancellationToken)
	{
		var userToDelete = await _rep.Users.GetByIdAsync(request.Id, request.TrackChanges);

		if (userToDelete is null)
			return new ApiNotFoundResponse($"User is not found by id: {request.Id}");

		_rep.Users.Delete(userToDelete);
		await _rep.SaveAsync();

		return new ApiOkResponse<User>(userToDelete);
	}
}
