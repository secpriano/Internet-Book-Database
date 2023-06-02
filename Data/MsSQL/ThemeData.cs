using System.Data.SqlClient;
using Interface.DTO;
using Interface.Interfaces;

namespace Data;

public class ThemeData : Database, IThemeData
{
    public bool Add(ThemeDTO entity)
    {
        using SqlConnection sqlConnection = new(ConnectionString);
        sqlConnection.Open();
        
        using SqlCommand sqlCommand = new(
            "INSERT INTO Theme (ThemeText) " +
            "VALUES (@ThemeText);",
            sqlConnection)
        {
            Parameters =
            {
                new("@ThemeText", entity.Description)
            }
        };
        
        return sqlCommand.ExecuteNonQuery() > 0;
    }

    public ThemeDTO GetById(long id)
    {
        throw new NotImplementedException();
    }

    public ThemeDTO Update(ThemeDTO entity)
    {
        throw new NotImplementedException();
    }

    public bool Delete(long id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<ThemeDTO> GetAll()
    {
        using SqlConnection sqlConnection = new(ConnectionString);
        sqlConnection.Open();
        using SqlCommand sqlCommand = new("SELECT * FROM Theme", sqlConnection);
        using SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
        
        List<ThemeDTO> themes = new();
        
        while (sqlDataReader.Read())
        {
            themes.Add(new(
                sqlDataReader.GetByte(0),
                sqlDataReader.GetString(1)
            ));
        }
        return themes;
    }
}