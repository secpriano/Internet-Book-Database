using System.Data.SqlClient;
using Interface.DTO;
using Interface.Interfaces;

namespace Data.MsSQL;

public class AccountData : Database, IAccountData
{
    public AccountDTO Login(AccountDTO accountDTO, string password)
    {
        SqlConnection sqlConnection = new(ConnectionString);
        sqlConnection.Open();
            
        using SqlCommand sqlCommand = new(
            "SELECT * FROM Account WHERE Email = @Email AND Password = @Password",
            sqlConnection)
        {
            Parameters =
            {
                new("@Email", accountDTO.Email),
                new("@Password", password)
            }
        };
            
        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
        
        if (!sqlDataReader.HasRows)
        {
            throw new("Invalid email or password.");
        }

        AccountDTO accountDto = null;
        while (sqlDataReader.Read())
        {
            accountDto = new(
                sqlDataReader.GetInt64(sqlDataReader.GetOrdinal("AccountId")),
                sqlDataReader.GetString(sqlDataReader.GetOrdinal("Username")),
                sqlDataReader.GetString(sqlDataReader.GetOrdinal("Email")),
                sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("IsAdmin"))
            );
        }

        return accountDto;
    }
}