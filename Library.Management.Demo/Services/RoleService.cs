using Microsoft.AspNetCore.Identity;

namespace Library.Management.Demo.Services
{
    public class RoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> CreateRole(string roleName)
        {
            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                return await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
            return IdentityResult.Failed(new IdentityError() { Description = "Role already exists" });
        }
    }
}
