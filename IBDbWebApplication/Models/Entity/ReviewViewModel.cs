using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IBDbWebApplication.Models.Entity;

public class ReviewViewModel
{
    public long? Id { get; }
    public string Title { get; set; }
    public string Content { get; set; }
    public AccountModel Account { get; }
    
    public IEnumerable<CommentViewModel> CommentModels { get; } = new List<CommentViewModel>();

    public ReviewViewModel(long? id, string title, string content, AccountModel account, IEnumerable<CommentViewModel> commentModels) =>
        (Id, Title, Content, Account, CommentModels) = (id, title, content, account, commentModels);
}