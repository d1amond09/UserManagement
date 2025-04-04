﻿using UserManagement.Application.Responses;
using MediatR;

namespace UserManagement.Application.UseCases.Users.BlockUsers;

public sealed record BlockUsersUseCase(IEnumerable<string> UserIds, bool TrackChanges) :
	IRequest<ApiBaseResponse>;