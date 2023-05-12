﻿using System.Collections.ObjectModel;
using Business.Entity;
using Interface.Interfaces;

namespace Business.Container;

public class GenreContainer
{
    private readonly IGenreData _genreData;
    
    public GenreContainer(IGenreData genreData)
    {
        _genreData = genreData;
    }
    
    public IEnumerable<Genre> GetAll()
    {
        return _genreData.GetAll().Select(genre => new Genre(genre.Id, genre.Name));
    }
    
    public bool Add(Genre genre)
    {
        ValidateGenre(genre);
        
        return _genreData.Add(genre.ToDto());
    }
    
    private void ValidateGenre(Genre genre)
    {
        ValidateName(genre.Name);
        
        if (Validate.Exceptions.InnerExceptions.Count > 0)
        {
            throw Validate.Exceptions;
        }
    }
    
    private void ValidateName(string name)
    {
        Validate.OutOfRange((ulong)name.Length, 2, 25, "Genre", Validate.Unit.Character);
        
        Validate.Regex(name, @"^[a-zA-Z0-9 ]+$", "Name must be alphanumeric");
    }
}