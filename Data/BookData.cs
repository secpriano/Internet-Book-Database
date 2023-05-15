using System.Data;
using System.Data.SqlClient;
using Interface.DTO;
using Interface.Interfaces;

namespace Data;

public sealed class BookData : Database, IBookData
{
public bool Add(BookDTO entity)
{
    using SqlConnection sqlConnection = new(ConnectionString);
    sqlConnection.Open();
    using SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
    try
    {
        using SqlCommand sqlCommand = new(
            "INSERT INTO Book (Title, ISBN, AmountPages, Synopsis, PublishDate, PublisherID) " +
            "VALUES (@Title, @ISBN, @AmountPages, @Synopsis, @PublishDate, @PublisherID);" +
            "SELECT SCOPE_IDENTITY()",
            sqlConnection,
            sqlTransaction)
        {
            Parameters =
            {
                new("@Title", entity.Title),
                new("@ISBN", entity.Isbn),
                new("@AmountPages", (long)entity.AmountPages),
                new("@Synopsis", entity.Synopsis),
                new("@PublishDate", entity.PublishDate),
                new("@PublisherID", entity.Publisher.Id)
            }
        };
        
        decimal bookId = (decimal) sqlCommand.ExecuteScalar();

        AddItemsToTable(entity.Genres, "BookGenre", (long)bookId, "GenreID", sqlCommand);
        AddItemsToTable(entity.Themes, "BookTheme", (long)bookId, "ThemeID", sqlCommand);
        AddItemsToTable(entity.Settings, "BookSetting", (long)bookId, "SettingID", sqlCommand);
        AddItemsToTable(entity.Authors, "BookAuthor", (long)bookId, "AuthorID", sqlCommand);

        sqlTransaction.Commit();
        return true;
    }
    catch (Exception ex)
    {
        sqlTransaction.Rollback();
        throw ex;
    }
}

private void AddItemsToTable<T>(IEnumerable<T> items, string tableName, long bookId, string columnName, SqlCommand command)
{
    command.CommandText = $"INSERT INTO {tableName} (BookID, {columnName}) VALUES (@BookID, @ID)";
    command.Parameters.Clear();
    command.Parameters.AddWithValue("@BookID", bookId);
    command.Parameters.Add("@ID", SqlDbType.Int);
    foreach (var item in items)
    {
        command.Parameters["@ID"].Value = typeof(T).GetProperty("Id").GetValue(item);
        command.ExecuteNonQuery();
    }
}

    public BookDTO GetById(long id)
    {
        throw new NotImplementedException();
    }

    public BookDTO Update(BookDTO entity)
    {
        throw new NotImplementedException();
    }

    public bool Delete(long id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<BookDTO> GetAll()
    {
        List<BookDTO> books = new();
        
        using SqlConnection sqlConnection = new(ConnectionString);
        sqlConnection.Open();
        
        using SqlCommand sqlCommand = new("SELECT b.BookID, b.Isbn, b.Title, b.Synopsis, b.PublishDate, b.AmountPages, a.AuthorID AS AuthorId, a.Name AS AuthorName, a.Description AS AuthorDescription, a.BirthDate, a.DeathDate, ga.GenreID AS AuthorGenreID,ga.GenreText AS AuthorGenreName,p.PublisherID AS PublisherId, p.Name AS PublisherName, g.GenreID AS GenreId, g.GenreText AS GenreName, t.ThemeID AS ThemeId, t.ThemeText AS ThemeName, s.SettingID AS SettingId, s.SettingText AS SettingName FROM Book AS b LEFT JOIN BookAuthor AS ba ON b.BookID = ba.BookID LEFT JOIN Author AS a ON ba.AuthorID = a.AuthorID LEFT JOIN AuthorGenre AS ag ON a.AuthorID = ag.AuthorID LEFT JOIN Genre AS ga ON ag.GenreID = ga.GenreID LEFT JOIN Publisher AS p ON b.PublisherID = p.PublisherID LEFT JOIN BookGenre AS bg ON b.BookID = bg.BookID LEFT JOIN Genre AS g ON bg.GenreID = g.GenreID LEFT JOIN BookTheme AS bt ON b.BookID = bt.BookID LEFT JOIN Theme AS t ON bt.ThemeID = t.ThemeID LEFT JOIN BookSetting AS bs ON b.BookID = bs.BookID LEFT JOIN Setting AS s ON bs.SettingID = s.SettingID", sqlConnection);
        using SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
        while (sqlDataReader.Read())
        {
            long? bookId = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("BookId")) ? null : (long?)sqlDataReader["BookId"];
            string isbn = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Isbn"));
            string title = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Title"));
            string synopsis = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Synopsis"));
            DateTime publishDate = sqlDataReader.GetDateTime(sqlDataReader.GetOrdinal("PublishDate"));
            short amountPages = (short)sqlDataReader["AmountPages"];
        
            Dictionary<long, AuthorDTO> authors = new();

            long? publisherId = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("PublisherId")) ? null : (long?)sqlDataReader["PublisherId"];
            string publisherName = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("PublisherName")) ? null : sqlDataReader.GetString(sqlDataReader.GetOrdinal("PublisherName"));
            PublisherDTO publisher = new() { Id = publisherId, Name = publisherName };
        
            List<GenreDTO> genres = new();
        
            List<ThemeDTO> themes = new();
        
            List<SettingDTO> settings = new();
        
            while (bookId == (long)sqlDataReader["BookId"])
            {
                long? authorId = (long?)sqlDataReader["AuthorId"];
                if (!authors.ContainsKey((long)authorId))
                {
                    string authorName = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("AuthorName")) ? null : sqlDataReader.GetString(sqlDataReader.GetOrdinal("AuthorName"));
                    string authorDescription = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("AuthorDescription")) ? null : sqlDataReader.GetString(sqlDataReader.GetOrdinal("AuthorDescription"));
                    DateTime authorBirthDate = sqlDataReader.GetDateTime(sqlDataReader.GetOrdinal("BirthDate"));
                    DateTime? authorDeathDate = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("DeathDate")) ? null : (DateTime?)sqlDataReader["DeathDate"];
                    authors[(long)authorId] = new()
                    {
                        Id = authorId, 
                        Name = authorName, 
                        Description = authorDescription,
                        BirthDate = DateOnly.FromDateTime(authorBirthDate),
                        DeathDate = authorDeathDate != null ? DateOnly.FromDateTime((DateTime)authorDeathDate) : null,
                        Genres = new List<GenreDTO>()
                    };
                }
                byte authorGenreId = (byte)sqlDataReader["AuthorGenreId"];
                string authorGenreName = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("AuthorGenreName")) ? null : sqlDataReader.GetString(sqlDataReader.GetOrdinal("AuthorGenreName"));
                
                authors[(long)authorId].Genres.ToList().Add(new GenreDTO(authorGenreId as byte?, authorGenreName));
                            
                byte? genreId = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("GenreId")) ? null : sqlDataReader["GenreId"] as byte?;
                string genreName = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("GenreName")) ? null : sqlDataReader.GetString(sqlDataReader.GetOrdinal("GenreName"));
                if (genreId != null)
                {
                    genres.Add(new() { Id = genreId, Name = genreName });
                }
        
                byte? themeId = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("ThemeId")) ? null : sqlDataReader["ThemeId"] as byte?;
                string themeName = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("ThemeName")) ? null : sqlDataReader.GetString(sqlDataReader.GetOrdinal("ThemeName"));
                if (themeId != null)
                {
                    themes.Add(new() { Id = themeId, Description = themeName });
                }
        
                byte? settingId = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("SettingId")) ? null : sqlDataReader["SettingId"] as byte?;
                string settingName = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("SettingName")) ? null : sqlDataReader.GetString(sqlDataReader.GetOrdinal("SettingName"));
                if (settingId != null)
                {
                    settings.Add(new() { Id = settingId, Description = settingName });
                }
        
                if (!sqlDataReader.Read())
                {
                    break;
                }
            }
        
            books.Add(new()
            {
                Id = bookId,
                Isbn = isbn,
                Title = title,
                Synopsis = synopsis,
                PublishDate = DateOnly.FromDateTime(publishDate),
                AmountPages = (ushort) amountPages,
                Authors = authors.Values.ToList(),
                Publisher = publisher,
                Genres = genres.Distinct(),
                Themes = themes.Distinct(),
                Settings = settings.Distinct()
            });
        }
        
        return books; 
    }
}