using AspNet_Core6.Fundamentals.Models;
using System.Security.Claims;

namespace AspNet_Core6.Fundamentals.Extensions
{
    public static class RoleClaimExtension
    {
        public static IEnumerable<Claim> GetClaims(this User user)
        {
            var roles = new List<Claim>
            {
                new Claim(type: ClaimTypes.Name, value: user.Email),
            };

            roles.AddRange(collection: user.Roles.Select(role => new Claim(type: ClaimTypes.Role, value: role.Slug)));
            return roles;
        }
    }
}
