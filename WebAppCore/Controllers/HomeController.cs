using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SlayLib.Data;
using SlayLib.Interfaces;
using SlayLib.Models;
using WebAppCore.Models;

namespace WebAppCore.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IMitRepository _mitRepository;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ApplicationDbContext _context;

    public HomeController(
        ILogger<HomeController> logger,
        IMitRepository mitRepository,
        SignInManager<ApplicationUser> signInManager,
        ApplicationDbContext context)
    {
        _logger = logger;
        _mitRepository = mitRepository;
        _signInManager = signInManager;
        _context = context;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        // Get featured recipes (most recent public recipes)
        var featuredRecipes = await _context.Recipes
            .Include(r => r.Author)
            .Include(r => r.Ratings)
            .Where(r => r.IsPublic)
            .OrderByDescending(r => r.CreatedAt)
            .Take(6)
            .ToListAsync();

        // Get recipes by category
        var breakfastRecipes = await _context.Recipes
            .Include(r => r.Author)
            .Include(r => r.Ratings)
            .Where(r => r.IsPublic && r.Category == "Breakfast")
            .OrderByDescending(r => r.CreatedAt)
            .Take(4)
            .ToListAsync();

        var lunchRecipes = await _context.Recipes
            .Include(r => r.Author)
            .Include(r => r.Ratings)
            .Where(r => r.IsPublic && r.Category == "Lunch")
            .OrderByDescending(r => r.CreatedAt)
            .Take(4)
            .ToListAsync();

        var dinnerRecipes = await _context.Recipes
            .Include(r => r.Author)
            .Include(r => r.Ratings)
            .Where(r => r.IsPublic && r.Category == "Dinner")
            .OrderByDescending(r => r.CreatedAt)
            .Take(4)
            .ToListAsync();

        var dessertRecipes = await _context.Recipes
            .Include(r => r.Author)
            .Include(r => r.Ratings)
            .Where(r => r.IsPublic && r.Category == "Dessert")
            .OrderByDescending(r => r.CreatedAt)
            .Take(4)
            .ToListAsync();

        // Calculate average ratings
        foreach (var recipe in featuredRecipes.Concat(breakfastRecipes).Concat(lunchRecipes).Concat(dinnerRecipes).Concat(dessertRecipes))
        {
            if (recipe.Ratings != null && recipe.Ratings.Any())
            {
                ViewData[$"Rating_{recipe.Id}"] = recipe.Ratings.Average(r => r.Rating);
                ViewData[$"RatingCount_{recipe.Id}"] = recipe.Ratings.Count;
            }
        }

        ViewData["FeaturedRecipes"] = featuredRecipes;
        ViewData["BreakfastRecipes"] = breakfastRecipes;
        ViewData["LunchRecipes"] = lunchRecipes;
        ViewData["DinnerRecipes"] = dinnerRecipes;
        ViewData["DessertRecipes"] = dessertRecipes;

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }


    [Authorize(Policy = "VerifiedClientOnly")]
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
