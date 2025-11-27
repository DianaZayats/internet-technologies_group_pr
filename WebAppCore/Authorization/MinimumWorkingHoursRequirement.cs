using Microsoft.AspNetCore.Authorization;

namespace WebAppCore.Authorization
{
    /// <summary>
    /// Вимога авторизації: користувач повинен мати мінімальну кількість робочих годин
    /// </summary>
    public class MinimumWorkingHoursRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// Мінімальна необхідна кількість робочих годин
        /// </summary>
        public int MinimumHours { get; }

        public MinimumWorkingHoursRequirement(int minimumHours)
        {
            MinimumHours = minimumHours;
        }
    }
}

