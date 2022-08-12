namespace MyWebApp.Exceptions;

[Serializable]
public class ValidationException : Exception
{
    public ValidationException(string name, object key) : base("One or more validation failures have occurred.")
    {
        Data.Add("name", name);
        Data.Add("key", key);
    }

}
