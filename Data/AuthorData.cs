using System.Data.SqlClient;
using Interface.DTO;
using Interface.Interfaces;

namespace Data;

public class AuthorData : Database, IAuthorData
{
    public bool Add(AuthorDTO entity)
    {
        throw new NotImplementedException();
    }

    public AuthorDTO GetById(long id)
    {
        throw new NotImplementedException();
    }

    public AuthorDTO Update(AuthorDTO entity)
    {
        throw new NotImplementedException();
    }

    public bool Delete(long id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<AuthorDTO> GetAll()
    {
        SqlConnection sqlConnection = new(ConnectionString);
        sqlConnection.Open();
        SqlCommand sqlCommand = new("SELECT * FROM Author", sqlConnection);
        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
        
        List<AuthorDTO> authors = new();    
        
        while (sqlDataReader.Read())
        {
            authors.Add(new(
                sqlDataReader.GetInt64(0),
                sqlDataReader.GetString(1),
                sqlDataReader.GetString(2),
                sqlDataReader.GetDateTime(3),
                sqlDataReader.GetDateTime(4)
            ));
        }
        return authors;
    }
}