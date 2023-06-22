namespace Business.Container;

public class Validate
{
    public void OutOfRange(in ulong value, in ulong min, in ulong max, in string name, in Unit unit)
    {
        if (value < min) throw new KeyValueException($"{name} must be more than or equal to {min} {unit.ToString()}. Not {value} {unit.ToString()}.", name);

        if (value > max) throw new KeyValueException($"{name} must be less or equal to {max} {unit.ToString()}. Not {value} {unit.ToString()}.", name);
    }
    
    public void OutOfRange(in ulong value, in ulong min, in ulong max, in string name, in string otherProperty, in Unit unit)
    {
        if (value < min) throw new KeyValueException($"{name} must be more than or equal to the length of {otherProperty}: {min} {unit.ToString()}. Not {value} {unit.ToString()}.", name);

        if (value > max) throw new KeyValueException($"{name} must be less or equal to {max} {unit.ToString()}. Not {value} {unit.ToString()}.", name);
    }
        
    public void ExactValue(in ulong value, in ulong exact, in string name, in Unit unit)
    {
        if (value != exact) throw new KeyValueException($"{name} must be exactly {exact} {unit.ToString()}.", name);
    }
    
    public void Regex(in string input, in string pattern, in string name, in string message)
    {
        if (!System.Text.RegularExpressions.Regex.IsMatch(input, pattern)) throw new KeyValueException(message, name);
    }
    
    public void Duplicate<T>(IEnumerable<T> entities, in string name) where T : IEquatable<T>
    {
        byte count = 0;
    
        for (int i = 0; i < entities.Count(); i++)
        {
            for (int j = 0; j < entities.Count(); j++)
            {
                if (i != j && entities.ElementAt(i).Equals(entities.ElementAt(j)))
                    throw new KeyValueException($@"{name} is already added.", name);
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