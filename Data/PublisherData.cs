using System.Data.SqlClient;
using Interface.DTO;
using Interface.Interfaces;

namespace Data;

public class PublisherData : Database, IPublisherData
{
    public bool Add(PublisherDTO entity)
    {
        throw new NotImplementedException();
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
        SqlConnection sqlConnection = new(ConnectionString);
        sqlConnection.Open();
        SqlCommand sqlCommand = new("SELECT * FROM Publisher", sqlConnection);
        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
        
        List<PublisherDTO> publishers = new();
        
        while (sqlDataReader.Read())
        {
            publishers.Add(new(
                sqlDataReader.GetInt64(0),
                sqlDataReader.GetString(1),
                sqlDataReader.GetDateTime(2),
                sqlDataReader.GetString(3)
            ));
        }
        
        return publishers;
    }
}