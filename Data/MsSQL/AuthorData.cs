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
        using SqlCommand sqlCommand = new("SELECT Author.AuthorID, Name, Description, BirthDate, DeathDate FROM Author", sqlConnection);
        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
        
        while (sqlDataReader.Read())
        {
            long? authorId = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("AuthorID")) ? null : (long?)sqlDataReader["AuthorID"];
            string Name = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Name"));
            string Description = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Description"));
            DateTime BirthDate = sqlDataReader.GetDateTime(sqlDataReader.GetOrdinal("BirthDate"));
            DateTime? DeathDate = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("DeathDate")) ? null : (DateTime?)sqlDataReader["DeathDate"];

            authors.Add(new(
                authorId,
                Name,
                Description,
                DateOnly.FromDateTime(BirthDate),
                DeathDate != null ? DateOnly.FromDateTime((DateTime)DeathDate) : null,
                new()
            ));
        }
        
        sqlDataReader.Close();
        

        foreach (AuthorDTO author in authors)
        {
            sqlCommand.Parameters.Clear();
            sqlCommand.CommandText = "SELECT Genre.GenreID, GenreText FROM AuthorGenre LEFT JOIN Genre ON AuthorGenre.GenreID = Genre.GenreID WHERE AuthorID = @AuthorID";
            sqlCommand.Parameters.AddWithValue("@AuthorID", author.Id);
            
            sqlDataReader = sqlCommand.ExecuteReader();
            
            while (sqlDataReader.Read())
            {
                byte? genreId = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("GenreId")) ? null : sqlDataReader["GenreId"] as byte?;
                string genreName = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("GenreText")) ? null : sqlDataReader.GetString(sqlDataReader.GetOrdinal("GenreText"));
                author.Genres.Add(new(genreId, genreName));
            }
            
            sqlDataReader.Close();
        }

        return authors;
    }

    public IEnumerable<AuthorDTO> GetByIds(IEnumerable<long> authorIds)
    {
        using SqlConnection sqlConnection = new(ConnectionString);
        sqlConnection.Open();
        using SqlCommand sqlCommand = new();
        sqlCommand.Connection = sqlConnection;

        List<AuthorDTO> authors = new();

        SqlDataReader sqlDataReader;
            
        foreach (long authorIdentity in authorIds)
        {
            sqlCommand.Parameters.Clear();
            sqlCommand.CommandText = "SELECT Author.AuthorID, Name, Description, BirthDate, DeathDate FROM Author WHERE AuthorID = @AuthorID";
            sqlCommand.Parameters.AddWithValue("@AuthorID", authorIdentity);
            
            sqlDataReader = sqlCommand.ExecuteReader();

            while (sqlDataReader.Read())
            {
                long? authorId = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("AuthorID"))
                    ? null
                    : (long?) sqlDataReader["AuthorID"];
                string Name = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Name"));
                string Description = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Description"));
                DateTime BirthDate = sqlDataReader.GetDateTime(sqlDataReader.GetOrdinal("BirthDate"));
                DateTime? DeathDate = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("DeathDate"))
                    ? null
                    : (DateTime?) sqlDataReader["DeathDate"];

                authors.Add(new(
                    authorId,
                    Name,
                    Description,
                    DateOnly.FromDateTime(BirthDate),
                    DeathDate != null ? DateOnly.FromDateTime((DateTime) DeathDate) : null,
                    new()
                ));
            }
            sqlDataReader.Close();
        }

        sqlCommand.Parameters.Clear();
            
            
        
        foreach (AuthorDTO author in authors)
        {
            sqlCommand.Parameters.Clear();
            sqlCommand.CommandText = "SELECT Genre.GenreID, GenreText FROM AuthorGenre LEFT JOIN Genre ON AuthorGenre.GenreID = Genre.GenreID WHERE AuthorID = @AuthorID";
            sqlCommand.Parameters.AddWithValue("@AuthorID", author.Id);
            
            sqlDataReader = sqlCommand.ExecuteReader();
            
            while (sqlDataReader.Read())
            {
                byte? genreId = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("GenreId")) ? null : sqlDataReader["GenreId"] as byte?;
                string genreName = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("GenreText")) ? null : sqlDataReader.GetString(sqlDataReader.GetOrdinal("GenreText"));
                author.Genres.Add(new(genreId, genreName));
            }
            
            sqlDataReader.Close();
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