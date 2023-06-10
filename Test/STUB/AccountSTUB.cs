using Interface.DTO;
using Interface.Interfaces;

namespace Test.STUB;

public class AccountSTUB : IAccountData
{
    public List<AccountDTO> Accounts = new()
    {
        new(1, "NiceUsername", "just@me.com", true, "1234"),
        new(2, "NotANiceUsername2", "dont@me.com", false, "4321")
    };

    public AccountDTO Login(AccountDTO accountDTO, string password)
    {
        return Accounts.FirstOrDefault(account => account.Username == accountDTO.Username && account.Password == password);
    }
}