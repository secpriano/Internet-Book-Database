using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace Business.Container;

public static class Validate
{
    public static AggregateException Exceptions = new();
    public static void OutOfRange(in ulong value, in ulong min, in ulong max, in string name, in Unit unit)
    {
        if (value < min) Exceptions.InnerExceptions.Append(new($"{name} must be more than or equal to {min} {unit.ToString()}."));

        if (value > max) Exceptions.InnerExceptions.Append(new($"{name} must be less or equal to {max} {unit.ToString()}."));
    }
    
    public static void ExactValue(in ulong value, in ulong exact, in string name, in Unit unit)
    {
        if (value != exact) Exceptions.InnerExceptions.Append(new($"{name} must be exactly {exact} {unit.ToString()}."));
    }
    
    public static void Regex(in string input, in string pattern, in string message)
    {
        if (!System.Text.RegularExpressions.Regex.IsMatch(input, pattern)) Exceptions.InnerExceptions.Append(new(message));
    }
    
    public static void Duplicate<T>(IEnumerable<T> entities, in string name)
    {
        byte count = 0;
        
        for (int i = 0; i < entities.Count(); i++)
        {
            for (int j = 0; j < entities.Count(); j++)
            {
                if (i != j && entities.ElementAt(i).Equals(entities.ElementAt(j)))
                    Exceptions.InnerExceptions.Append(new($@"{name} is already added."));
            }
        }
    }

    public enum Unit : byte
    {
        Character,
        Page,
        Year
    }
}