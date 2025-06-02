using ASPLearn.Core.Conventors;
using ASPLearn.Core.DTOs;
using ASPLearn.Core.Generator;
using ASPLearn.Core.Security;
using ASPLearn.Core.Services.Interfaces;
using ASPLearn.DataLayer.Entities.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TopLearn.Core.Convertors;
using TopLearn.Core.Senders;

namespace ASPLearn.Web.Controllers
{
	public class AccountController : Controller
	{
		private readonly IUserService _userService;
		private readonly IViewRenderService _viewRenderService;

		// Can Use Primary Constructor by the way
		public AccountController(IUserService userService, IViewRenderService viewRenderService)
		{
			_userService = userService;
			_viewRenderService = viewRenderService;
		}

		#region Register
		[Route("Register")]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		[Route("Register")]
		public IActionResult Register(RegisterViewModel register)
		{
			if (!ModelState.IsValid)
			{
				return View(register);
			}
			if (_userService.IsExistUserName(register.UserName))
			{
				ModelState.AddModelError("UserName", "این نام کاربری قبلا ثبت شده است");
				return View(register);
			}

			if (_userService.IsExistEmail(register.Email))
			{
				ModelState.AddModelError("Email", "این ایمیل قبلا ثبت شده است");
				return View(register);
			}

			User user = new User()
			{
				UserName = register.UserName,
				Email = FixedText.FixedEmail(register.Email), //register.Email,
				Password = PasswordHelper.EncodePasswordMd5(register.Password),
				ActiveCode = NameGenerator.GenerateUniqCode(),
				IsActive = false,
				RegisterDate = DateOnly.FromDateTime(DateTime.Now), //DateTime.Now
				UserAvatar = "UserAvatarPic.png",
			};
			_userService.AddUser(user);

			#region SendEmail

			string body = _viewRenderService.RenderToStringAsync("_ActiveEmail", user);
			SendEmail.Send(user.Email, "فعالسازی حساب کاربری", body);
			#endregion

			return View("SuccessRegister", user);
		}
		#endregion

		#region Login
		[Route("Login")]
		public IActionResult Login(bool EditProfile = false)
		{
			ViewBag.EditProfile = EditProfile;
			return View();
		}

		[HttpPost]
		[Route("Login")]
		public IActionResult Login(LoginViewModel login,string ReturnUrl="/")
		{
			if (!ModelState.IsValid)
				return View(login);

			var user = _userService.LoginUser(login);
			if (user != null)
			{
				if (user.IsActive)
				{
					var claims = new List<Claim>()
					{
						new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
						new Claim(ClaimTypes.Name, user.UserName),
						//new Claim(ClaimTypes.Email, user.Email),
					};
					var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
					var principal = new ClaimsPrincipal(identity);
					var properties = new AuthenticationProperties()
					{
						IsPersistent = login.RememberMe
					};
					HttpContext.SignInAsync(principal, properties);
					ViewBag.IsSuccess = true;
					if (ReturnUrl != "/")
						Redirect(ReturnUrl);
					return View(login);
				}
				else
				{
					ModelState.AddModelError("Email", "حساب کاربری شما فعال نمی باشد.");
				}
			}

			ModelState.AddModelError("Email", "کاربری با مشخصات وارد شده یافت نشد");
			return View(login);
		}

		#endregion

		#region Logout
		[Route("/Logout")]
		public IActionResult Logout()
		{
			HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return Redirect("/");
		}
		#endregion

		#region ActiveAccount

		public IActionResult ActiveAccount(string id)
		{
			ViewBag.IsActive = _userService.ActiveAccount(id);
			return View("ActiveAccount");
		}
		#endregion

		#region ForgotPassword
		[Route("ForgotPassword")]
		public IActionResult ForgotPassword()
		{
			return View();
		}

		[HttpPost]
		[Route("ForgotPassword")]
		public IActionResult ForgotPassword(ForgotPasswordViewModel forgot)
		{
			if (!ModelState.IsValid)
				return View(forgot);

			string fixedEmail = FixedText.FixedEmail(forgot.Email);
			var user = _userService.GetUserByEmail(fixedEmail);
			if (user == null)
			{
				ModelState.AddModelError("Email", "کاربری با این ایمیل یافت نشد");
				return View(forgot);
			}

			string bodyEmail = _viewRenderService.RenderToStringAsync("_ForgotPassword", user);
			SendEmail.Send(user.Email, "بازیابی کلمه عبور", bodyEmail);
			ViewBag.IsSuccess = true;

			return View();
		}
		#endregion

		#region ResetPassword

		public IActionResult ResetPassword(string id)
		{

			return View(new ResetPasswordViewModel()
			{
				ActiveCode = id,

			});
		}

		[HttpPost]
		public IActionResult ResetPassword(ResetPasswordViewModel reset)
		{
			if (!ModelState.IsValid)
			{
				return View(reset);
			}
			User user = _userService.GetUserByActiveCode(reset.ActiveCode);
			if (user == null)
			{
				ModelState.AddModelError("Password", "کاربری یافت نشد");
				return View(reset);
			}
			string hashPassword = PasswordHelper.EncodePasswordMd5(reset.Password!);
			user.Password = hashPassword;
			user.ActiveCode = NameGenerator.GenerateUniqCode();
			_userService.UpdateUser(user);
			return Redirect("/Login");
		}

		#endregion
	}
}
