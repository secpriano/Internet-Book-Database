using Business.Entity;
using Interface.Interfaces;

namespace Business.Container;

public class AccountContainer
{
    private readonly IAccountData _accountData;
    
    public AccountContainer(IAccountData accountData)
    {
        _accountData = accountData;
    }
    
    public Account Login(Account account, string password)
    {
        ValidateAccount(account, password);
        
        return new(_accountData.Login(account.ToDTO(), password));
    }

    private void ValidateAccount(Account account, string password)
    {
        try
        {
            Task[] tasks = {
                Task.Run(() => ValidateEmail(account.Email)),
                Task.Run(() => ValidatePassword(password))
            };

            Task.WaitAll(tasks);
        }
        catch (AggregateException ex)
        {
            throw new AggregateException(ex.InnerExceptions);
        }
    }
    
    private void ValidateEmail(string accountEmail)
    {
        Validate.OutOfRange((ulong) accountEmail.Length, 5, 100, "Email", Validate.Unit.Character);
    }

    private void ValidatePassword(string password)
    {
        Validate.OutOfRange((ulong) password.Length, 1, 512, "Password", Validate.Unit.Character);
    }
}