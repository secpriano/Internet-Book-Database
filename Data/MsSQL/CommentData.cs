using System.Data.SqlClient;
using Interface.DTO;
using Interface.Interfaces;

namespace Data;

public class CommentData : Database, ICommentData
{
    public bool Add(CommentDTO entity)
    {
        using SqlConnection sqlConnection = new(ConnectionString);
        sqlConnection.Open();
        
        using SqlCommand sqlCommand = new("INSERT INTO UserBookReviewComment (Comment, UserID, UserBookReviewID) VALUES (@Comment, @UserID, @UserBookReviewID);", sqlConnection)
        {
            Parameters =
            {
                new("@Comment", entity.Content),
                new("@UserID", entity.UserId),
                new("@UserBookReviewID", entity.ReviewId)
            }
        };
        
        return sqlCommand.ExecuteNonQuery() == 1;
    }

    public CommentDTO GetById(long id)
    {
        throw new NotImplementedException();
    }

    public CommentDTO Update(CommentDTO entity)
    {
        throw new NotImplementedException();
    }

    public bool Delete(long id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<CommentDTO> GetAll()
    {
        throw new NotImplementedException();
    }
}