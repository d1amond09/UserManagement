using UserManagement.Domain.Contracts.Persistence;
using UserManagement.Application.Responses;
using MediatR;

namespace UserManagement.Application.UseCases.Users.UnblockUsers
{
	public class UnblockUsersHandler(IRepositoryManager rep) : IRequestHandler<UnblockUsersUseCase, ApiBaseResponse>
	{ 
		private readonly IRepositoryManager _rep = rep;

		public async Task<ApiBaseResponse> Handle(UnblockUsersUseCase request, CancellationToken cancellationToken)
		{
			var users = await _rep.Users.GetByIdsAsync(request.UserIds, request.TrackChanges);
			List<Guid> unblockedUserIds = [];

			foreach (var user in users)
			{
				user.IsBlocked = false;
				unblockedUserIds.Add(user.Id);
			}

			await _rep.SaveAsync();

			return new ApiOkResponse<List<Guid>>(unblockedUserIds);
		}
	}
}