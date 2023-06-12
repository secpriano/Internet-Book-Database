using Interface.DTO;
using Interface.Interfaces;

namespace Test.STUB;

public class GenreSTUB : IGenreData
{
    public List<GenreDTO> Genres = new()
    {
        new(1, "High Fantasy"),
        new(2, "Horror"),
        new(3, "Science Fiction")
    };

    public bool Add(GenreDTO entity)
    {
        Genres.Add(entity);
        return Genres.Exists(entity.Equals);
    }

    public GenreDTO GetById(long id)
    {
        throw new NotImplementedException();
    }

    public GenreDTO Update(GenreDTO entity)
    {
        throw new NotImplementedException();
    }

    public bool Delete(long id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<GenreDTO> GetAll()
    {
        throw new NotImplementedException();
    }

    public bool Exist(string uid)
    {
        return Genres.Exists(genre => genre.Name == uid);
    }
}