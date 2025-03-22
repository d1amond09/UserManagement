using UserManagement.Domain.Contracts.Persistence;
using UserManagement.Application.Responses;
using MediatR;

namespace UserManagement.Application.UseCases.Users.BlockUsers
{
	public class BlockUsersHandler(IRepositoryManager rep) : IRequestHandler<BlockUsersUseCase, ApiBaseResponse>
	{ 
		private readonly IRepositoryManager _rep = rep;

		public async Task<ApiBaseResponse> Handle(BlockUsersUseCase request, CancellationToken cancellationToken)
		{
			var userIds = request.BlockUsersRequest.UserIds.Select(Guid.Parse);
			var users = await _rep.Users.GetByIdsAsync(userIds, request.TrackChanges);
			List<Guid> blockedUserIds = [];

			foreach (var user in users)
			{
				user.IsBlocked = true;
				blockedUserIds.Add(user.Id);
			}

			await _rep.SaveAsync();

			return new ApiOkResponse<List<Guid>>(blockedUserIds);
		}
	}
}