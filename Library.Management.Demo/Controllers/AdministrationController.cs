using Library.Management.Demo.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Library.Management.Demo.Controllers
{
    [ApiController]
    public class AdministrationController : ControllerBase
    {
        private readonly RoleService _roleService;
        public AdministrationController(RoleService roleService)
        {
            _roleService = roleService;
        }
        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            try
            {
                var result = await _roleService.CreateRole(roleName);
                if (result.Succeeded)
                {
                    return Created();
                }
                return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
