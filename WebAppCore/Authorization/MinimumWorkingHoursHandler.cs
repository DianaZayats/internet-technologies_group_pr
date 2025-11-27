using Microsoft.AspNetCore.Authorization;

namespace WebAppCore.Authorization
{
    /// <summary>
    /// Обробник авторизації для перевірки мінімальної кількості робочих годин користувача
    /// </summary>
    public class MinimumWorkingHoursHandler : AuthorizationHandler<MinimumWorkingHoursRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            MinimumWorkingHoursRequirement requirement)
        {
            // Отримуємо твердження WorkingHours з поточного користувача
            var workingHoursClaim = context.User.FindFirst("WorkingHours");

            // Якщо твердження відсутнє, вимога не виконується
            if (workingHoursClaim == null)
            {
                return Task.CompletedTask;
            }

            // Спробуємо розпарсити значення твердження як ціле число
            if (int.TryParse(workingHoursClaim.Value, out int workingHours))
            {
                // Якщо значення більше або дорівнює мінімальній кількості годин, позначаємо вимогу як виконану
                if (workingHours >= requirement.MinimumHours)
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}

