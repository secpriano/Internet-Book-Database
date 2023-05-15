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
        
        using SqlCommand sqlCommand = new("SELECT b.BookID, b.Isbn, b.Title, b.Synopsis, b.PublishDate, b.AmountPages, a.AuthorID AS AuthorId, a.Name AS AuthorName, p.PublisherID AS PublisherId, p.Name AS PublisherName, g.GenreID AS GenreId, g.GenreText AS GenreName, t.ThemeID AS ThemeId, t.ThemeText AS ThemeName, s.SettingID AS SettingId, s.SettingText AS SettingName FROM Book AS b LEFT JOIN BookAuthor AS ba ON b.BookID = ba.BookID LEFT JOIN Author AS a ON ba.AuthorID = a.AuthorID LEFT JOIN Publisher AS p ON b.PublisherID = p.PublisherID LEFT JOIN BookGenre AS bg ON b.BookID = bg.BookID LEFT JOIN Genre AS g ON bg.GenreID = g.GenreID LEFT JOIN BookTheme AS bt ON b.BookID = bt.BookID LEFT JOIN Theme AS t ON bt.ThemeID = t.ThemeID LEFT JOIN BookSetting AS bs ON b.BookID = bs.BookID LEFT JOIN Setting AS s ON bs.SettingID = s.SettingID", sqlConnection);
        using SqlDataReader reader = sqlCommand.ExecuteReader();
        while (reader.Read())
        {
            long? bookId = reader.IsDBNull(reader.GetOrdinal("BookId")) ? null : (long?)reader["BookId"];
            string isbn = reader.GetString(reader.GetOrdinal("Isbn"));
            string title = reader.GetString(reader.GetOrdinal("Title"));
            string synopsis = reader.GetString(reader.GetOrdinal("Synopsis"));
            DateTime publishDate = reader.GetDateTime(reader.GetOrdinal("PublishDate"));
            short amountPages = (short)reader["AmountPages"];
        
            List<AuthorDTO> authors = new();
        
            long? publisherId = reader.IsDBNull(reader.GetOrdinal("PublisherId")) ? null : (long?)reader["PublisherId"];
            string publisherName = reader.IsDBNull(reader.GetOrdinal("PublisherName")) ? null : reader.GetString(reader.GetOrdinal("PublisherName"));
            PublisherDTO publisher = new() { Id = publisherId, Name = publisherName };
        
            List<GenreDTO> genres = new();
        
            List<ThemeDTO> themes = new();
        
            List<SettingDTO> settings = new();
        
            while (bookId == (long)reader["BookId"])
            {
                long? authorId = reader.IsDBNull(reader.GetOrdinal("AuthorId")) ? null : (long?)reader["AuthorId"];
                string authorName = reader.IsDBNull(reader.GetOrdinal("AuthorName")) ? null : reader.GetString(reader.GetOrdinal("AuthorName"));
                AuthorDTO author = new() { Id = authorId, Name = authorName };
                if (authorId != null)
                {
                    authors.Add(author);
                }
                            
                byte? genreId = reader.IsDBNull(reader.GetOrdinal("GenreId")) ? null : reader["GenreId"] as byte?;
                string genreName = reader.IsDBNull(reader.GetOrdinal("GenreName")) ? null : reader.GetString(reader.GetOrdinal("GenreName"));
                if (genreId != null)
                {
                    genres.Add(new() { Id = genreId, Name = genreName });
                }
        
                byte? themeId = reader.IsDBNull(reader.GetOrdinal("ThemeId")) ? null : reader["ThemeId"] as byte?;
                string themeName = reader.IsDBNull(reader.GetOrdinal("ThemeName")) ? null : reader.GetString(reader.GetOrdinal("ThemeName"));
                if (themeId != null)
                {
                    themes.Add(new() { Id = themeId, Description = themeName });
                }
        
                byte? settingId = reader.IsDBNull(reader.GetOrdinal("SettingId")) ? null : reader["SettingId"] as byte?;
                string settingName = reader.IsDBNull(reader.GetOrdinal("SettingName")) ? null : reader.GetString(reader.GetOrdinal("SettingName"));
                if (settingId != null)
                {
                    settings.Add(new() { Id = settingId, Description = settingName });
                }
        
                if (!reader.Read())
                {
                    break;
                }
            }
        
            BookDTO book = new()
            {
                Id = bookId,
                Isbn = isbn,
                Title = title,
                Synopsis = synopsis,
                PublishDate = publishDate,
                AmountPages = (ushort) amountPages,
                Authors = authors.Distinct(),
                Publisher = publisher,
                Genres = genres.Distinct(),
                Themes = themes.Distinct(),
                Settings = settings.Distinct()
            };
        
            books.Add(book);
        }
        
        return books; 
    }
}