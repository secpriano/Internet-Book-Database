﻿using Interface.DTO;

namespace Business.Entity;

public record Genre(long? Id, string Name)
{
    public Genre(GenreDTO genreDto) : this(genreDto.Id, genreDto.Name) { }
    public GenreDTO ToDto() => new(Id, Name);
}
