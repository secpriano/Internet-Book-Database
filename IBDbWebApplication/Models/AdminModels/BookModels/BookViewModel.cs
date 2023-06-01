using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using IBDbWebApplication.Models.Entity;

namespace IBDbWebApplication.Models.AdminModels.BookModels;

public class BookViewModel
{
    [Key]
    public long? Id { get; set; }
    
    [DisplayName("Isbn")]
    [Required(ErrorMessage = "isbn is required")]
    [StringLength(13, ErrorMessage = "isbn must be 13 characters long", MinimumLength = 13 )]
    [RegularExpression(@"^[0-9]+$", ErrorMessage = "A")]
    public string Isbn { get; set; }
    
    [DisplayName("Title")]
    [Required(ErrorMessage = "An title is required")]
    [StringLength(100, ErrorMessage = "A", MinimumLength = 1 )]
    [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "A")]
    public string Title { get; set; }
    
    [DisplayName("Synopsis")]
    [Required(ErrorMessage = "An synopsis is required")]
    [StringLength(1000, ErrorMessage = "A", MinimumLength = 0 )]
    [RegularExpression(@"^[a-zA-Z ,.?!]+$", ErrorMessage = "A")]
    public string Synopsis { get; set; }
    
    [DisplayName("PublishDate")]
    [Required(ErrorMessage = "An publishDate is required")]
    // TODO: date validation
    public DateTime PublishDate { get; set; }
    
    [DisplayName("AmountPages")]
    [Required(ErrorMessage = "An amountPages is required")]
    [Range(1, 50000, ErrorMessage = "A")]
    public ushort AmountPages { get; set; }
    
    [DisplayName("Authors")]
    [Required(ErrorMessage = "An authors is required")]
    public IEnumerable<byte> AuthorIds { get; set; }
    
    [DisplayName("Publisher")]
    [Required(ErrorMessage = "An publisher is required")]
    public byte PublisherId { get; set; }
    
    [DisplayName("Genres")]
    [Required(ErrorMessage = "An genres is required")]
    public IEnumerable<byte> GenreIds { get; set; }
    
    [DisplayName("Themes")]
    [Required(ErrorMessage = "An themes is required")]
    public IEnumerable<byte> ThemeIds { get; set; }
    
    [DisplayName("Settings")]
    [Required(ErrorMessage = "An settings is required")]
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