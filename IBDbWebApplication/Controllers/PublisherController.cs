using Business.Container;
using Business.Entity;
using Data;
using IBDbWebApplication.Models.AdminModels.PublisherModels;
using Microsoft.AspNetCore.Mvc;

namespace IBDbWebApplication.Controllers;

public class PublisherController : Controller
{
    private readonly PublisherContainer _publisherContainer = new(new PublisherData());
    
    [HttpGet]
    public IActionResult Index()
    {
        return RedirectToAction(nameof(Publisher), "Admin");
    }
    
    [HttpPost]
    public IActionResult AddPublisher(PublisherViewModel publisherViewModel)
    {
        _publisherContainer.Add(new(publisherViewModel.Id, publisherViewModel.Name, DateOnly.FromDateTime(publisherViewModel.FoundingDate), publisherViewModel.Description));
        return RedirectToAction(nameof(Publisher), "Admin");
    }
}