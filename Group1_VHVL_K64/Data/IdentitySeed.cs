using Group1_VHVL_K64.Constants;
using Microsoft.AspNetCore.Identity;

namespace Group1_VHVL_K64.Data
{
    public static class IdentitySeed
    {
        public static async Task EnsureRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { AppRoles.Admin, AppRoles.User };

            foreach (string roleName in roles)
            {
                bool roleExists = await roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    }
}
