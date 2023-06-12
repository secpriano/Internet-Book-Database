using Interface.DTO;
using Interface.Interfaces;

namespace Test.STUB;

public class AuthorSTUB : IAuthorData
{
    private static GenreSTUB _genreStub = new();
    
    public List<AuthorDTO> Authors = new()
    {
        new(1, "J.R.R. Tolkien", "Tolkien was an English writer, poet, philologist, and university professor who is best known as the author of the classic high fantasy works The Hobbit, The Lord of the Rings, and The Silmarillion.", 
            new(1897, 2, 13), new(1972, 8, 2),
            new()
            {
                _genreStub.Genres[0],
                _genreStub.Genres[1]
            }),
        new(2, "Stephen King", "King is an American author of horror, supernatural fiction, suspense, and fantasy novels. His books have sold more than 350 million copies, and many have been adapted into films, television series, miniseries, and comic books.", 
            new(1947, 9, 21), new(2021, 5, 4),
            new()
            {
                _genreStub.Genres[0],
                _genreStub.Genres[1],
                _genreStub.Genres[2]
            }),
        new(3, "George R.R. Martin", "Martin is an American novelist and short-story writer in the fantasy, horror, and science fiction genres, screenwriter, and television producer. He is best known for his series of epic fantasy novels A Song of Ice and Fire, which was later adapted into the HBO series Game of Thrones.", 
            new(1948, 9, 20), new(2021, 5, 9),
            new()
            {
                _genreStub.Genres[0],
                _genreStub.Genres[1],
                _genreStub.Genres[2]
            }),
    };

    public bool Add(AuthorDTO entity)
    {
        Authors.Add(entity);
        return Authors.Exists(entity.Equals);
    }

    public AuthorDTO GetById(long id)
    {
        throw new NotImplementedException();
    }

    public AuthorDTO Update(AuthorDTO entity)
    {
        throw new NotImplementedException();
    }

    public bool Delete(long id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<AuthorDTO> GetAll()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<AuthorDTO> GetByIds(IEnumerable<long> authorIds)
    {
        return Authors.Where(author => authorIds.Contains(author.Id.Value));
    }

    public bool Exist(string uid)
    {
        return Authors.Exists(author => author.Name == uid);
    }
}