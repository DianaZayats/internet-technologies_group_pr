using Microsoft.AspNetCore.Authorization;
using SlayLib.Models;

namespace WebAppCore.Authorization
{
    /// <summary>
    /// Обробник авторизації для перевірки, чи є поточний користувач власником ресурсу
    /// </summary>
    public class IsResourceOwnerHandler : AuthorizationHandler<IsResourceOwnerRequirement, Post>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            IsResourceOwnerRequirement requirement,
            Post resource)
        {
            // Отримуємо ідентифікатор поточного користувача
            var currentUserId = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            // Перевіряємо, чи ідентифікатор користувача збігається з ідентифікатором автора ресурсу
            if (!string.IsNullOrEmpty(currentUserId) && 
                currentUserId == resource.AuthorId)
            {
                // Якщо збігається, позначаємо вимогу як виконану
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}

