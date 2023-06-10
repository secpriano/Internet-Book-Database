namespace Business.Container;

public class KeyValueException : Exception
{
    public string Type { get; set; }
    
    public KeyValueException(string message, string type) : base(message)
    {
        Type = type;
    }
}