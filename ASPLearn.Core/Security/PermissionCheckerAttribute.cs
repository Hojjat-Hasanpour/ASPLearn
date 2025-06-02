using ASPLearn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ASPLearn.Core.Security
{
	public class PermissionCheckerAttribute : AuthorizeAttribute, IAuthorizationFilter
	{
		private IAdminService _adminService; //Can not be readonly
		private int _permissionId = 0;
		public PermissionCheckerAttribute(int permissionId)
		{
			_permissionId = permissionId;
		}
		public void OnAuthorization(AuthorizationFilterContext context)
		{
			_adminService = (IAdminService)context.HttpContext.RequestServices.GetService(typeof(IAdminService))!;
			if (context.HttpContext.User.Identity.IsAuthenticated)
			{
				string userName = context.HttpContext.User.Identity.Name!;
				if (!_adminService.CheckPermission(_permissionId, userName))
				{
					context.Result = new RedirectResult($"/Login?{context.HttpContext.Request.Path}"); // /Home/AccessDenied
				}

				//// Check if the user has the required permission
				//if (!context.HttpContext.User.HasClaim(c => c.Type == "Permission" && c.Value == _permissionId.ToString()))
				//{
				//	context.Result = new ForbidResult();
				//}
			}
			else
			{
				context.Result = new RedirectResult("/Login");
			}
		}
	}
}
