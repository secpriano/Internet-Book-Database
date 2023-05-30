namespace IBDbWebApplication.Models.Entity;

public record ReviewModel(long? Id, string Title, string Content, long BookId, long UserId, IEnumerable<CommentModel> CommentModels);