using Business.Entity;
using Interface.Interfaces;

namespace Business.Container;

public class GenreContainer
{
    private readonly IGenreData _genreData;
    private ICollection<ArgumentException> _exceptions;
    
    public GenreContainer(IGenreData genreData)
    {
        _genreData = genreData;
        _exceptions = new List<ArgumentException>();
    }
    
    public bool Add(Genre genre)
    {
        ValidateGenre(genre);
        
        return _genreData.Add(genre.GetDto());
    }
    
    private void ValidateGenre(Genre genre)
    {
        ValidateName(genre.Name);
        
        if (_exceptions.Count > 0)
        {
            throw new AggregateException(_exceptions);
        }
    }
    
    private void ValidateName(string name)
    {
        
    }
}