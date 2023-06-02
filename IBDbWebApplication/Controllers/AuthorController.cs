﻿using Business.Container;
using Business.Entity;
using Data;
using IBDbWebApplication.Models.AdminModels.AuthorModels;
using Microsoft.AspNetCore.Mvc;

namespace IBDbWebApplication.Controllers;

public class AuthorController : Controller
{
    private readonly AuthorContainer _authorContainer = new(new AuthorData());
    // GET
    public IActionResult Index()
    {
        return RedirectToAction(nameof(Author), "Admin");
    }
    
    public IActionResult AddAuthor(AuthorViewModel authorViewModel)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction(nameof(Index));
        }
        
        _authorContainer.Add(new(
            authorViewModel.Id,
            authorViewModel.Name,
            authorViewModel.Description,
            DateOnly.FromDateTime(authorViewModel.BirthDate),
            authorViewModel.DeathDate != null ? DateOnly.FromDateTime((DateTime)authorViewModel.DeathDate) : null,
            authorViewModel.GenreIds.Select(genreId => new Genre(genreId))
        ));

        return RedirectToAction(nameof(Author), "Admin");
    }
}