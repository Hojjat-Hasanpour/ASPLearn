using Microsoft.AspNetCore.Mvc.RazorPages;
using ASPLearn.Core.Services.Interfaces;
using ASPLearn.Core.DTOs;
using ASPLearn.Core.Security;

namespace ASPLearn.Web.Pages.Admin.Users
{
	[PermissionChecker(2)] // Assuming 2 is the permission ID for viewing users
	public class IndexModel(IAdminService adminService) : PageModel // Primary constructor
	{
		private readonly IAdminService _adminService = adminService;

		public UserForAdminViewModel UserForAdminViewModel { get; set; }

		public void OnGet(int pageId = 1, string filterUserName = "", string filterEmail = "")
		{
			UserForAdminViewModel = _adminService.GetUsers(pageId, filterEmail, filterUserName);
		}
	}
}
