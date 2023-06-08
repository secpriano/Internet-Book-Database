using Interface.DTO;

namespace Business.Entity;

public class Account
{
    public long? Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public bool IsAdmin { get; set; }
    
    public Account(long? id, string username, string email, bool isAdmin) => 
        (Id, Username, Email, IsAdmin) = (id, username, email, isAdmin);

    public Account(string email) => Email = email;
    public Account(long? id) => Id = id;
    
    public Account(AccountDTO accountDTO) : this(accountDTO.Id, accountDTO.Username, accountDTO.Email, accountDTO.IsAdmin) { }
    
    public AccountDTO ToDTO()
    {
        return new(
            Id,
            Username,
            Email,
            IsAdmin
            );
    }
}