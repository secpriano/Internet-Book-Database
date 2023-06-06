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
                new("@PublishDate", entity.PublishDate.ToDateTime(TimeOnly.FromDateTime(DateTime.Now))),
                new("@PublisherID", entity.Publisher.Id)
            }
        };
        
        decimal bookId = (decimal) sqlCommand.ExecuteScalar();

        AddItemsToTable(entity.Themes, "BookTheme", (long)bookId, "ThemeID", sqlCommand);
        AddItemsToTable(entity.Settings, "BookSetting", (long)bookId, "SettingID", sqlCommand);
        AddItemsToTable(entity.Genres, "BookGenre", (long)bookId, "GenreID", sqlCommand);
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

private static void AddItemsToTable<T>(IEnumerable<T> items, string tableName, long bookId, string columnName, SqlCommand command)
{
    command.CommandText = $"INSERT INTO {tableName} (BookID, {columnName}) VALUES (@BookID, @ID)";
    command.Parameters.Clear();
    command.Parameters.AddWithValue("@BookID", bookId);
    command.Parameters.Add("@ID", SqlDbType.Int);
    foreach (T item in items)
    {
        command.Parameters["@ID"].Value = typeof(T).GetProperty("Id").GetValue(item);
        command.ExecuteNonQuery();
    }
}

private static void EditItemsInTable<T>(IEnumerable<T> items, string tableName, long bookId, string columnName,
    SqlCommand command)
{
    command.CommandText = $"UPDATE {tableName} SET {columnName} = @ID WHERE BookID = @BookID";
    command.Parameters.Clear();
    command.Parameters.AddWithValue("@BookID", bookId);
    command.Parameters.Add("@ID", SqlDbType.Int);
    foreach (T item in items)
    {
        command.Parameters["@ID"].Value = typeof(T).GetProperty("Id").GetValue(item);
        command.ExecuteNonQuery();
    }
}

    public BookDTO GetById(long id)
    {
        using SqlConnection sqlConnection = new(ConnectionString);
        sqlConnection.Open();

        using SqlCommand sqlCommand = new(
            "SELECT BookID, Title ,ISBN ,AmountPages ,Synopsis ,PublishDate ,Book.PublisherID ,Publisher.Name ,Publisher.Description ,Publisher.FoundingDate " +
                                          "FROM Book " +
                                          "JOIN Publisher ON Book.PublisherID = Publisher.PublisherID " +
                                          "WHERE BookID = @BookID", sqlConnection);
        
        sqlCommand.Parameters.AddWithValue("@BookID", id);
        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

        BookDTO book = new();

        while (sqlDataReader.Read())
        {
            book.Id = sqlDataReader.GetInt64(sqlDataReader.GetOrdinal("BookId"));
            book.Isbn = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Isbn"));
            book.Title = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Title"));
            book.Synopsis = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Synopsis"));
            book.PublishDate = DateOnly.FromDateTime(sqlDataReader.GetDateTime(sqlDataReader.GetOrdinal("PublishDate")));
            book.AmountPages = (ushort)sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("AmountPages"));
            book.Publisher = new()
            {
                Id = sqlDataReader.GetInt64(sqlDataReader.GetOrdinal("PublisherId")),
                Name = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Name")),
                FoundingDate = DateOnly.FromDateTime(sqlDataReader.GetDateTime(sqlDataReader.GetOrdinal("FoundingDate"))),
                Description = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Description"))
            };
        }
        
        sqlConnection.Close();
        sqlConnection.Open();

        sqlCommand.CommandText = "SELECT BookAuthor.AuthorID, Author.Name, Author.Description, Author.BirthDate, Author.DeathDate " +
                                 "FROM BookAuthor " +
                                 "JOIN Author ON BookAuthor.AuthorID = Author.AuthorID " +
                                 "WHERE BookID = @BookID";
        sqlCommand.Parameters.Clear();
        sqlCommand.Parameters.AddWithValue("@BookID", id);
        
        sqlDataReader = sqlCommand.ExecuteReader();
        
        List<AuthorDTO> authors = new();
        while (sqlDataReader.Read())
        {
                
            AuthorDTO authorDto = new()
            {
                Id = sqlDataReader.GetInt64(sqlDataReader.GetOrdinal("AuthorID")),
                Name = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Name")),
                Description = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Description")),
                BirthDate = DateOnly.FromDateTime(sqlDataReader.GetDateTime(sqlDataReader.GetOrdinal("BirthDate"))),
                DeathDate = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("DeathDate"))
                    ? null
                    : DateOnly.FromDateTime(sqlDataReader.GetDateTime(sqlDataReader.GetOrdinal("DeathDate"))),
                Genres = new()
            };
            authors.Add(authorDto);
        }
        book.Authors = authors;
        
        sqlCommand.CommandText = "SELECT BookGenre.GenreID, Genre.GenreText " +
                                 "FROM BookGenre " +
                                 "JOIN Genre ON BookGenre.GenreID = Genre.GenreID " +
                                 "WHERE BookID = @BookID";
        sqlCommand.Parameters.Clear();
        sqlCommand.Parameters.AddWithValue("@BookID", id);
        
        sqlConnection.Close();
        sqlConnection.Open();
        
        sqlDataReader = sqlCommand.ExecuteReader();
        
        List<GenreDTO> genres = new();
        while (sqlDataReader.Read())
        {
            byte genreId = sqlDataReader.GetByte(sqlDataReader.GetOrdinal("GenreID"));
            string genreText = sqlDataReader.GetString(sqlDataReader.GetOrdinal("GenreText"));

            GenreDTO genre = new()
            {
                Id = genreId,
                Name = genreText
            };
            
            genres.Add(genre);
        }
        book.Genres = genres;
        
        sqlCommand.CommandText = "SELECT BookSetting.SettingID, Setting.SettingText " +
                                 "FROM BookSetting " +
                                 "JOIN Setting ON BookSetting.SettingID = Setting.SettingID " +
                                 "WHERE BookID = @BookID";
        sqlCommand.Parameters.Clear();
        sqlCommand.Parameters.AddWithValue("@BookID", id);
        
        sqlConnection.Close();
        sqlConnection.Open();
        
        sqlDataReader = sqlCommand.ExecuteReader();
        
        List<SettingDTO> settings = new();
        while (sqlDataReader.Read())
        {
            SettingDTO settingDto = new()
            {
                Id = sqlDataReader.GetByte(sqlDataReader.GetOrdinal("SettingID")),
                Description = sqlDataReader.GetString(sqlDataReader.GetOrdinal("SettingText"))
            };
            
            settings.Add(settingDto);
        }
        book.Settings = settings;
        
        sqlCommand.CommandText = "SELECT BookTheme.ThemeID, Theme.ThemeText " +
                                 "FROM BookTheme " +
                                 "JOIN Theme ON BookTheme.ThemeID = Theme.ThemeID " +
                                 "WHERE BookID = @BookID";
        sqlCommand.Parameters.Clear();
        sqlCommand.Parameters.AddWithValue("@BookID", id);
        
        sqlConnection.Close();
        sqlConnection.Open();
        
        sqlDataReader = sqlCommand.ExecuteReader();
        
        List<ThemeDTO> themes = new();
        while (sqlDataReader.Read())
        {
            ThemeDTO themeDto = new()
            {
                Id = sqlDataReader.GetByte(sqlDataReader.GetOrdinal("ThemeID")),
                Description = sqlDataReader.GetString(sqlDataReader.GetOrdinal("ThemeText"))
            };
            
            themes.Add(themeDto);
        }
        book.Themes = themes;

        sqlCommand.CommandText = "SELECT COUNT_BIG(*) FROM UserBookFavorite WHERE BookID = @BookID";
        sqlCommand.Parameters.Clear();
        sqlCommand.Parameters.AddWithValue("@BookID", id);
        
        sqlConnection.Close();
        sqlConnection.Open();
        
        long countedRows = (long)sqlCommand.ExecuteScalar();
        book.Favorites = (ulong)countedRows;

        sqlConnection.Close();
        return book;
    }

    public BookDTO Update(BookDTO entity)
    {
        using SqlConnection sqlConnection = new(ConnectionString);
        sqlConnection.Open();
        using SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
        try
        {
            using SqlCommand sqlCommand = new("UPDATE Book SET Title = @Title, Isbn = @Isbn, AmountPages = @AmountPages, Synopsis = @Synopsis, PublishDate = @PublishDate, PublisherID = @PublisherID WHERE BookID = @BookID", sqlConnection, sqlTransaction)
            {
                Parameters =
                {
                    new("@Title", entity.Title),
                    new("@ISBN", entity.Isbn),
                    new("@AmountPages", (long)entity.AmountPages),
                    new("@Synopsis", entity.Synopsis),
                    new("@PublishDate", entity.PublishDate.ToDateTime(TimeOnly.FromDateTime(DateTime.Now))),
                    new("@PublisherID", entity.Publisher.Id),
                    new("@BookID", entity.Id)
                }
            };
            
            sqlCommand.ExecuteNonQuery();
            
            EditItemsInTable(entity.Genres, "BookGenre", entity.Id.Value, "GenreID", sqlCommand);
            EditItemsInTable(entity.Themes, "BookTheme", entity.Id.Value, "ThemeID", sqlCommand);
            EditItemsInTable(entity.Settings, "BookSetting", entity.Id.Value, "SettingID", sqlCommand);
            EditItemsInTable(entity.Authors, "BookAuthor", entity.Id.Value, "AuthorID", sqlCommand);

            sqlTransaction.Commit();
            return entity;
        }
        catch (Exception ex)
        {
            sqlTransaction.Rollback();
            throw ex;
        }
    }

    public bool Delete(long id)
    {
        using SqlConnection sqlConnection = new(ConnectionString);
        sqlConnection.Open();
        
        using SqlCommand sqlCommand = new("DELETE FROM Book WHERE BookID = @BookID", sqlConnection);
        sqlCommand.Parameters.AddWithValue("@BookID", id);
        
        return sqlCommand.ExecuteNonQuery() > 0;
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
                        Genres = new()
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

    public bool Favorite(long bookId, long userId)
    {
        using SqlConnection sqlConnection = new(ConnectionString);
        sqlConnection.Open();
        using SqlCommand sqlCommand = new("INSERT INTO UserBookFavorite (UserID ,BookID) VALUES (@UserID, @BookID)", sqlConnection);
        sqlCommand.Parameters.AddWithValue("@UserID", userId);
        sqlCommand.Parameters.AddWithValue("@BookID", bookId);
        
        return sqlCommand.ExecuteNonQuery() > 0;
    }

    public bool Unfavorite(long bookId, long userId)
    {
        using SqlConnection sqlConnection = new(ConnectionString);
        sqlConnection.Open();
        using SqlCommand sqlCommand = new("DELETE FROM UserBookFavorite WHERE UserID = @UserID AND BookID = @BookID", sqlConnection);
        sqlCommand.Parameters.AddWithValue("@UserID", userId);
        sqlCommand.Parameters.AddWithValue("@BookID", bookId);
        
        return sqlCommand.ExecuteNonQuery() > 0;
    }

    public bool IsFavorite(long bookId, long userId)
    {
        using SqlConnection sqlConnection = new(ConnectionString);
        sqlConnection.Open();
        using SqlCommand sqlCommand = new("SELECT COUNT(*) FROM UserBookFavorite WHERE UserID = @UserID AND BookID = @BookID", sqlConnection);
        sqlCommand.Parameters.AddWithValue("@UserID", userId);
        sqlCommand.Parameters.AddWithValue("@BookID", bookId);
        
        return (int)sqlCommand.ExecuteScalar() > 0;
    }
}