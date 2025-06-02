using ASPLearn.Core.DTOs;
using ASPLearn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASPLearn.Web.Pages.Admin.Users
{
	public class RestoreDeletedUserModel(IAdminService admin_service) : PageModel
	{
		private readonly IAdminService _admin_service = admin_service;
		//public UpdateUserViewModel UpdateUserViewModel { get; set; }
		public IActionResult OnGet(int id)
		{
			_admin_service.RestoreDeletedUser(id);
			return RedirectToPage("Index");
		}
	}
}
