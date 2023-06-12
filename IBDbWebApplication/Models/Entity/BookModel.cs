namespace IBDbWebApplication.Models.Entity;

public class BookModel
{
    public long? Id { get; set; }
    
    public string Isbn { get; set; }
    
    public string Title { get; set; }
    
    public string Synopsis { get; set; }
    
    public DateOnly PublishDate { get; set; }
    
    public ulong AmountPages { get; set; }
    
    public IEnumerable<AuthorModel> Authors { get; set; }
    
    public PublisherModel Publisher { get; set; }
    
    public IEnumerable<GenreModel> Genres { get; set; }
    
    public IEnumerable<ThemeModel> Themes { get; set; }

    public IEnumerable<SettingModel> Settings { get; set; }

    public ulong Favorites { get; set; }


    public BookModel(
        long? id, 
        string isbn, 
        string title, 
        string synopsis, 
        DateOnly publishDate, 
        ulong amountPages, 
        IEnumerable<AuthorModel> authors, 
        PublisherModel publisher, 
        IEnumerable<GenreModel> genres, 
        IEnumerable<ThemeModel> themes, 
        IEnumerable<SettingModel> settings,
        ulong favorites) =>
        (Id, Isbn, Title, Synopsis, PublishDate, AmountPages, Authors, Publisher, Genres, Themes, Settings, Favorites) =
        (id, isbn, title, synopsis, publishDate, amountPages, authors, publisher, genres, themes, settings, favorites);

    public BookModel()
    {
            
    }
}