using Microsoft.AspNetCore.Mvc.RazorPages;
using ASPLearn.Core.Services.Interfaces;
using ASPLearn.DataLayer.Entities.User;

namespace ASPLearn.Web.Pages.Admin.Roles
{
	public class IndexModel(IAdminService adminService) : PageModel // Primary constructor
	{
		private readonly IAdminService _adminService = adminService;

		public List<Role> RolesList { get; set; }

		public void OnGet()
		{
			RolesList = _adminService.GetRoles();
		}
	}
}
