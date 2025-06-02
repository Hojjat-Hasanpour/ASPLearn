using ASPLearn.Core.Security;
using ASPLearn.Core.Services.Interfaces;
using ASPLearn.DataLayer.Entities.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASPLearn.Web.Pages.Admin.Roles
{
	[PermissionChecker(8)] //EditRoleId
	public class EditRoleModel : PageModel
    {
		private readonly IAdminService _adminService;
		public EditRoleModel(IAdminService adminService)
		{
			_adminService = adminService;
		}
		[BindProperty]
		public Role Role { get; set; }

		public void OnGet(int id)
        {
			Role = _adminService.GetRoleById(id);
			ViewData["Permissions"] = _adminService.GetAllPermissions();
			ViewData["SelectedPermissions"] = _adminService.GetPermissionsRole(id);
		}

		public IActionResult OnPost(List<int> selectedPermissions)
		{
			if (!ModelState.IsValid)
				return Page();
			_adminService.UpdateRole(Role);
			_adminService.UpdatePermissionRoles(selectedPermissions, Role.RoleId);

			return RedirectToPage("Index");
		}
	}
}
