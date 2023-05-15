using System.Data.SqlClient;
using Interface.DTO;
using Interface.Interfaces;

namespace Data;

public class SettingData : Database, ISettingData
{
    public bool Add(SettingDTO entity)
    {
        throw new NotImplementedException();
    }

    public SettingDTO GetById(long id)
    {
        throw new NotImplementedException();
    }

    public SettingDTO Update(SettingDTO entity)
    {
        throw new NotImplementedException();
    }

    public bool Delete(long id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<SettingDTO> GetAll()
    {
        using SqlConnection sqlConnection = new(ConnectionString);
        sqlConnection.Open();
        using SqlCommand sqlCommand = new("SELECT * FROM Setting", sqlConnection);
        using SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
        
        List<SettingDTO> settings = new();
        
        while (sqlDataReader.Read())
        {
            settings.Add(new(
                sqlDataReader.GetByte(0),
                sqlDataReader.GetString(1)
            ));
        }
        return settings;
    }
}