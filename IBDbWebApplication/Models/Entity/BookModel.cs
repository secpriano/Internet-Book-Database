namespace IBDbWebApplication.Models.AdminModels.BookModels;

public class BookModel
{
    public long? Id { get; }
    public string Isbn { get; }
    public string Title { get; }
    public string Synopsis { get; set; }
    public DateTime PublishDate { get; set; }
    public ulong AmountPages { get; set; }
    public IEnumerable<AuthorModel> Authors { get; set; }
    public PublisherModel Publisher { get; }
    public IEnumerable<GenreModel> Genres { get; }
    public IEnumerable<ThemeModel> Themes { get; }
    public IEnumerable<SettingModel> Settings { get; }

    public BookModel(
        long? id, 
        string isbn, 
        string title, 
        string synopsis, 
        DateTime publishDate, 
        ulong amountPages, 
        IEnumerable<AuthorModel> authors, 
        PublisherModel publisher, 
        IEnumerable<GenreModel> genres, 
        IEnumerable<ThemeModel> themes, 
        IEnumerable<SettingModel> settings) =>
        (Id, Isbn, Title, Synopsis, PublishDate, AmountPages, Authors, Publisher, Genres, Themes, Settings) =
        (id, isbn, title, synopsis, publishDate, amountPages, authors, publisher, genres, themes, settings);
}