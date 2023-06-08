using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IBDbWebApplication.Models.Entity;

public class ReviewModel
{ 
    public long BookId { get; set; }

    [DisplayName("Title")]
    [Required(ErrorMessage = "A title is required")]
    [StringLength(100, ErrorMessage = "Content must be more than 1 character long and less than 100 characters", MinimumLength = 1)]
    public string Title { get; set; }
    
    [DisplayName("Content")]
    [Required(ErrorMessage = "Content is required")]
    [StringLength(2000, ErrorMessage = "Content must be more than 2 characters long and less than 2000 characters", MinimumLength = 2 )]
    public string Content { get; set; }
    
    public ReviewModel(string title, string content, long bookId) =>
        (Title, Content, BookId) = (title, content, bookId);

    public ReviewModel() { }
}