﻿namespace UserManagement.Application.Responses;

public abstract class ApiBaseResponse(bool success)
{
	public bool Success { get; set; } = success;
}
