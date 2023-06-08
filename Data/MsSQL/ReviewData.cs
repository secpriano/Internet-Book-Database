using System.Data.SqlClient;
using Interface.DTO;
using Interface.Interfaces;

namespace Data.MsSQL;

public class ReviewData : Database, IReviewData
{
    public bool Add(ReviewDTO entity)
    {
        using SqlConnection sqlConnection = new(ConnectionString);
        sqlConnection.Open();
        using SqlCommand sqlCommand = new("INSERT INTO UserBookReview (Title, Review, AccountID, BookID) VALUES (@Title, @Review, @AccountID, @BookID);", sqlConnection)
        {
            Parameters =
            {
                new("@Title", entity.Title),
                new("@Review", entity.Content),
                new("@AccountID", entity.Account.Id),
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
        using SqlCommand sqlCommand = new("SELECT " +
                                          "UserBookReview.UserBookReviewID, Title, Review, Comment, AccountReview.AccountID AS ReviewAccountId, AccountReview.Username AS ReviewUsername, AccountComment.AccountID AS CommentAccountId, AccountComment.Username AS CommentUsername " +
                                          "FROM UserBookReview " +
                                          "LEFT JOIN UserBookReviewComment ON UserBookReview.UserBookReviewID = UserBookReviewComment.UserBookReviewID " +
                                          "LEFT JOIN Account AS AccountReview ON UserBookReview.AccountID = AccountReview.AccountID " +
                                          "LEFT JOIN Account AS AccountComment ON UserBookReviewComment.AccountID = AccountComment.AccountID " +
                                          "WHERE BookId = @BookId", sqlConnection)
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
            long reviewAccountId = sqlDataReader.GetInt64(sqlDataReader.GetOrdinal("ReviewAccountId"));
            string reviewUsername = sqlDataReader.GetString(sqlDataReader.GetOrdinal("ReviewUsername"));
            string title = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Title"));
            string reviewContent = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Review"));
            long? commentAccountId = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("CommentAccountId")) ? null : sqlDataReader.GetInt64(sqlDataReader.GetOrdinal("CommentAccountId"));
            string commentUsername = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("CommentUsername")) ? null : sqlDataReader.GetString(sqlDataReader.GetOrdinal("CommentUsername"));
            string commentContent = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("Comment")) ? null : sqlDataReader.GetString(sqlDataReader.GetOrdinal("Comment"));

            ReviewDTO reviewDTO = reviews.FirstOrDefault(dto => dto.Id == reviewId && dto.BookId == bookId && dto.Account.Username == reviewUsername);

            if (reviewDTO == null)
            {
                reviewDTO = new(reviewId, title, reviewContent, new(reviewAccountId, reviewUsername), bookId, new());
                reviews.Add(reviewDTO);
            }

            if (commentUsername == null || commentContent == null) continue;
            CommentDTO commentDTO = new(commentContent, new(commentAccountId, commentUsername), reviewId);
            reviewDTO.Comments.Add(commentDTO);
        }
        return reviews;
    }
}