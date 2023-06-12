using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using IBDbWebApplication.Models.Entity;

namespace IBDbWebApplication.Models.AdminModels.BookModels;

public class BookViewModel
{
    [Key]
    public long? Id { get; set; }
    
    [DisplayName("Isbn")]
    [Required(ErrorMessage = "Isbn is required")]
    [StringLength(13, ErrorMessage = "isbn must be 13 characters long", MinimumLength = 13 )]
    [RegularExpression(@"^[0-9]+$", ErrorMessage = "isbn must only contain numbers.")]
    public string Isbn { get; set; }
    
    [DisplayName("Title")]
    [Required(ErrorMessage = "Title is required")]
    [StringLength(100, ErrorMessage = "Title must be between 1 and 100 characters long", MinimumLength = 1 )]
    [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "title must only contain letters, and spaces.")]
    public string Title { get; set; }
    
    [DisplayName("Synopsis")]
    [Required(ErrorMessage = "Synopsis is required")]
    [StringLength(1000, ErrorMessage = "Title must be between 1 and 1000 characters long", MinimumLength = 1 )]
    [RegularExpression(@"^[a-zA-Z ,'’.?!]+$", ErrorMessage = "synopsis must only contain letters, spaces, and punctuation.")]
    public string Synopsis { get; set; }
    
    [DisplayName("Publish date")]
    [Required(ErrorMessage = "Publish date is required")]
    [DataType(DataType.Date)]
    public DateTime? PublishDate { get; set; }
    
    [DisplayName("Amount pages")]
    [Required(ErrorMessage = "Amount pages is required")]
    [Range(1, 50000, ErrorMessage = "Amount pages must be between 1 and 50.000")]
    [RegularExpression(@"^[0-9]+$", ErrorMessage = "Amount pages must only contain numbers.")]
    public ushort AmountPages { get; set; }
    
    [DisplayName("Authors")]
    [Required(ErrorMessage = "Author(s) is required")]
    public IEnumerable<long> AuthorIds { get; set; }
    
    [DisplayName("Publisher")]
    [Required(ErrorMessage = "Publisher is required")]
    public byte PublisherId { get; set; }
    
    [DisplayName("Genres")]
    [Required(ErrorMessage = "Genre(s) is required")]
    public IEnumerable<byte> GenreIds { get; set; }
    
    [DisplayName("Themes")]
    [Required(ErrorMessage = "Theme(s) is required")]
    public IEnumerable<byte> ThemeIds { get; set; }
    
    [DisplayName("Settings")]
    [Required(ErrorMessage = "Setting(s) is required")]
    public IEnumerable<byte> SettingIds { get; set; }

    public IEnumerable<BookModel> BookModels { get; set; } = new List<BookModel>();
    public IEnumerable<AuthorModel> AuthorModels { get; set; }  = new List<AuthorModel>();
    public IEnumerable<PublisherModel> PublisherModels { get; set; } = new List<PublisherModel>();
    public IEnumerable<GenreModel> GenreModels { get; set; } = new List<GenreModel>();
    public IEnumerable<ThemeModel> ThemeModels { get; set; } = new List<ThemeModel>();
    public IEnumerable<SettingModel> SettingModels { get; set; } = new List<SettingModel>();
    public BookViewModel(IEnumerable<BookModel> bookModels, IEnumerable<AuthorModel> authorModels, IEnumerable<PublisherModel> publisherModels, IEnumerable<GenreModel> genreModels, IEnumerable<ThemeModel> themeModels, IEnumerable<SettingModel> settingModels) =>
        (BookModels, AuthorModels, PublisherModels, GenreModels, ThemeModels, SettingModels) = (bookModels, authorModels, publisherModels, genreModels, themeModels, settingModels);
    public BookViewModel()
    {
        
    }
}