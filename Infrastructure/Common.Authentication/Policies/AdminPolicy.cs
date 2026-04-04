using Common.Authentication;
using Common.Authentication.Consts;
using Microsoft.AspNetCore.Authorization;

namespace Common.Authentication.Policies;

public class AdminPolicy : IAuthorizationPolicy
{
    public string PolicyName => AppPoliciesConsts.Admin;

    public void Apply(AuthorizationPolicyBuilder builder) =>
        builder.RequireRole(RolesConsts.Admin);
}
