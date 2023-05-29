using System.Collections;
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
        List<AuthorDTO> authors = new();
        
        using SqlConnection sqlConnection = new(ConnectionString);
        sqlConnection.Open();
        using SqlCommand sqlCommand = new("SELECT Author.AuthorID, Name, Description, BirthDate, DeathDate, AuthorGenre.GenreID, GenreText FROM Author LEFT JOIN AuthorGenre ON Author.AuthorID = AuthorGenre.AuthorID LEFT JOIN Genre ON AuthorGenre.GenreID = Genre.GenreID", sqlConnection);
        using SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
        
        while (sqlDataReader.Read())
        {
            long? authorId = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("AuthorID")) ? null : (long?)sqlDataReader["AuthorID"];
            string Name = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Name"));
            string Description = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Description"));
            DateTime BirthDate = sqlDataReader.GetDateTime(sqlDataReader.GetOrdinal("BirthDate"));
            DateTime? DeathDate = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("DeathDate")) ? null : (DateTime?)sqlDataReader["DeathDate"];
            
            List<GenreDTO> genres = new();
            
            while (authorId == (long)sqlDataReader["AuthorID"])
            {
                byte? genreId = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("GenreId")) ? null : sqlDataReader["GenreId"] as byte?;
                string genreName = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("GenreText")) ? null : sqlDataReader.GetString(sqlDataReader.GetOrdinal("GenreText"));
                if (genreId != null)
                {
                    genres.Add(new(genreId, genreName));
                }

                if (!sqlDataReader.Read()) break;
            }
            
            authors.Add(new(
                authorId,
                Name,
                Description,
                DateOnly.FromDateTime(BirthDate),
                DeathDate != null ? DateOnly.FromDateTime((DateTime)DeathDate) : null,
                genres
            ));
        }

        return authors;
    }
}