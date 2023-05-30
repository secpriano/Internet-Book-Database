using System.Data.SqlClient;
using Interface.DTO;
using Interface.Interfaces;

namespace Data;

public class ReviewData : Database, IReviewData
{
    public bool Add(ReviewDTO entity)
    {
        throw new NotImplementedException();
    }

    public ReviewDTO GetById(long id)
    {
        throw new NotImplementedException();
    }

    public ReviewDTO Update(ReviewDTO entity)
    {
        throw new NotImplementedException();
    }

    public bool Delete(long id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<ReviewDTO> GetAll()
    {
        using SqlConnection sqlConnection = new(ConnectionString);
    }

    public IEnumerable<ReviewDTO> GetAllByBookId(long bookId)
    {
        using SqlConnection sqlConnection = new(ConnectionString);
        sqlConnection.Open();
        using SqlCommand sqlCommand = new("SELECT UserBookReview.UserBookReviewID, BookID, UserBookReview.UserID AS ReviewUserID, Title, Review, UserBookReviewCommentID, UserBookReviewComment.UserID AS CommentUserID, Comment FROM UserBookReview JOIN UserBookReviewComment ON UserBookReview.UserBookReviewID = UserBookReviewComment.UserBookReviewCommentID WHERE BookId = @BookId;", sqlConnection)
        {
            Parameters =
            {
                new("@BookId", bookId)
            }
        };
        
        using SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
        
        List<ReviewDTO> reviews = new();
            
        while (sqlDataReader.Read())
        {
            reviews.Add(new(
                sqlDataReader.GetInt64(sqlDataReader.GetOrdinal("Id")),
                sqlDataReader.GetString(sqlDataReader.GetOrdinal("Title")),
                sqlDataReader.GetString(sqlDataReader.GetOrdinal("Review")),
                sqlDataReader.GetInt64(sqlDataReader.GetOrdinal("UserId")),
                sqlDataReader.GetInt64(sqlDataReader.GetOrdinal("BookId")),
            ));
        }
        
        return reviews;
    }
}