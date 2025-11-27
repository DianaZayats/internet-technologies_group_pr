using Microsoft.AspNetCore.Authorization;
using SlayLib.Models;

namespace WebAppCore.Authorization
{
    public class ForumAccessHandler
     : AuthorizationHandler<ForumAccessRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            ForumAccessRequirement requirement)
        {
            var user = context.User;

            if (user.HasClaim("IsMentor", "true") ||
                user.HasClaim("IsVerifiedUser", "true") ||
                user.HasClaim("HasForumAccess", "true"))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

}

