namespace Interface.DTO;

public record struct BookDTO(long? Id, string Isbn, string Title, string Synopsis, DateOnly PublishDate, 
    ushort AmountPages, IEnumerable<AuthorDTO> Authors, PublisherDTO Publisher, IEnumerable<GenreDTO> Genres, 
    IEnumerable<ThemeDTO> Themes, IEnumerable<SettingDTO> Settings, ulong Favorites);