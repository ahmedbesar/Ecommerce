using Common.Authentication;
using Common.Authentication.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Common.Authentication.Policies;

public class SelfUserOrAdminPolicy : IAuthorizationPolicy
{
    public string PolicyName => AppPoliciesConsts.SelfUserOrAdmin;

    public void Apply(AuthorizationPolicyBuilder builder) =>
        builder.RequireAssertion(context =>
        {
            var userName = context.User.Identity?.Name;
            var routeUserName = (context.Resource as HttpContext)?
                                    .Request.RouteValues["userName"]?.ToString()
                                    ?? context.Resource as string; // Support explicit string resource for POST/body payloads

            return context.User.IsInRole(RolesConsts.Admin) || userName == routeUserName;
        });
}
