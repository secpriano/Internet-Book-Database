using System.Data;
using System.Data.SqlClient;
using Interface.DTO;
using Interface.Interfaces;

namespace Data.MsSQL;

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

        long? bookId = null;
        string isbn = null;
        string title = null;
        string synopsis = null;
        DateOnly publishDate = default;
        short amountPages = 0;
        PublisherDTO publisherDto = new();

        while (sqlDataReader.Read())
        {
            bookId = sqlDataReader.GetInt64(sqlDataReader.GetOrdinal("BookId"));
            isbn = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Isbn"));
            title = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Title"));
            synopsis = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Synopsis"));
            publishDate = DateOnly.FromDateTime(sqlDataReader.GetDateTime(sqlDataReader.GetOrdinal("PublishDate")));
            amountPages = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("AmountPages"));
            publisherDto = new()
            {
                Id = sqlDataReader.GetInt64(sqlDataReader.GetOrdinal("PublisherId")),
                Name = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Name")),
                FoundingDate = DateOnly.FromDateTime(sqlDataReader.GetDateTime(sqlDataReader.GetOrdinal("FoundingDate"))),
                Description = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Description"))
            };
        }
        BookDTO book = new(
            bookId,
            isbn,
            title,
            synopsis,
            publishDate,
            (ushort) amountPages,
            new(),
            publisherDto,
            new(),
            new(),
            new(),
            0
            );
        
        sqlConnection.Close();
        sqlConnection.Open();

        sqlCommand.CommandText = "SELECT BookAuthor.AuthorID, Author.Name, Author.Description, Author.BirthDate, Author.DeathDate " +
                                 "FROM BookAuthor " +
                                 "JOIN Author ON BookAuthor.AuthorID = Author.AuthorID " +
                                 "WHERE BookID = @BookID";
        sqlCommand.Parameters.Clear();
        sqlCommand.Parameters.AddWithValue("@BookID", id);
        
        sqlDataReader = sqlCommand.ExecuteReader();
        
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
            book.Authors.Add(authorDto);
        }
        
        sqlCommand.CommandText = "SELECT BookGenre.GenreID, Genre.GenreText " +
                                 "FROM BookGenre " +
                                 "JOIN Genre ON BookGenre.GenreID = Genre.GenreID " +
                                 "WHERE BookID = @BookID";
        sqlCommand.Parameters.Clear();
        sqlCommand.Parameters.AddWithValue("@BookID", id);
        
        sqlConnection.Close();
        sqlConnection.Open();
        
        sqlDataReader = sqlCommand.ExecuteReader();
        
        while (sqlDataReader.Read())
        {
            byte genreId = sqlDataReader.GetByte(sqlDataReader.GetOrdinal("GenreID"));
            string genreText = sqlDataReader.GetString(sqlDataReader.GetOrdinal("GenreText"));

            GenreDTO genre = new()
            {
                Id = genreId,
                Name = genreText
            };
            
            book.Genres.Add(genre);
        }
        
        sqlCommand.CommandText = "SELECT BookSetting.SettingID, Setting.SettingText " +
                                 "FROM BookSetting " +
                                 "JOIN Setting ON BookSetting.SettingID = Setting.SettingID " +
                                 "WHERE BookID = @BookID";
        sqlCommand.Parameters.Clear();
        sqlCommand.Parameters.AddWithValue("@BookID", id);
        
        sqlConnection.Close();
        sqlConnection.Open();
        
        sqlDataReader = sqlCommand.ExecuteReader();
        
        while (sqlDataReader.Read())
        {
            SettingDTO settingDto = new()
            {
                Id = sqlDataReader.GetByte(sqlDataReader.GetOrdinal("SettingID")),
                Description = sqlDataReader.GetString(sqlDataReader.GetOrdinal("SettingText"))
            };
            
            book.Settings.Add(settingDto);
        }
        
        sqlCommand.CommandText = "SELECT BookTheme.ThemeID, Theme.ThemeText " +
                                 "FROM BookTheme " +
                                 "JOIN Theme ON BookTheme.ThemeID = Theme.ThemeID " +
                                 "WHERE BookID = @BookID";
        sqlCommand.Parameters.Clear();
        sqlCommand.Parameters.AddWithValue("@BookID", id);
        
        sqlConnection.Close();
        sqlConnection.Open();
        
        sqlDataReader = sqlCommand.ExecuteReader();
        
        while (sqlDataReader.Read())
        {
            ThemeDTO themeDto = new()
            {
                Id = sqlDataReader.GetByte(sqlDataReader.GetOrdinal("ThemeID")),
                Description = sqlDataReader.GetString(sqlDataReader.GetOrdinal("ThemeText"))
            };
            
            book.Themes.Add(themeDto);
        }

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
        
        using SqlCommand sqlCommand = new("SELECT b.BookID, b.Isbn, b.Title, b.Synopsis, b.PublishDate, b.AmountPages, p.PublisherID AS PublisherId, p.Name AS PublisherName FROM Book AS b LEFT JOIN Publisher AS p ON b.PublisherID = p.PublisherID", sqlConnection);
        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
        while (sqlDataReader.Read())
        {
            long? bookId = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("BookId")) ? null : (long?)sqlDataReader["BookId"];
            string isbn = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Isbn"));
            string title = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Title"));
            string synopsis = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Synopsis"));
            DateTime publishDate = sqlDataReader.GetDateTime(sqlDataReader.GetOrdinal("PublishDate"));
            short amountPages = (short)sqlDataReader["AmountPages"];
        
            List<AuthorDTO> authors = new();

            long? publisherId = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("PublisherId")) ? null : (long?)sqlDataReader["PublisherId"];
            string publisherName = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("PublisherName")) ? null : sqlDataReader.GetString(sqlDataReader.GetOrdinal("PublisherName"));
            PublisherDTO publisher = new() { Id = publisherId, Name = publisherName };
        
            List<GenreDTO> genres = new();
        
            List<ThemeDTO> themes = new();
        
            List<SettingDTO> settings = new();
            
            books.Add(new(
                bookId,
                isbn,
                title,
                synopsis,
                DateOnly.FromDateTime(publishDate),
                (ushort) amountPages,
                authors,
                publisher,
                genres,
                themes,
                settings,
                0
            ));
        }

        foreach (BookDTO bookDto in books)
        {
            sqlCommand.CommandText = "SELECT Author.AuthorID, Author.Name, Author.Description " +
                                     "FROM BookAuthor " +
                                     "JOIN Author ON BookAuthor.AuthorID = Author.AuthorID " +
                                     "WHERE BookID = @BookID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@BookID", bookDto.Id);
        
            sqlConnection.Close();
            sqlConnection.Open();
        
            sqlDataReader = sqlCommand.ExecuteReader();
        
            while (sqlDataReader.Read())
            {
                AuthorDTO authorDto = new()
                {
                    Id = sqlDataReader.GetInt64(sqlDataReader.GetOrdinal("AuthorID")),
                    Name = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Name")),
                    Description = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Description")),
                    Genres = new()
                };
            
                bookDto.Authors.Add(authorDto);
            }

            foreach (AuthorDTO author in bookDto.Authors)
            {
                // get genres for author
                sqlCommand.CommandText = "SELECT Genre.GenreID, Genre.GenreText " +
                                         "FROM AuthorGenre " +
                                         "JOIN Genre ON AuthorGenre.GenreID = Genre.GenreID " +
                                         "WHERE AuthorID = @AuthorID";
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.AddWithValue("@AuthorID", author.Id);
                
                sqlConnection.Close();
                sqlConnection.Open();
                
                sqlDataReader = sqlCommand.ExecuteReader();
                
                while (sqlDataReader.Read())
                {
                    GenreDTO genreDto = new()
                    {
                        Id = sqlDataReader.GetByte(sqlDataReader.GetOrdinal("GenreID")),
                        Name = sqlDataReader.GetString(sqlDataReader.GetOrdinal("GenreText"))
                    };
                    
                    author.Genres.Add(genreDto);
                }
            }
        
            sqlCommand.CommandText = "SELECT BookGenre.GenreID, Genre.GenreText " +
                                     "FROM BookGenre " +
                                     "JOIN Genre ON BookGenre.GenreID = Genre.GenreID " +
                                     "WHERE BookID = @BookID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@BookID", bookDto.Id);
        
            sqlConnection.Close();
            sqlConnection.Open();
        
            sqlDataReader = sqlCommand.ExecuteReader();
        
            while (sqlDataReader.Read())
            {
                GenreDTO genreDto = new()
                {
                    Id = sqlDataReader.GetByte(sqlDataReader.GetOrdinal("GenreID")),
                    Name = sqlDataReader.GetString(sqlDataReader.GetOrdinal("GenreText"))
                };
            
                bookDto.Genres.Add(genreDto);
            }
        
            sqlCommand.CommandText = "SELECT BookSetting.SettingID, Setting.SettingText " +
                                     "FROM BookSetting " +
                                     "JOIN Setting ON BookSetting.SettingID = Setting.SettingID " +
                                     "WHERE BookID = @BookID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@BookID", bookDto.Id);
        
            sqlConnection.Close();
            sqlConnection.Open();
        
            sqlDataReader = sqlCommand.ExecuteReader();
        
            while (sqlDataReader.Read())
            {
                SettingDTO settingDto = new()
                {
                    Id = sqlDataReader.GetByte(sqlDataReader.GetOrdinal("SettingID")),
                    Description = sqlDataReader.GetString(sqlDataReader.GetOrdinal("SettingText"))
                };
            
                bookDto.Settings.Add(settingDto);
            }
            
            sqlCommand.CommandText = "SELECT BookTheme.ThemeID, Theme.ThemeText " +
                                     "FROM BookTheme " +
                                     "JOIN Theme ON BookTheme.ThemeID = Theme.ThemeID " +
                                     "WHERE BookID = @BookID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@BookID", bookDto.Id);
            
            sqlConnection.Close();
            sqlConnection.Open();
            
            sqlDataReader = sqlCommand.ExecuteReader();
            
            while (sqlDataReader.Read())
            {
                ThemeDTO themeDto = new()
                {
                    Id = sqlDataReader.GetByte(sqlDataReader.GetOrdinal("ThemeID")),
                    Description = sqlDataReader.GetString(sqlDataReader.GetOrdinal("ThemeText"))
                };
                
                bookDto.Themes.Add(themeDto);
            }

            sqlCommand.CommandText = "SELECT COUNT(*) FROM UserBookFavorite WHERE BookID = @BookID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@BookID", bookDto.Id);
            
            sqlConnection.Close();
            sqlConnection.Open();

            int countedRows = (int)sqlCommand.ExecuteScalar();
            bookDto.Favorites = (ulong)countedRows;
        }
        return books;
    }

    public bool Favorite(long bookId, long accountId)
    {
        using SqlConnection sqlConnection = new(ConnectionString);
        sqlConnection.Open();
        using SqlCommand sqlCommand = new("INSERT INTO UserBookFavorite (AccountID ,BookID) VALUES (@AccountID, @BookID)", sqlConnection);
        sqlCommand.Parameters.AddWithValue("@AccountID", accountId);
        sqlCommand.Parameters.AddWithValue("@BookID", bookId);
        
        return sqlCommand.ExecuteNonQuery() > 0;
    }

    public bool Unfavorite(long bookId, long accountId)
    {
        using SqlConnection sqlConnection = new(ConnectionString);
        sqlConnection.Open();
        using SqlCommand sqlCommand = new("DELETE FROM UserBookFavorite WHERE AccountID = @AccountID AND BookID = @BookID", sqlConnection);
        sqlCommand.Parameters.AddWithValue("@AccountID", accountId);
        sqlCommand.Parameters.AddWithValue("@BookID", bookId);
        
        return sqlCommand.ExecuteNonQuery() > 0;
    }

    public bool IsFavorite(long bookId, long accountId)
    {
        using SqlConnection sqlConnection = new(ConnectionString);
        sqlConnection.Open();
        using SqlCommand sqlCommand = new("SELECT COUNT(*) FROM UserBookFavorite WHERE AccountID = @AccountID AND BookID = @BookID", sqlConnection);
        sqlCommand.Parameters.AddWithValue("@AccountID", accountId);
        sqlCommand.Parameters.AddWithValue("@BookID", bookId);
        
        return (int)sqlCommand.ExecuteScalar() > 0;
    }

    public IEnumerable<BookDTO> GetAllFavoritesByAccountId(long accountId)
    {
        using SqlConnection sqlConnection = new(ConnectionString);
        sqlConnection.Open();
        using SqlCommand sqlCommand = new("SELECT TOP(10) * FROM Book WHERE BookID IN (SELECT BookID FROM UserBookFavorite WHERE AccountID = @AccountID)", sqlConnection);
        sqlCommand.Parameters.AddWithValue("@AccountID", accountId);
        
        using SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
        
        List<BookDTO> books = new();

        while (sqlDataReader.Read())
        {
            BookDTO bookDto = new(
                sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("BookId"))
                    ? null
                    : sqlDataReader.GetInt64(sqlDataReader.GetOrdinal("BookId")),
                sqlDataReader.GetString(sqlDataReader.GetOrdinal("Isbn")),
                sqlDataReader.GetString(sqlDataReader.GetOrdinal("Title")),
                string.Empty,
                DateOnly.FromDateTime(sqlDataReader.GetDateTime(sqlDataReader.GetOrdinal("PublishDate"))),
                Convert.ToUInt16(sqlDataReader["AmountPages"]),
                new(),
                new(),
                new(),
                new(),
                new(),
                0
            );

            books.Add(bookDto);
        }
        
        return books;
    }

    public bool Shelf(long bookId, long accountId)
    {
        using SqlConnection sqlConnection = new(ConnectionString);
        sqlConnection.Open();
        using SqlCommand sqlCommand = new("INSERT INTO UserBookShelve (AccountID ,BookID) VALUES (@AccountID, @BookID)", sqlConnection);
        sqlCommand.Parameters.AddWithValue("@AccountID", accountId);
        sqlCommand.Parameters.AddWithValue("@BookID", bookId);
        
        return sqlCommand.ExecuteNonQuery() > 0;
    }

    public bool Unshelve(long bookId, long accountId)
    {
        using SqlConnection sqlConnection = new(ConnectionString);
        sqlConnection.Open();
        using SqlCommand sqlCommand = new("DELETE FROM UserBookShelve WHERE AccountID = @AccountID AND BookID = @BookID", sqlConnection);
        sqlCommand.Parameters.AddWithValue("@AccountID", accountId);
        sqlCommand.Parameters.AddWithValue("@BookID", bookId);
        
        return sqlCommand.ExecuteNonQuery() > 0;
    }

    public bool Shelved(long bookId, long accountId)
    {
        using SqlConnection sqlConnection = new(ConnectionString);
        sqlConnection.Open();
        using SqlCommand sqlCommand = new("SELECT COUNT(*) FROM UserBookShelve WHERE AccountID = @AccountID AND BookID = @BookID", sqlConnection);
        sqlCommand.Parameters.AddWithValue("@AccountID", accountId);
        sqlCommand.Parameters.AddWithValue("@BookID", bookId);
        
        return (int)sqlCommand.ExecuteScalar() > 0;
    }

    public IEnumerable<BookDTO> GetAllShelvedByAccountId(long accountId)
    {
        using SqlConnection sqlConnection = new(ConnectionString);
        sqlConnection.Open();
        using SqlCommand sqlCommand = new("SELECT * FROM Book WHERE BookID IN (SELECT BookID FROM UserBookShelve WHERE AccountID = @AccountID)", sqlConnection);
        sqlCommand.Parameters.AddWithValue("@AccountID", accountId);
        
        using SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
        
        List<BookDTO> books = new();

        while (sqlDataReader.Read())
        {
            BookDTO bookDto = new(
                sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("BookId"))
                    ? null
                    : sqlDataReader.GetInt64(sqlDataReader.GetOrdinal("BookId")),
                sqlDataReader.GetString(sqlDataReader.GetOrdinal("Isbn")),
                sqlDataReader.GetString(sqlDataReader.GetOrdinal("Title")),
                string.Empty,
                DateOnly.FromDateTime(sqlDataReader.GetDateTime(sqlDataReader.GetOrdinal("PublishDate"))),
                Convert.ToUInt16(sqlDataReader["AmountPages"]),
                new(),
                new(),
                new(),
                new(),
                new(),
                0
            );

            books.Add(bookDto);
        }
        
        return books;
    }

    public bool Exist(string isbn)
    {
        using SqlConnection sqlConnection = new(ConnectionString);
        sqlConnection.Open();
        
        using SqlCommand sqlCommand = new("SELECT COUNT(*) FROM Book WHERE Isbn = @Isbn", sqlConnection);
        sqlCommand.Parameters.AddWithValue("@Isbn", isbn);
        
        return (int)sqlCommand.ExecuteScalar() > 0;
    }
}