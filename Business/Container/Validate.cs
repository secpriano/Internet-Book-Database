using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace Business.Container;

public static class Validate
{
    public static ArgumentException? OutOfRange(in ulong value, in ulong min, in ulong max, in string name, in string unit)
    {
        if (value < min) return new($"{name} must be more than or equal to {min} {unit}.");

        if (value > max) return new($"{name} must be less or equal to {max} {unit}.");

        return null;
    }
    
    public static ArgumentException? ExactValue(in ulong value, in ulong exact, in string name, in string unit)
    {
        if (value != exact) return new($"{name} must be exactly {exact} {unit}.");
        
        return null;
    }
    
    public static ArgumentException? ValidateExpression(in string input, in string pattern, in string message)
    {
        if (!Regex.IsMatch(input, pattern)) return new(message);
        
        return null;
    }
}