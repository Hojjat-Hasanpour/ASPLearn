using ASPLearn.Core.DTOs;
using ASPLearn.Core.Security;
using ASPLearn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASPLearn.Web.Pages.Admin.Users
{
	[PermissionChecker(3)] // Assuming 3 is the permission ID for creating users
	public class CreateUserModel : PageModel
    {
        
        private IUserService _userService;
		private IAdminService _adminService;
		public CreateUserModel(IUserService userService, IAdminService adminService)
		{
			_userService = userService;
			_adminService = adminService;
		}

		[BindProperty]
		public CreateUserViewModel CreateUserViewModel { get; set; }
		public void OnGet()
        {
            ViewData["Roles"] = _adminService.GetRoles();
        }

		public IActionResult OnPost(List<int> selectedRoles)
		{
			if (!ModelState.IsValid)
			{
				ViewData["Roles"] = _adminService.GetRoles();
				return Page();
			}
				
			int userId = _adminService.AddUserFormAdmin(CreateUserViewModel);
			//Add Roles
			_adminService.AddRolesToUser(selectedRoles, userId);

			return Redirect("/Admin/Users");
		}
    }
}
