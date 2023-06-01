using Business.Entity;
using Interface.Interfaces;

namespace Business.Container;

public class AuthorContainer
{
    private readonly IAuthorData _authorData;
    
    public AuthorContainer(IAuthorData authorData)
    {
        _authorData = authorData;
    }   
    
    public IEnumerable<Author> GetAll()
    {
        return _authorData.GetAll().Select(author => new Author(author));
    }

    public bool Add(Author author)
    {
        ValidateAuthor(author);
        
        return _authorData.Add(author.ToDto());
    }
    
    private void ValidateAuthor(Author author)
    {
        ValidateName(author.Name);
        ValidateDescription(author.Description);
        ValidateBirthDate(author.BirthDate);
        ValidateDeathDate(author.DeathDate, author.BirthDate);
        // TODO: Validate duplicate
        
        if (Validate.Exceptions.InnerExceptions.Count > 0)
        {
            throw Validate.Exceptions;
        }
    }

    private void ValidateDeathDate(DateOnly? authorDeathDate, DateOnly authorBirthDate)
    {
        if (authorDeathDate == null)
        {
            return;
        }
        
        Validate.OutOfRange((ulong)authorDeathDate.Value.Year, (ulong)authorBirthDate.Year, (ulong)DateTime.Now.Year, "Death date", Validate.Unit.Year);
    }

    private void ValidateName(string name)
    {
        Validate.OutOfRange((ulong)name.Length, 1, 1000, "Name", Validate.Unit.Character);
        Validate.Regex(name, "^[a-zA-Z .]+$", "Name can only contain letters, spaces, and periods.");
    }
    
    private void ValidateDescription(string authorDescription)
    {
        Validate.OutOfRange((ulong)authorDescription.Length, 10, 1000, "Description", Validate.Unit.Character);
    }
    
    private void ValidateBirthDate(DateOnly birthDate)
    {
        Validate.OutOfRange((ulong)birthDate.Year, (ulong)DateTime.Now.AddYears(-150).Year, (ulong)DateTime.Now.AddYears(-6).Year, "Birth date", Validate.Unit.Year);
    }
}