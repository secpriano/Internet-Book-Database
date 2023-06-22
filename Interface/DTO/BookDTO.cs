namespace Interface.DTO;

public record BookDTO(long? Id, string Isbn, string Title, string Synopsis, DateOnly PublishDate,
    ushort AmountPages, List<AuthorDTO> Authors, PublisherDTO Publisher, List<GenreDTO> Genres,
    List<ThemeDTO> Themes, List<SettingDTO> Settings, ulong Favorites)
{
    public ulong Favorites { get; set; }
};