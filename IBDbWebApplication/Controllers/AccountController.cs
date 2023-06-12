using Business.Container;
using Business.Entity;
using Data.MsSQL;
using IBDbWebApplication.Models.AccountModels;
using IBDbWebApplication.Models.Entity;
using Interface.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IBDbWebApplication.Controllers;

public class AccountController : Controller
{
    private readonly AccountContainer _accountContainer = new(new AccountData());
    private readonly BookContainer _bookContainer = new(new BookData());
    
    [HttpGet]
    public IActionResult Index()
    {
        if (HttpContext.Session.GetInt32("Account") == 0)
            return RedirectToAction("Login", "Account");
        
        AccountModel accountModel = new(
            HttpContext.Session.GetInt32("Account"),
            HttpContext.Session.GetString("Username"),
            "",
            HttpContext.Session.GetInt32("IsAdmin") == 1
            );
        
        return View(GetAccountViewModel(accountModel));
    }
    
    private AccountViewModel GetAccountViewModel(AccountModel accountModel)
    {
        return new(accountModel, GetAllFavoritesByAccountId(accountModel.Id.Value));
    }
    
    private IEnumerable<BookModel> GetAllFavoritesByAccountId(long accountId)
    {
        return _bookContainer.GetAllFavoritesByAccountId(accountId).Select(book => new BookModel()
        {
            Id = book.Id,
            Isbn = book.Isbn,
            Title = book.Title,
            PublishDate = book.PublishDate,
            AmountPages = book.AmountPages
        });
    }
    
    [HttpGet]
    public IActionResult Login()
    {
        if (HttpContext.Session.GetInt32("Account") != null)
            return RedirectToAction("Index", "Home");
        
        return View();
    }
    
    [HttpPost]
    public IActionResult Login(AccountModel accountModel)
    {
        if (HttpContext.Session.GetInt32("Account") != null)
            return RedirectToAction("Login", "Account");
        
        if (!ModelState.IsValid)
        {
            return View(accountModel);
        }
        
        try
        {
            Account account = _accountContainer.Login(new(accountModel.Email), accountModel.Password);
            HttpContext.Session.SetInt32("Account", account.Id == null ? 0 : (int) account.Id);
            HttpContext.Session.SetString("Username", account.Username);
            HttpContext.Session.SetInt32("IsAdmin", account.IsAdmin ? 1 : 0);
            
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            ModelState.AddModelError("Login", e.Message);
            return View(accountModel);
        }
    }
    
    public IActionResult Logout()
    {
        if (HttpContext.Session.GetInt32("Account") == 0)
            return RedirectToAction("Login", "Account");
        
        HttpContext.Session.Clear();
        return RedirectToAction("Login", "Account");
    }
}