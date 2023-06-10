using System.Collections;
using System.Data;
using System.Data.SqlClient;
using Interface.DTO;
using Interface.Interfaces;

namespace Data.MsSQL;

public class AuthorData : Database, IAuthorData
{
    public bool Add(AuthorDTO entity)
    {
        using SqlConnection sqlConnection = new(ConnectionString);
        sqlConnection.Open();
        using SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
        try
        {
            using SqlCommand sqlCommand = new(
                "INSERT INTO Author (Name, Description, BirthDate, DeathDate) " +
                "VALUES (@Name, @Description, @BirthDate, @DeathDate)" +
                "SELECT SCOPE_IDENTITY()",
                sqlConnection, sqlTransaction)
            {
                Parameters =
                {
                    new("@Name", entity.Name),
                    new("@Description", entity.Description),
                    new("@BirthDate", entity.BirthDate.ToDateTime(TimeOnly.FromDateTime(DateTime.Now))),
                    new("@DeathDate", entity.DeathDate != null ? entity.DeathDate?.ToDateTime(TimeOnly.FromDateTime(DateTime.Now)) : DBNull.Value)
                }
            };

            decimal authorId = (decimal)sqlCommand.ExecuteScalar();
            
            AddItemsToTable(entity.Genres, "AuthorGenre", (long)authorId, "GenreID", sqlCommand);
            
            sqlTransaction.Commit();
            return true;
        }
        catch (Exception ex)
        {
            sqlTransaction.Rollback();
            throw ex;
        }
    }
    
    private static void AddItemsToTable<T>(IEnumerable<T> items, string tableName, long authorId, string columnName, SqlCommand command)
    {
        command.CommandText = $"INSERT INTO {tableName} (AuthorID, {columnName}) VALUES (@AuthorID, @ID)";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@AuthorID", authorId);
        command.Parameters.Add("@ID", SqlDbType.Int);
        foreach (T item in items)
        {
            command.Parameters["@ID"].Value = typeof(T).GetProperty("Id").GetValue(item);
            command.ExecuteNonQuery();
        }
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

    public IEnumerable<AuthorDTO> GetByIds(IEnumerable<byte> authorIds)
    {
        using SqlConnection sqlConnection = new(ConnectionString);
        sqlConnection.Open();
        using SqlCommand sqlCommand = new();
        sqlCommand.Connection = sqlConnection;

        List<AuthorDTO> authors = new();
        
        foreach (byte AddedAuthorId in authorIds)
        {
            sqlCommand.CommandText = "SELECT Author.AuthorID, Name, Description, BirthDate, DeathDate, AuthorGenre.GenreID, GenreText FROM Author LEFT JOIN AuthorGenre ON Author.AuthorID = AuthorGenre.AuthorID LEFT JOIN Genre ON AuthorGenre.GenreID = Genre.GenreID WHERE Author.AuthorID IN (@AuthorID)";
            sqlCommand.Parameters.AddWithValue("@AuthorID", AddedAuthorId);
            
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            
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
            
            sqlCommand.Parameters.Clear();
        }
        
        return authors;
    }

    public bool Exist(string name)
    {
        using SqlConnection sqlConnection = new(ConnectionString);
        sqlConnection.Open();
        
        using SqlCommand sqlCommand = new("SELECT COUNT(*) FROM Author WHERE Name = @Name", sqlConnection);
        sqlCommand.Parameters.AddWithValue("@Name", name);
        
        return (int)sqlCommand.ExecuteScalar() > 0;
    }
}