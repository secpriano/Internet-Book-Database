using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IBDbWebApplication.Models.Entity;

public class CommentModel
{
    public long ReviewId { get; set; }
    
    [DisplayName("Content")]
    [Required(ErrorMessage = "Content is required")]
    [StringLength(4000, ErrorMessage = "Content must be more than 2 characters long and less than 4000 characters", MinimumLength = 2 )]
    public string Content { get; set; }
    public long BookId { get; set; }
    
    public CommentModel(string content) => Content = content;

    public CommentModel() { }
}