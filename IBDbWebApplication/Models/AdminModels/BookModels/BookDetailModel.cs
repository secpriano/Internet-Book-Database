using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using IBDbWebApplication.Models.Entity;

namespace IBDbWebApplication.Models.AdminModels.BookModels;

public class BookDetailModel
{
    [Key]
    public long BookId { get; set; }
    [DisplayName("Title")]
    [Required(ErrorMessage = "An title is required")]
    [StringLength(25, ErrorMessage = "A", MinimumLength = 1 )]
    public string Title { get; set; }
    [DisplayName("Content")]
    [Required(ErrorMessage = "An content is required")]
    [StringLength(500, ErrorMessage = "A", MinimumLength = 2 )]
    public string Content { get; set; }
    
    public BookModel BookModel { get; set; }
    public IEnumerable<ReviewModel> ReviewModels { get; set; }

    public BookDetailModel(BookModel bookModel, IEnumerable<ReviewModel> reviewModels)
    {
        BookModel = bookModel;
        ReviewModels = reviewModels;
    }
}