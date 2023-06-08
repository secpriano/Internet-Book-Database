using System.Data.SqlClient;
using Interface.DTO;
using Interface.Interfaces;

namespace Data.MsSQL;

public class PublisherData : Database, IPublisherData
{
    public bool Add(PublisherDTO entity)
    {
        using SqlConnection sqlConnection = new(ConnectionString);
        sqlConnection.Open();
        using SqlCommand sqlCommand = new(
            "INSERT INTO Publisher (Name, FoundingDate, Description) " +
            "VALUES (@Name, @FoundingDate, @Description);",
            sqlConnection)
        {
            Parameters =
            {
                new("@Name", entity.Name),
                new("@FoundingDate", entity.FoundingDate.ToDateTime(TimeOnly.FromDateTime(DateTime.Now))),
                new("@Description", entity.Description)
            }
        };
            
        return sqlCommand.ExecuteNonQuery() > 0;
    }

    public PublisherDTO GetById(long id)
    {
        throw new NotImplementedException();
    }

    public PublisherDTO Update(PublisherDTO entity)
    {
        throw new NotImplementedException();
    }

    public bool Delete(long id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<PublisherDTO> GetAll()
    {
        using SqlConnection sqlConnection = new(ConnectionString);
        sqlConnection.Open();
        using SqlCommand sqlCommand = new("SELECT * FROM Publisher", sqlConnection);
        using SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
        
        List<PublisherDTO> publishers = new();
        
        while (sqlDataReader.Read())
        {
            publishers.Add(new(
                sqlDataReader.GetInt64(0),
                sqlDataReader.GetString(1),
                DateOnly.FromDateTime(sqlDataReader.GetDateTime(2)),
                sqlDataReader.GetString(3)
            ));
        }
        
        return publishers;
    }
}