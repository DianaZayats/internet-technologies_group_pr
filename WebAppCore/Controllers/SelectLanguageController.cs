using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Globalization; // Хоча не строго обов'язково для цього файлу, корисно для читабельності

namespace WebAppCore.Controllers
{
    public class SelectLanguageController : Controller
    {
        private readonly IOptions<RequestLocalizationOptions> LocOptions;

        // 1. Інжекція IOptions<RequestLocalizationOptions> для доступу до списку підтримуваних культур
        public SelectLanguageController(IOptions<RequestLocalizationOptions> locoptions)
        {
            LocOptions = locoptions;
        }

        // 2. Дія Index (GET): відображає доступні мови
        public IActionResult Index(string returnUrl)
        {
            // Зберігаємо URL, на який потрібно повернутися (щоб повернути користувача на попередню сторінку)
            ViewData["ReturnUrl"] = returnUrl;

            // Отримуємо список підтримуваних UI-культур, які ви налаштували в Program.cs
            var cultureItems = LocOptions.Value.SupportedUICultures?.ToList();

            return View(cultureItems); // Передаємо список у View
        }

        // 3. Дія SetLanguage (POST): встановлює обрану мову в Cookie
        [HttpPost]
        public IActionResult SetLanguage(string cultureName, string returnUrl)
        {
            // Створюємо та зберігаємо Cookie з обраною культурою (діє 1 рік)
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(cultureName)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            // Перенаправлення користувача на returnUrl
            if (string.IsNullOrEmpty(returnUrl))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return LocalRedirect(returnUrl);
            }
        }
    }
}