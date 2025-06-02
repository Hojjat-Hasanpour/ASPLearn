using ASPLearn.Core.Security;
using ASPLearn.Core.Services.Interfaces;
using ASPLearn.DataLayer.Entities.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASPLearn.Web.Pages.Admin.Roles
{
	[PermissionChecker(9)] // Assuming 9 is the permission ID for deleting roles
	public class DeleteRoleModel(IAdminService adminService) : PageModel
	{
		private readonly IAdminService _adminService = adminService;

		[BindProperty]
		public Role Role { get; set; }

		public void OnGet(int id)
		{
			Role = _adminService.GetRoleById(id);
		}

		public IActionResult OnPost()
		{
			if (!ModelState.IsValid)
				return Page();
			// Check if the role is used by any user
			//var isRoleUsed = _adminService.GetUsers().Any(u => u.Roles.Any(r => r.RoleId == Role.RoleId));
			//if (isRoleUsed)
			//{
			//	ModelState.AddModelError("", "This role is currently in use and cannot be deleted.");
			//	return Page();
			//}
			// Delete the role
			_adminService.DeleteRole(Role);

			return RedirectToPage("Index");
		}
	}
}
