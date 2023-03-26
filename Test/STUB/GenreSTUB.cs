using Interface.DTO;

namespace Test.STUB;

public class GenreSTUB
{
    public List<GenreDTO> Genres = new()
    {
        new(1, "High Fantasy"),
        new(2, "Horror"),
        new(3, "Science Fiction")
    };
}