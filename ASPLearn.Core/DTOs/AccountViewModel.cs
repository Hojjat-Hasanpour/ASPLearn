using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPLearn.Core.DTOs
{
	public class RegisterViewModel
	{
		[Display(Name = "نام کاربری")]
		[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
		[MaxLength(200, ErrorMessage = "حداکثر طول {0}، {1} کاراکتر می باشد")]
		public required string UserName { get; set; }

		[Display(Name = "ایمیل")]
		[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
		[MaxLength(200, ErrorMessage = "حداکثر طول {0}، {1} کاراکتر می باشد")]
		[EmailAddress(ErrorMessage = "لطفا ایمیل معتبر وارد کنید")]
		public required string Email { get; set; }

		[Display(Name = "کلمه عبور")]
		[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
		[MaxLength(200, ErrorMessage = "حداکثر طول {0}، {1} کاراکتر می باشد")]
		public required string Password { get; set; }

		[Display(Name = "تکرار کلمه عبور")]
		[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
		[MaxLength(200, ErrorMessage = "حداکثر طول {0}، {1} کاراکتر می باشد")]
		[Compare("Password", ErrorMessage = "کلمه عبور و تکرار آن برابر نیست")]
		public required string ConfirmPassword { get; set; }
	}

	public class LoginViewModel
	{
		[Display(Name = "ایمیل")]
		[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
		[MaxLength(200, ErrorMessage = "حداکثر طول {0}، {1} کاراکتر می باشد")]
		[EmailAddress(ErrorMessage = "لطفا ایمیل معتبر وارد کنید")]
		public required string Email { get; set; }

		[Display(Name = "کلمه عبور")]
		[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
		[MaxLength(200, ErrorMessage = "حداکثر طول {0}، {1} کاراکتر می باشد")]
		public required string Password { get; set; }
		[Display(Name = "مرا به خاطر بسپار")]
		public bool RememberMe { get; set; }
	}

	public class ForgotPasswordViewModel
	{
		[Display(Name = "ایمیل")]
		[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
		[MaxLength(200, ErrorMessage = "حداکثر طول {0}، {1} کاراکتر می باشد")]
		[EmailAddress(ErrorMessage = "لطفا ایمیل معتبر وارد کنید")]
		public required string Email { get; set; }
	}

	public class ResetPasswordViewModel
	{
		[Display(Name = "کلمه عبور")]
		[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
		[MaxLength(200, ErrorMessage = "حداکثر طول {0}، {1} کاراکتر می باشد")]
		public string? Password { get; set; }
		[Display(Name = "تکرار کلمه عبور")]
		[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
		[MaxLength(200, ErrorMessage = "حداکثر طول {0}، {1} کاراکتر می باشد")]
		[Compare("Password", ErrorMessage = "کلمه عبور و تکرار آن برابر نیست")]
		public string? ConfirmPassword { get; set; }
		public required string ActiveCode { get; set; }
	}
}
