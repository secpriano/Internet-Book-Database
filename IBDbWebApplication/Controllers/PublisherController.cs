using System.Text.RegularExpressions;
using Business.Container;
using Data.MsSQL;
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
        if (HttpContext.Session.GetInt32("IsAdmin") == null)
            return RedirectToAction("Login", "Account");
        
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
        if (HttpContext.Session.GetInt32("IsAdmin") == null)
            return RedirectToAction("Login", "Account");
        
        if (!ModelState.IsValid)
        {
            return View(nameof(Index), GetPublisherViewModel());
        }

        try
        {
            _publisherContainer.Add(new(
                publisherViewModel.Id, 
                publisherViewModel.Name, 
                DateOnly.FromDateTime(publisherViewModel.FoundingDate.Value), 
                publisherViewModel.Description)
            );
        }
        catch (AggregateException ex)
        {
            foreach (Exception innerException in ex.InnerExceptions)
            {
                ModelState.AddModelError($"{Regex.Replace(innerException.GetType().GetProperty("Type").GetValue(innerException) as string, @"\s", "")}", innerException.Message);
            }
            return View(nameof(Index), GetPublisherViewModel());
        }
        
        
        return RedirectToAction(nameof(Index));
    }
    
    private IEnumerable<PublisherModel> GetPublisherModels()
    {
        return _publisherContainer.GetAll().Select(publisher => new PublisherModel(publisher.Id, publisher.Name, publisher.FoundingDate, publisher.Description));
    }
}