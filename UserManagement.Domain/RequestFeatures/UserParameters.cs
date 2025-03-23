namespace UserManagement.Domain.RequestFeatures;

public class UserParameters : RequestParameters
{
    public UserParameters()
    {
		OrderBy = "name";
    }
}
