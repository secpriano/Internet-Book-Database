using System.Collections.ObjectModel;
using Business.Entity;
using Interface.Interfaces;

namespace Business.Container;

public class GenreContainer
{
    private readonly IGenreData _genreData;
    Validate Validate = new();

    
    public GenreContainer(IGenreData genreData)
    {
        _genreData = genreData;
    }
    
    public IEnumerable<Genre> GetAll()
    {
        return _genreData.GetAll().Select(genre => new Genre(genre.Id, genre.Name));
    }
    
    public bool Add(Genre genre)
    {
        ValidateGenre(genre);
        
        return _genreData.Add(genre.ToDto());
    }
    
    private void ValidateGenre(Genre genre)
    {
        try
        {
            Task[] tasks = {
                Task.Run(() => ValidateName(genre.Name))
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
        Validate.OutOfRange((ulong)name.Length, 2, 25, "Genre", Validate.Unit.Character);
        Validate.Regex(name, @"^[a-zA-Z ,&]+$", "Name", "Name can only contain letters, spaces, commas, and ampersands.");
        if (_genreData.Exist(name)) throw new KeyValueException($"Genre {name} is already in use.", "Name");
    }
}