using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SlayLib.Interfaces;
using SlayLib.Models;
using WebAppCore.Models;

namespace WebAppCore.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IMitRepository _mitRepository;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public HomeController(
        ILogger<HomeController> logger,
        IMitRepository mitRepository,
        SignInManager<ApplicationUser> signInManager)
    {
        _logger = logger;
        _mitRepository = mitRepository;
        _signInManager = signInManager;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var users = await _mitRepository.GetAllAsync<ApplicationUser>();
        return View(users);
    }

    public IActionResult Privacy()
    {
        return View();
    }


    [AllowAnonymous]
    public IActionResult Archive()
    {
        return View();
    }

    
    [AllowAnonymous]
    public async Task<IActionResult> ForceLogout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
