﻿namespace UserManagement.Application.Responses;

public abstract class ApiBadRequestResponse(string message) : ApiBaseResponse(false)
{
	public string Message { get; set; } = message;
}


