using UserManagement.Domain.Contracts.Persistence;
using UserManagement.Application.Responses;
using MediatR;
using UserManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace UserManagement.Application.UseCases.Auth.LoginUser;

public class BlockUsersHandler(IRepositoryManager rep, IPasswordHasher<User> passwordHasher) : IRequestHandler<LoginUserUseCase, ApiBaseResponse>
{ 
	private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;
	private readonly IRepositoryManager _rep = rep;

	public async Task<ApiBaseResponse> Handle(LoginUserUseCase request, CancellationToken cancellationToken)
	{
		User? user = await _rep.Users.GetByEmailAsync(request.UserToLogin.Email, request.TrackChanges);

		if (user == null)
			return new InvalidCredentialsBadRequestResponse();

		if (user.IsBlocked)
			return new UserBlockedBadRequestResponse();

		bool isValid = VerifyHashedPassword(user, request.UserToLogin.Password);
		await SaveDateLastLogin(user, isValid);

		if (!isValid)
			return new InvalidCredentialsBadRequestResponse();

		(bool, User) result = new(isValid, user);
		return new ApiOkResponse<(bool, User)>(result);
	}

	private async Task SaveDateLastLogin(User user, bool isValid)
	{
		if (isValid)
		{
			user.LastLogin = DateTime.Now;
			await _rep.SaveAsync();
		}
	}

	private bool VerifyHashedPassword(User user, string password)
	{
		var pvResult = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
		return pvResult.Equals(PasswordVerificationResult.Success);
	}
}