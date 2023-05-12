﻿using System.Data.SqlClient;
using Interface.DTO;
using Interface.Interfaces;

namespace Data;

public class GenreData : Database, IGenreData
{
    public bool Add(GenreDTO entity)
    {
        throw new NotImplementedException();
    }

    public GenreDTO GetById(long id)
    {
        throw new NotImplementedException();
    }

    public GenreDTO Update(GenreDTO entity)
    {
        throw new NotImplementedException();
    }

    public bool Delete(long id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<GenreDTO> GetAll()
    {
        SqlConnection sqlConnection = new(ConnectionString);
        sqlConnection.Open();
        SqlCommand sqlCommand = new("SELECT * FROM Genre", sqlConnection);
        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
        
        List<GenreDTO> genres = new();
        
        while (sqlDataReader.Read())
        {
            genres.Add(new(
                sqlDataReader.GetInt64(0),
                sqlDataReader.GetString(1)
            ));
        }
        
        return genres;
    }
}