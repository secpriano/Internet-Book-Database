using Business.Container;
using Business.Entity;
using Data;
using IBDbWebApplication.Models.Entity;
using Interface.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IBDbWebApplication.Controllers;

public class AccountController : Controller
{
    private readonly AccountContainer _accountController = new(new AccountData());
    
    [HttpGet]
    public IActionResult Index()
    {
        AccountModel accountModel = new(
            HttpContext.Session.GetInt32("Account"),
            HttpContext.Session.GetString("Username"),
            "",
            HttpContext.Session.GetInt32("IsAdmin") == 1
            );
        return View(accountModel);
    }
    
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult Login(AccountModel accountModel)
    {
        if (!ModelState.IsValid)
        {
            return View(accountModel);
        }
        
        try
        {
            Account account = _accountController.Login(new(accountModel.Email), accountModel.Password);
            HttpContext.Session.SetInt32("Account", (int) account.Id);
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
        HttpContext.Session.Clear();
        return RedirectToAction("Login", "Account");
    }
}