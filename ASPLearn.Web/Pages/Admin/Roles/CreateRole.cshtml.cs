using ASPLearn.Core.Security;
using ASPLearn.Core.Services.Interfaces;
using ASPLearn.DataLayer.Entities.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASPLearn.Web.Pages.Admin.Roles
{
    [PermissionChecker(7)] // Assuming 7 is the permission ID for creating roles
    public class CreateRoleModel(IAdminService adminService) : PageModel
    {
        private readonly IAdminService _adminService = adminService;
        [BindProperty]
        public Role Role { get; set; }
        public void OnGet()
        {
            ViewData["Permissions"] = _adminService.GetAllPermissions();
        }

        public IActionResult OnPost(List<int> selectedPermissions)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Permissions"] = _adminService.GetAllPermissions();
                return Page();
            }

            Role.IsDelete = false; // Initialize IsDelete to false by default
            int roleId = _adminService.AddRole(Role);
            if (selectedPermissions is { Count: > 0 })
            {
                // Add permissions to the role
                _adminService.AddPermissionsToRole(selectedPermissions, roleId);
            }
            return RedirectToPage("Index");
        }
    }
}
