﻿namespace Interface.DTO;

public record ReviewDTO(long Id, string Title, string Content, long UserId, long BookId, IEnumerable<CommentDTO> Comments);