namespace IBDbWebApplication.Models.Entity;

public class BookModel
{
    //[Key]
    public long? Id { get; }
    
    /*[DisplayName("Isbn")]
    [Required(ErrorMessage = "An ISBN is required")]
    [StringLength(1, ErrorMessage = "1", MinimumLength = 0)]
    [RegularExpression(@"", ErrorMessage = "2")]*/
    public string Isbn { get; set; }
    
    /*[DisplayName("Title")]
    [Required(ErrorMessage = "A title is required")]
    [StringLength(1, ErrorMessage = "3", MinimumLength = 0)]
    [RegularExpression(@"", ErrorMessage = "4")]*/
    public string Title { get; set; }
    
    /*[DisplayName("Synopsis")]
    [Required(ErrorMessage = "A synopsis is required")]
    [StringLength(1, ErrorMessage = "5", MinimumLength = 0)]
    [RegularExpression(@"", ErrorMessage = "6")]*/
    public string Synopsis { get; set; }
    
    /*[DisplayName("PublishDate")]
    [Required(ErrorMessage = "A publish date is required")]
    // TODO: date validation
    [RegularExpression(@"", ErrorMessage = "7")]*/
    public DateTime PublishDate { get; set; }
    
    /*[DisplayName("AmountPages")]
    [Required(ErrorMessage = "An amount of pages is required")]
    [Range(1, 10, ErrorMessage = "8")]
    [RegularExpression(@"", ErrorMessage = "9")]*/
    public ulong AmountPages { get; set; }
    
    /*[DisplayName("Authors")]
    [Required(ErrorMessage = "An authors is required")]
    [RegularExpression(@"", ErrorMessage = "10")]*/
    public IEnumerable<AuthorModel> Authors { get; set; }
    
    /*[DisplayName("Publisher")]
    [Required(ErrorMessage = "A publisher is required")]
    [Range(1, 10, ErrorMessage = "11")]
    [StringLength(1, ErrorMessage = "12", MinimumLength = 0)]
    [RegularExpression(@"", ErrorMessage = "13")]*/
    public PublisherModel Publisher { get; set; }
    
    /*[DisplayName("Genres")]
    [Required(ErrorMessage = "Genres are required")]
    [Range(1, 10, ErrorMessage = "14")]
    [StringLength(1, ErrorMessage = "15", MinimumLength = 0)]
    [RegularExpression(@"", ErrorMessage = "16")]*/
    public IEnumerable<GenreModel> Genres { get; set; }
    
    /*[DisplayName("Themes")]
    [Required(ErrorMessage = "Themes are required")]
    [Range(1, 10, ErrorMessage = "17")]
    [StringLength(1, ErrorMessage = "18", MinimumLength = 0)]
    [RegularExpression(@"", ErrorMessage = "19")]*/
    public IEnumerable<ThemeModel> Themes { get; set; }
    
    /*[DisplayName("Settings")]
    [Required(ErrorMessage = "Settings are required")]
    [Range(1, 10, ErrorMessage = "20")]
    [StringLength(1, ErrorMessage = "21", MinimumLength = 0)]
    [RegularExpression(@"", ErrorMessage = "22")]*/
    public IEnumerable<SettingModel> Settings { get; set; }


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