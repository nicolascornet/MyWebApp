namespace MyWebApp.Exceptions;

[Serializable]
public class ForbiddenException : Exception
{
    public ForbiddenException(string role) : base("Forbidden")
    {
        Data.Add("role", role);
    }
}
