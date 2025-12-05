using Microsoft.AspNetCore.Mvc;
using WebAppCore.ViewModels;

namespace WebAppCore.Controllers
{
    /// <summary>
    /// Контролер для обробки форми зворотного зв'язку з демонстрацією валідації
    /// </summary>
    public class ContactController : Controller
    {
        private readonly ILogger<ContactController> _logger;

        public ContactController(ILogger<ContactController> logger)
        {
            _logger = logger;
        }

        // GET: Contact
        /// <summary>
        /// Відображає форму зворотного зв'язку
        /// </summary>
        public IActionResult Index()
        {
            return View(new ContactViewModel());
        }

        // POST: Contact
        /// <summary>
        /// Обробляє відправку форми зворотного зв'язку з серверною валідацією
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(ContactViewModel model)
        {
            // Перевірка серверної валідації
            if (!ModelState.IsValid)
            {
                // Якщо валідація не пройдена, повертаємо ту саму view з моделлю та помилками
                _logger.LogWarning("Форма зворотного зв'язку містить помилки валідації");
                return View(model);
            }

            // Якщо валідація пройдена успішно, виконуємо логіку обробки
            _logger.LogInformation("Форма зворотного зв'язку успішно оброблена для користувача: {Email}", model.Email);

            // Перенаправлення на сторінку успіху (Post/Redirect/Get pattern)
            return RedirectToAction(nameof(Success));
        }

        // GET: Contact/Success
        /// <summary>
        /// Відображає сторінку успішного відправлення форми
        /// </summary>
        public IActionResult Success()
        {
            return View();
        }
    }
}

