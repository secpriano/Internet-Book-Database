using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IBDbWebApplication.Models.Entity;

public class CommentViewModel
{
    public string Content { get; }
    public AccountModel Account { get; }
    
    public CommentViewModel(string content, AccountModel account) => (Content, Account) = (content, account);
}