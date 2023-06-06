using Interface.DTO;

namespace Interface.Interfaces;

public interface IAccountData
{
    AccountDTO Login(AccountDTO accountDTO, string password);
}