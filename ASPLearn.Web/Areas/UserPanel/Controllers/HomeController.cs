using ASPLearn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ASPLearn.Core.DTOs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using ASPLearn.DataLayer.Migrations;

namespace ASPLearn.Web.Areas.UserPanel.Controllers
{
	[Area("UserPanel")]
	[Authorize]
	public class HomeController : Controller
	{
		private IUserService _userService;
		private ICourseService _courseService;
		public HomeController(IUserService userService, ICourseService courseService)
		{
			_userService = userService;
			_courseService = courseService;
		}

		public IActionResult Index()
		{
			return View("Index", _userService.GetUserInformation(User.Identity!.Name!));
		}

		[Route("UserPanel/EditProfile")]
		public IActionResult EditProfile()
		{
			return View(_userService.GetDataEditProfile(User.Identity!.Name!));
		}

		[HttpPost]
		[Route("UserPanel/EditProfile")]
		public IActionResult EditProfile(EditProfileViewModel profile)
		{
			if (!ModelState.IsValid)
				return View(profile);
			_userService.EditProfile(User.Identity!.Name!, profile);

			// Logout user
			HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

			return Redirect("/Login?EditProfile=True");
		}

		[Route("UserPanel/ChangePassword")]
		public IActionResult ChangePassword()
		{
			return View();
		}

		[HttpPost]
		[Route("UserPanel/ChangePassword")]
		public IActionResult ChangePassword(ChangePasswordViewModel change)
		{
			if (!ModelState.IsValid)
				return View(change);

			string currentUserName = User.Identity!.Name!;

			if (!_userService.CompareOldPassword(currentUserName, change.OldPassword))
			{
				ModelState.AddModelError("OldPassword", "کلمه عبور فعلی صحیح نمی باشد");
				return View(change);
				
			}

			_userService.ChangeUserPassword(currentUserName, change.Password);
			ViewBag.IsSuccess = true;
			return View();
		}

		[Route("UserPanel/MyCourses")]
		public IActionResult MyCourses()
		{
			var userId = _userService.GetUserByUserName(User.Identity!.Name!).UserId;
			var UserCourses = _courseService.GetUserCourses(userId);
			if (UserCourses == null)
				return NotFound();
			return View(UserCourses);
		}
	}
}
