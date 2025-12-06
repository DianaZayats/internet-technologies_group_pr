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

    public async Task<IActionResult> Index(int featuredPage = 1)
    {
        int pageSize = 4;
        var featuredQuery = _context.Recipes
            .Include(r => r.Author)
            .Include(r => r.Ratings)
            .Where(r => r.IsPublic)
            .OrderByDescending(r => r.CreatedAt)
            .AsQueryable();

        var featuredRecipes = await PaginateList<Recipe>.CreateAsync(featuredQuery, featuredPage, pageSize);

        ViewData["BreakfastRecipes"] = await _context.Recipes
            .Include(r => r.Author)
            .Include(r => r.Ratings)
            .Where(r => r.IsPublic && r.Category == "Breakfast")
            .OrderByDescending(r => r.CreatedAt)
            .Take(4)
            .ToListAsync();

        ViewData["LunchRecipes"] = await _context.Recipes
            .Include(r => r.Author)
            .Include(r => r.Ratings)
            .Where(r => r.IsPublic && r.Category == "Lunch")
            .OrderByDescending(r => r.CreatedAt)
            .Take(4)
            .ToListAsync();

        ViewData["DinnerRecipes"] = await _context.Recipes
            .Include(r => r.Author)
            .Include(r => r.Ratings)
            .Where(r => r.IsPublic && r.Category == "Dinner")
            .OrderByDescending(r => r.CreatedAt)
            .Take(4)
            .ToListAsync();

        ViewData["DessertRecipes"] = await _context.Recipes
            .Include(r => r.Author)
            .Include(r => r.Ratings)
            .Where(r => r.IsPublic && r.Category == "Dessert")
            .OrderByDescending(r => r.CreatedAt)
            .Take(4)
            .ToListAsync();

        foreach (var recipe in featuredRecipes)
        {
            if (recipe.Ratings != null && recipe.Ratings.Any())
            {
                recipe.AverageRating = recipe.Ratings.Average(r => r.Rating);
                recipe.RatingCount = recipe.Ratings.Count;
            }
        }
        int totalRecipes = await featuredQuery.CountAsync();
        ViewBag.FeaturedRecipes = featuredRecipes;
        ViewBag.FeaturedPage = featuredPage;
        ViewBag.FeaturedTotalPages = (int)Math.Ceiling(totalRecipes / (double)pageSize);

        return View(featuredRecipes);
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
