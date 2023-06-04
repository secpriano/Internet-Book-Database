using Business.Container;
using Business.Entity;
using Data;
using IBDbWebApplication.Models.AdminModels.PublisherModels;
using IBDbWebApplication.Models.Entity;
using Microsoft.AspNetCore.Mvc;

namespace IBDbWebApplication.Controllers;

public class PublisherController : Controller
{
    private readonly PublisherContainer _publisherContainer = new(new PublisherData());
    
    [HttpGet]
    public IActionResult Index()
    {
       
        return View(GetPublisherViewModel());
    }
    
    private PublisherViewModel GetPublisherViewModel()
    {
        return new(
            GetPublisherModels()
        );
    }
    
    [HttpPost]
    public IActionResult AddPublisher(PublisherViewModel publisherViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(nameof(Index), GetPublisherViewModel());
        }
        
        _publisherContainer.Add(new(publisherViewModel.Id, publisherViewModel.Name, DateOnly.FromDateTime(publisherViewModel.FoundingDate.Value), publisherViewModel.Description));
        return RedirectToAction(nameof(Index));
    }
    
    private IEnumerable<PublisherModel> GetPublisherModels()
    {
        return _publisherContainer.GetAll().Select(publisher => new PublisherModel(publisher.Id, publisher.Name, publisher.FoundingDate, publisher.Description));
    }
}