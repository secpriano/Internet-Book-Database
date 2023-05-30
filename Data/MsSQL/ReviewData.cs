using System.Data.SqlClient;
using Interface.DTO;
using Interface.Interfaces;

namespace Data;

public class ReviewData : Database, IReviewData
{
    public bool Add(ReviewDTO entity)
    {
        using SqlConnection sqlConnection = new(ConnectionString);
        sqlConnection.Open();
        using SqlCommand sqlCommand = new("INSERT INTO UserBookReview (Title, Review, UserID, BookID) VALUES (@Title, @Review, @UserID, @BookID);", sqlConnection)
        {
            Parameters =
            {
                new("@Title", entity.Title),
                new("@Review", entity.Content),
                new("@UserID", entity.UserId),
                new("@BookID", entity.BookId)
            }
        };
        
        return sqlCommand.ExecuteNonQuery() == 1;
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
        throw new NotImplementedException();
    }

    public IEnumerable<ReviewDTO> GetAllByBookId(long bookId)
    {
        using SqlConnection sqlConnection = new(ConnectionString);
        sqlConnection.Open();
        using SqlCommand sqlCommand = new("SELECT UserBookReview.UserBookReviewID, BookID, UserBookReview.UserID AS ReviewUserID, Title, Review, UserBookReviewCommentID, UserBookReviewComment.UserID AS CommentUserID, Comment  FROM UserBookReview LEFT JOIN UserBookReviewComment ON UserBookReview.UserBookReviewID = UserBookReviewComment.UserBookReviewID WHERE BookId = @BookId", sqlConnection)
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
            long reviewId = sqlDataReader.GetInt64(sqlDataReader.GetOrdinal("UserBookReviewID"));
            long reviewUserId = sqlDataReader.GetInt64(sqlDataReader.GetOrdinal("ReviewUserID"));
            string title = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Title"));
            string reviewContent = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Review"));
            long? commentId = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("UserBookReviewCommentID")) ? null : sqlDataReader.GetInt64(sqlDataReader.GetOrdinal("UserBookReviewCommentID"));
            long? commentUserId = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("CommentUserID")) ? null : sqlDataReader.GetInt64(sqlDataReader.GetOrdinal("CommentUserID"));
            string commentContent = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("Comment")) ? null : sqlDataReader.GetString(sqlDataReader.GetOrdinal("Comment"));

            ReviewDTO reviewDTO = reviews.FirstOrDefault(dto => dto.Id == reviewId && dto.BookId == bookId && dto.UserId == reviewUserId);

            if (reviewDTO == null)
            {
                reviewDTO = new(reviewId, title, reviewContent, reviewUserId, bookId, new());
                reviews.Add(reviewDTO);
            }

            if (commentId == null || commentUserId == null || commentContent == null) continue;
            CommentDTO commentDTO = new(commentId.Value, commentContent, commentUserId.Value);
            reviewDTO.Comments.Add(commentDTO);
        }
        return reviews;
    }
}