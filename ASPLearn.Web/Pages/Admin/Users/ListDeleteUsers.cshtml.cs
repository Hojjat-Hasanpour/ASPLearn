using ASPLearn.Core.DTOs;
using ASPLearn.Core.Services.Interfaces;
using ASPLearn.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASPLearn.Web.Pages.Admin.Users
{
	public class ListDeleteUsersModel(IAdminService adminService) : PageModel
	{
		private readonly IAdminService _adminService = adminService;

		public UserForAdminViewModel UserForAdminViewModel { get; set; }

		public void OnGet(int pageId = 1, string filterUserName = "", string filterEmail = "")
		{
			UserForAdminViewModel = _adminService.GetDeletedUsers(pageId, filterEmail, filterUserName);
		}
	}

}
