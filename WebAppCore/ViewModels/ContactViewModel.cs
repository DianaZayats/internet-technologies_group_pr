using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using WebAppCore.Resources;

namespace WebAppCore.ViewModels
{
    /// <summary>
    /// ViewModel для форми зворотного зв'язку з повною валідацією
    /// Демонструє використання локалізованих повідомлень про помилки валідації через ресурсні файли
    /// </summary>
    public class ContactViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "FullName_Required")]
        [StringLength(50, MinimumLength = 2, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "FullName_StringLength")]
        [Display(Name = "Повне ім'я")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "Email_Required")]
        [EmailAddress(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "Email_EmailAddress")]
        [Remote(action: "VerifyEmail", controller: "Validation", ErrorMessage = "Цей email вже зареєстрований")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "Age_Required")]
        [Range(18, 120, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "Age_Range")]
        [Display(Name = "Вік")]
        public int? Age { get; set; }

        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "Password_Required")]
        [StringLength(100, MinimumLength = 8, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "Password_StringLength")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "ConfirmPassword_Required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "ConfirmPassword_Compare")]
        [Display(Name = "Підтвердження пароля")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}

