using ASPLearn.DataLayer.Entities.User;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ASPLearn.Core.DTOs
{
	public class UserForAdminViewModel
	{
		public required List<User> Users { get; set; }
		public int CurrentPage { get; set; }
		public int PageCount { get; set; }
	}

	public class CreateUserViewModel
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

		public IFormFile? UserAvatar { get; set; }
		//public List<int>? SelectedRoles { get; set; }
	}

	public class UpdateUserViewModel
	{
		public required int UserId { get; set; }

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
		//[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
		[MaxLength(200, ErrorMessage = "حداکثر طول {0}، {1} کاراکتر می باشد")]
		public string? Password { get; set; }

		public IFormFile? UserAvatar { get; set; }
		public required string AvatarName { get; set; }
		public List<int>? UserRoles { get; set; }
	}
}
