﻿using Business.Container;
using Data;
using Microsoft.AspNetCore.Mvc;

namespace IBDbWebApplication.Controllers;

public class AdminController : Controller
{
    BookContainer bookContainer = new(new BookData());
    // GET
    public IActionResult Index()
    {
        return View();
    }
    
    public IActionResult Book()
    {
        return View("Book/Index");
    }
}