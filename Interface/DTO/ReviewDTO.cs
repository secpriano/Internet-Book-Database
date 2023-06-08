namespace Interface.DTO;

public record ReviewDTO(long? Id, string Title, string Content, AccountDTO Account, long BookId, List<CommentDTO> Comments);