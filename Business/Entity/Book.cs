using Interface.DTO;

namespace Business.Entity;

public class Book : IEqualityComparer<Book>
{
    public long? Id { get; }
    public string Isbn { get; }
    public string Title { get; }
    public string Synopsis { get; set; }
    public DateTime PublishDate { get; set; }
    public ushort AmountPages { get; set; }
    public IEnumerable<Author> Authors { get; set; }
    public Publisher Publisher { get; }
    public IEnumerable<Genre> Genres { get; }
    public IEnumerable<Theme> Themes { get; }
    public IEnumerable<Setting> Settings { get; }

    public Book(
        long? id, 
        string isbn, 
        string title, 
        string synopsis, 
        DateTime publishDate, 
        ushort amountPages, 
        IEnumerable<Author> authors, 
        Publisher publisher, 
        IEnumerable<Genre> genres, 
        IEnumerable<Theme> themes, 
        IEnumerable<Setting> settings) =>
        (Id, Isbn, Title, Synopsis, PublishDate, AmountPages, Authors, Publisher, Genres, Themes, Settings) =
        (id, isbn, title, synopsis, publishDate, amountPages, authors, publisher, genres, themes, settings);
    
    public Book(BookDTO bookDto) : this(
        bookDto.Id, 
        bookDto.Isbn, 
        bookDto.Title, 
        bookDto.Synopsis, 
        bookDto.PublishDate, 
        bookDto.AmountPages, 
        bookDto.Authors.Select(author => new Author(author)), 
        new(bookDto.Publisher), 
        bookDto.Genres.Select(genre => new Genre(genre)), 
        bookDto.Themes.Select(theme => new Theme(theme)), 
        bookDto.Settings.Select(setting => new Setting(setting))
    ) { }

    public BookDTO GetDto() => new(
        Id, 
        Isbn, 
        Title, 
        Synopsis, 
        PublishDate, 
        AmountPages, 
        Authors.Select(author => author.ToDto()), 
        Publisher.GetDto(), 
        Genres.Select(genre => genre.ToDto()), 
        Themes.Select(theme => theme.GetDto()), 
        Settings.Select(setting => setting.GetDto())
    );

    public bool Equals(Book x, Book y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.Id == y.Id && x.Isbn == y.Isbn && x.Title == y.Title && x.Synopsis == y.Synopsis && x.PublishDate.Equals(y.PublishDate) && x.AmountPages == y.AmountPages && x.Authors.Equals(y.Authors) && x.Publisher.Equals(y.Publisher) && x.Genres.Equals(y.Genres) && x.Themes.Equals(y.Themes) && x.Settings.Equals(y.Settings);
    }

    public int GetHashCode(Book obj)
    {
        HashCode hashCode = new HashCode();
        hashCode.Add(obj.Id);
        hashCode.Add(obj.Isbn);
        hashCode.Add(obj.Title);
        hashCode.Add(obj.Synopsis);
        hashCode.Add(obj.PublishDate);
        hashCode.Add(obj.AmountPages);
        hashCode.Add(obj.Authors);
        hashCode.Add(obj.Publisher);
        hashCode.Add(obj.Genres);
        hashCode.Add(obj.Themes);
        hashCode.Add(obj.Settings);
        return hashCode.ToHashCode();
    }
}