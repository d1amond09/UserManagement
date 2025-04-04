﻿using UserManagement.Application.Responses;
using UserManagement.Application.DTO.User;
using MediatR;

namespace UserManagement.Application.UseCases.Auth.RegisterUser;

public sealed record RegisterUserUseCase(UserForRegistrationDto UserForRegistrationDto) :
	IRequest<ApiBaseResponse>;
