using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SlayLib.Interfaces;
using SlayLib.Models;
using WebAppCore.Models;

namespace WebAppCore.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IMitRepository _mitRepository;

    public HomeController(ILogger<HomeController> logger, IMitRepository mitRepository)
    {
        _logger = logger;
        _mitRepository = mitRepository;
    }

    public async Task<IActionResult> Index()
    {
        var users = await _mitRepository.GetAllAsync<ApplicationUser>();
        return View(users);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
