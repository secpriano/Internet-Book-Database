namespace IBDbWebApplication.Models.AdminModels.BookModels;

public class BookViewModel
{
    public List<BookModel> BookModels { get; set; } = new List<BookModel>();
    public IEnumerable<AuthorModel> AuthorModels { get; set; }  = new List<AuthorModel>();
    public IEnumerable<PublisherModel> PublisherModels { get; set; } = new List<PublisherModel>();
    public IEnumerable<GenreModel> GenreModels { get; set; } = new List<GenreModel>();
    public IEnumerable<ThemeModel> ThemeModels { get; set; } = new List<ThemeModel>();
    public IEnumerable<SettingModel> SettingModels { get; set; } = new List<SettingModel>();
    public BookViewModel(List<BookModel> bookModels) =>
    BookModels = bookModels;

    public BookViewModel()
    {
        
    }
}