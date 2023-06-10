namespace Interface.DTO;

public record AccountDTO(long? Id, string Username, string Email, bool IsAdmin, string Password = "")
{
    public AccountDTO(long? id, string Username) : this(id, Username, "", false) { }
}