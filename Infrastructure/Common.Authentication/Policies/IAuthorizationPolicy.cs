using Microsoft.AspNetCore.Authorization;

namespace Common.Authentication.Policies;

public interface IAuthorizationPolicy
{
    string PolicyName { get; }
    void Apply(AuthorizationPolicyBuilder builder);
}
