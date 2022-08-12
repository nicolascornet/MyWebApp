namespace MyWebApp.Exceptions;

[Serializable]
public class NotFoundException : Exception
{
    public NotFoundException(string name, object key) : base("Object not found")
    {
        Data.Add("name", name);
        Data.Add("key", key);
    }
}
