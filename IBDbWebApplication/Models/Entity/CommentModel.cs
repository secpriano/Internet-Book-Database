using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IBDbWebApplication.Models.Entity;

public class CommentModel
{
    [Key]
    public long Id { get; }

    public long ReviewId { get; set; }
    
    [DisplayName("Content")]
    [Required(ErrorMessage = "Content is required")]
    [StringLength(4000, ErrorMessage = "Content must be more than 1 characters long and less than 4000 characters", MinimumLength = 1 )]
    public string Content { get; set; }
    public long UserId { get; }
    public long BookId { get; set; }
    
    public CommentModel(long id, string content, long userId) => (Id, Content, UserId) = (id, content, userId);

    public CommentModel() { }
}