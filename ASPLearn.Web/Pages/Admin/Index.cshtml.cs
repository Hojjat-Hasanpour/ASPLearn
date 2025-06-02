using ASPLearn.Core.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASPLearn.Web.Pages.Admin
{
	[PermissionChecker(1)] // Assuming 1 is the permission ID for accessing the admin index
	public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
