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
                new("@AmountPages", entity.AmountPages),
                new("@Synopsis", entity.Synopsis),
                new("@PublishDate", entity.PublishDate),
                new("@PublisherID", entity.Publisher.Id)
            }
        };
        
        long bookId = (long)sqlCommand.ExecuteScalar();

        AddItemsToTable(entity.Genres, "BookGenre", bookId, "GenreID", sqlCommand);
        AddItemsToTable(entity.Themes, "BookTheme", bookId, "ThemeID", sqlCommand);
        AddItemsToTable(entity.Settings, "BookSetting", bookId, "SettingID", sqlCommand);
        AddItemsToTable(entity.Authors, "BookAuthor", bookId, "AuthorID", sqlCommand);

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
        throw new NotImplementedException();
    }
}