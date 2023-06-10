using Interface.DTO;
using Interface.Interfaces;

namespace Test.STUB;

public class CommentSTUB : ICommentData
{
    static AccountSTUB _accountStub = new();
    
    public List<CommentDTO> Comments = new()
    {
        new(1, "This is a comment", _accountStub.Accounts[0], 1),
        new(2, "This is another comment", _accountStub.Accounts[1], 2)
    };

    public bool Add(CommentDTO entity)
    {
        Comments.Add(entity);
        return Comments.Exists(entity.Equals);
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
    
    public List<CommentDTO> GetAllByReviewId(long reviewId)
    {
        return Comments.Where(comments => comments.ReviewId == reviewId) as List<CommentDTO>;
    }
}