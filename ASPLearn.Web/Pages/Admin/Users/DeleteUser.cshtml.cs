using ASPLearn.Core.DTOs;
using ASPLearn.Core.Security;
using ASPLearn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASPLearn.Web.Pages.Admin.Users
{
	[PermissionChecker(5)] // Assuming 5 is the permission ID for deleting users
	public class DeleteUserModel(IAdminService adminService) : PageModel
    {
        private readonly IAdminService _admin_service = adminService;

		public InformationUserViewModel Info { get; set; }
		public void OnGet(int id)
        {
            ViewData["userId"] = id;
            Info = _admin_service.InformationUserViewModel(id);
		}

        public IActionResult OnPost(int id)
        {
            _admin_service.DeleteUser(id);
            return RedirectToPage("Index");
        }
    }
}
