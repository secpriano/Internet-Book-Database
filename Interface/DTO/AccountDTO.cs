namespace Interface.DTO;

public record AccountDTO(long? Id, string Username, string Email, bool IsAdmin)
{
    public AccountDTO(long? id, string reviewUsername) : this(id, reviewUsername, "", false) { }
}