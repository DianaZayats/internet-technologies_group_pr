using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SlayLib.Data;
using SlayLib.Models;
using System.Security.Claims;

namespace WebAppCore.Controllers
{
    /// <summary>
    /// Контролер для управління постами з ресурсною авторизацією
    /// </summary>
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<PostController> _logger;

        public PostController(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<ApplicationUser> userManager,
            ILogger<PostController> logger)
        {
            _context = context;
            _authorizationService = authorizationService;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: Post/Edit/5
        /// <summary>
        /// Відображає сторінку редагування поста
        /// </summary>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Завантажуємо пост з бази даних
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            // Перевіряємо авторизацію: чи може поточний користувач редагувати цей ресурс
            var authorizationResult = await _authorizationService.AuthorizeAsync(
                User, post, "CanEditResource");

            if (!authorizationResult.Succeeded)
            {
                // Якщо авторизація не пройдена, повертаємо 403
                return Forbid();
            }

            return View(post);
        }

        // POST: Post/Edit/5
        /// <summary>
        /// Обробляє відправку форми редагування поста
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,AuthorId")] Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            // Завантажуємо пост з бази даних
            var existingPost = await _context.Posts.FindAsync(id);
            if (existingPost == null)
            {
                return NotFound();
            }

            // Перевіряємо авторизацію: чи може поточний користувач редагувати цей ресурс
            var authorizationResult = await _authorizationService.AuthorizeAsync(
                User, existingPost, "CanEditResource");

            if (!authorizationResult.Succeeded)
            {
                // Якщо авторизація не пройдена, повертаємо 403
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Оновлюємо тільки дозволені поля
                    existingPost.Title = post.Title;
                    existingPost.Content = post.Content;
                    existingPost.UpdatedAt = DateTime.UtcNow;

                    _context.Update(existingPost);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { id = existingPost.Id });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await PostExistsAsync(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(existingPost);
        }

        // GET: Post/Details/5
        /// <summary>
        /// Відображає деталі поста
        /// </summary>
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Post/Create
        /// <summary>
        /// Відображає форму створення нового поста
        /// </summary>
        public IActionResult Create()
        {
            return View();
        }

        // POST: Post/Create
        /// <summary>
        /// Обробляє створення нового поста
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Content")] Post post)
        {
            // Виключаємо AuthorId з валідації, оскільки воно встановлюється автоматично
            ModelState.Remove(nameof(Post.AuthorId));
            ModelState.Remove(nameof(Post.CreatedAt));
            ModelState.Remove(nameof(Post.UpdatedAt));
            ModelState.Remove(nameof(Post.Id));

            if (ModelState.IsValid)
            {
                try
                {
                    // Встановлюємо ідентифікатор автора на поточного користувача
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (string.IsNullOrEmpty(userId))
                    {
                        _logger.LogWarning("Спроба створити пост без автентифікованого користувача");
                        return Unauthorized();
                    }

                    post.AuthorId = userId;
                    post.CreatedAt = DateTime.UtcNow;

                    _context.Add(post);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Пост створено успішно з ID: {PostId}", post.Id);
                    return RedirectToAction(nameof(Details), new { id = post.Id });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Помилка при створенні поста");
                    ModelState.AddModelError("", "Сталася помилка при створенні поста. Спробуйте ще раз.");
                }
            }
            else
            {
                // Логуємо помилки валідації
                foreach (var error in ModelState)
                {
                    foreach (var errorMessage in error.Value.Errors)
                    {
                        _logger.LogWarning("Помилка валідації для {Field}: {Error}", error.Key, errorMessage.ErrorMessage);
                    }
                }
            }
            return View(post);
        }

        // GET: Post
        /// <summary>
        /// Відображає список всіх постів
        /// </summary>
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Posts.ToListAsync());
        }

        private async Task<bool> PostExistsAsync(int id)
        {
            return await _context.Posts.AnyAsync(e => e.Id == id);
        }
    }
}

