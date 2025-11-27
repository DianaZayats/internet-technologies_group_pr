using Microsoft.AspNetCore.Authorization;

namespace WebAppCore.Authorization
{
    /// <summary>
    /// Вимога авторизації: поточний користувач повинен бути власником (автором) ресурсу
    /// </summary>
    public class IsResourceOwnerRequirement : IAuthorizationRequirement
    {
    }
}

