using Business.Entity;
using Interface.Interfaces;

namespace Business.Container;

public class AuthorContainer
{
    private readonly IAuthorData _authorData;
    Validate Validate = new();

    public AuthorContainer(IAuthorData authorData)
    {
        _authorData = authorData;
    }   
    
    public IEnumerable<Author> GetAll()
    {
        return _authorData.GetAll().Select(author => new Author(author));
    }
    
    public IEnumerable<Author> GetByIds(IEnumerable<long> authorIds)
    {
        return _authorData.GetByIds(authorIds).Select(dto => new Author(dto));
    }

    public bool Add(Author author)
    {
        ValidateAuthor(author);
        
        return _authorData.Add(author.ToDto());
    }
    
    private void ValidateAuthor(Author author)
    {
        try
        {
            Task[] tasks = {
                Task.Run(() => ValidateName(author.Name)),
                Task.Run(() => ValidateDescription(author.Description)),
                Task.Run(() => ValidateBirthDate(author.BirthDate)),
                Task.Run(() => ValidateDeathDate(author.DeathDate, author.BirthDate))
            };

            Task.WaitAll(tasks);
        }
        catch (AggregateException ex)
        {
            throw new AggregateException(ex.InnerExceptions);
        }
    }

    private void ValidateName(string name)
    {
        Validate.OutOfRange((ulong)name.Length, 1, 1000, "Name", Validate.Unit.Character);
        Validate.Regex(name, "^[a-zA-Z .]+$", "Name", "Name can only contain letters, spaces, and periods.");
        if (_authorData.Exist(name)) throw new KeyValueException($"Author {name} is already in use.", "Name");
    }
    
    private void ValidateDescription(string authorDescription)
    {
        Validate.OutOfRange((ulong)authorDescription.Length, 10, 1000, "Description", Validate.Unit.Character);
    }
    
    private void ValidateBirthDate(DateOnly birthDate)
    {
        Validate.OutOfRange((ulong)birthDate.Year, (ulong)DateTime.Now.AddYears(-150).Year, (ulong)DateTime.Now.AddYears(-6).Year, "Birth date", Validate.Unit.Year);
    }
    
    private void ValidateDeathDate(DateOnly? authorDeathDate, DateOnly authorBirthDate)
    {
        if (authorDeathDate == null)
        {
            return;
        }
        
        Validate.OutOfRange((ulong)authorDeathDate.Value.Year, (ulong)authorBirthDate.Year, (ulong)DateTime.Now.Year, "Death date", Validate.Unit.Year);
    }
}