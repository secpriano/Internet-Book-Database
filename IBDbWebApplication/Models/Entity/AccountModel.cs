using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IBDbWebApplication.Models.Entity;

public class AccountModel
{
    public long? Id { get; set; }
    public string Username { get; set; } = "";
    [DisplayName("Email")]
    [Required(ErrorMessage = "Email is required")]
    [StringLength(100, ErrorMessage = "Email must be between 5 and 100 characters long", MinimumLength = 5 )]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    public bool IsAdmin { get; set; }
    
    [DisplayName("Password")]
    [Required(ErrorMessage = "Password is required")]
    [StringLength(512, ErrorMessage = "Password must be between 1 and 512 characters long", MinimumLength = 1 )]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    public AccountModel(long? id, string username, string email, bool isAdmin) => 
        (Id, Username, Email, IsAdmin) = (id, username, email, isAdmin);

    public AccountModel() { }
}