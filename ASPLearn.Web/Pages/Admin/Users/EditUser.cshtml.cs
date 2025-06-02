using ASPLearn.Core.DTOs;
using ASPLearn.Core.Security;
using ASPLearn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASPLearn.Web.Pages.Admin.Users
{
	[PermissionChecker(4)] // Assuming 4 is the permission ID for editing users
	public class EditUserModel(IAdminService adminService) : PageModel
	{
		private readonly IAdminService _adminService = adminService;

		[BindProperty]
		public UpdateUserViewModel UpdateViewModel { get; set; }
		public void OnGet(int id)
		{
			UpdateViewModel = _adminService.ShowUserInAdmin(id);
			UpdateViewModel.UserRoles = _adminService.GetUserRoles(id); //GetUserRoles
			ViewData["Roles"] = _adminService.GetRoles();
		}

		public IActionResult OnPost(List<int> SelectedRoles)
		{
			if (!ModelState.IsValid)
			{
				ViewData["Roles"] = _adminService.GetRoles();
				return Page();
			}
			
			_adminService.UpdateUserInAdmin(UpdateViewModel);
			_adminService.UpdateUserRoles(UpdateViewModel.UserId, SelectedRoles);
			return RedirectToPage("Index");

		}
	}
}
