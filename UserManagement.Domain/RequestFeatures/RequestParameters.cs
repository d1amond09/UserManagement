namespace UserManagement.Domain.RequestFeatures;

public abstract class RequestParameters
{
	public string OrderBy { get; set; } = string.Empty;
	public string? SearchTerm { get; set; }
}


