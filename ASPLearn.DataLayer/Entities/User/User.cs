using ASPLearn.DataLayer.Entities.Course;
using ASPLearn.DataLayer.Entities.Question;
using System.ComponentModel.DataAnnotations;

namespace ASPLearn.DataLayer.Entities.User
{
	public class User
	{
		[Key]
		public int UserId { get; set; }

		[Display(Name = "نام کاربری")]
		[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
		[MaxLength(200, ErrorMessage = "حداکثر طول {0}، {1} کاراکتر می باشد")]
		public required string UserName { get; set; }

		[Display(Name = "ایمیل")]
		[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
		[MaxLength(200, ErrorMessage = "حداکثر طول {0}، {1} کاراکتر می باشد")]
		[EmailAddress(ErrorMessage = "لطفا یک ایمیل معتبر وارد کنید")]
		public required string Email { get; set; }

		[Display(Name = "کلمه عبور")]
		[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
		[MaxLength(200, ErrorMessage = "حداکثر طول {0}، {1} کاراکتر می باشد")]
		public required string Password { get; set; }

		[Display(Name = "کد فعالسازی")]
		[MaxLength(50, ErrorMessage = "حداکثر طول {0}، {1} کاراکتر می باشد")]
		public string? ActiveCode { get; set; }

		[Display(Name = "فعال")]
		public bool IsActive { get; set; }

		[Display(Name = "آواتار")]
		[MaxLength(200, ErrorMessage = "حداکثر طول {0}، {1} کاراکتر می باشد")]
		public string? UserAvatar { get; set; }

		[Display(Name = "تاریخ ثبت نام")]
		public DateOnly? RegisterDate { get; set; }

		[Display(Name = "حذف شده")]
		public bool IsDelete { get; set; } = false;

		#region Relations
		public virtual List<UserRole>? UserRoles { get; set; }
		public virtual List<Wallet.Wallet>? Wallets { get; set; }
		public virtual List<Course.Course>? Courses { get; set; }
		public virtual List<Order.Order>? Orders { get; set; }
		public virtual List<Question.Question>? Questions { get; set; }
		public virtual List<Answer>? Answers { get; set; }
		public virtual List<UserCourse>? UserCourses { get; set; }
		public virtual List<UserDiscountCode>? UserDiscountCodes { get; set; }
		public virtual List<CourseComment>? CourseComments { get; set; }
		public virtual List<CourseVote>? CourseVotes { get; set; }

		#endregion

	}
}
