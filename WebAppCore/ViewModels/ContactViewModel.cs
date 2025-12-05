using System.ComponentModel.DataAnnotations;

namespace WebAppCore.ViewModels
{
    /// <summary>
    /// ViewModel для форми зворотного зв'язку з повною валідацією
    /// </summary>
    public class ContactViewModel
    {
        [Required(ErrorMessage = "Поле 'Повне ім'я' є обов'язковим для заповнення")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Поле 'Повне ім'я' повинно містити від 2 до 50 символів")]
        [Display(Name = "Повне ім'я")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Поле 'Email' є обов'язковим для заповнення")]
        [EmailAddress(ErrorMessage = "Введіть коректну адресу електронної пошти")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Поле 'Вік' є обов'язковим для заповнення")]
        [Range(18, 120, ErrorMessage = "Вік повинен бути від 18 до 120 років")]
        [Display(Name = "Вік")]
        public int? Age { get; set; }

        [Required(ErrorMessage = "Поле 'Пароль' є обов'язковим для заповнення")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Пароль повинен містити від 8 до 100 символів")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Поле 'Підтвердження пароля' є обов'язковим для заповнення")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароль та підтвердження пароля не співпадають")]
        [Display(Name = "Підтвердження пароля")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}

