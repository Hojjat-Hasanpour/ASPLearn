using ASPLearn.DataLayer.Entities.Order;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPLearn.DataLayer.Entities.Course
{
	public class Course
	{
		[Key]
		public int CourseId { get; set; }

		[Required]
		public required int GroupId { get; set; }

		public int? SubGroup { get; set; }

		[Required]
		public required int TeacherId { get; set; }

		public required int StatusId { get; set; }

		public required int LevelId { get; set; }

		[Display(Name = "عنوان دوره")]
		[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
		[MaxLength(250, ErrorMessage = "حداکثر {0}،{1} کاراکتر مجاز است")] // nvarchar MAX
		public required string CourseTitle { get; set; }

		[Display(Name = "توضیحات دوره")]
		[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
		[MaxLength(300, ErrorMessage = "حداکثر {0}،{1} کاراکتر مجاز است")]
		public required string CourseDescription { get; set; }

		[Display(Name = "قیمت دوره")]
		[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
		public required int CoursePrice { get; set; }

		[Display(Name = "تگ ها")]
		[MaxLength(500, ErrorMessage = "حداکثر {0}،{1} کاراکتر مجاز است")]
		public string? Tags { get; set; }

		[Display(Name = "نام عکس دوره")]
		[MaxLength(50)]
		public string CourseImageName { get; set; } = "No_Image.jpg";

		[Display(Name = "فعال")]
		public bool IsActive { get; set; } = true;

		[MaxLength(100)]
		public string? DemoFileName { get; set; }

		[Required]
		public required DateTime CreateDate { get; set; }
		public DateTime? UpdateDate { get; set; }

		#region Relations

		[ValidateNever]
		[ForeignKey(nameof(TeacherId))]
		public User.User User { get; set; }

		[ValidateNever]
		[ForeignKey(nameof(GroupId))]
		public CourseGroup CourseGroup { get; set; }

		[ValidateNever]
		[ForeignKey(nameof(SubGroup))]
		public CourseGroup Group { get; set; }

		[ValidateNever]
		[ForeignKey(nameof(StatusId))]
		public CourseStatus CourseStatus { get; set; }

		[ValidateNever]
		[ForeignKey(nameof(LevelId))]
		public CourseLevel CourseLevel { get; set; }

		[ValidateNever]
		public List<CourseEpisode> Episodes { get; set; }
		[ValidateNever]
		public List<OrderDetail> OrderDetails { get; set; }

		[ValidateNever]
		public List<UserCourse> UserCourses { get; set; }

		[ValidateNever]
		public List<CourseComment> CourseComments { get; set; }

		[ValidateNever]
		public List<CourseVote> CourseVotes { get; set; }

		[ValidateNever]
		public List<Question.Question> Questions { get; set; }
		#endregion
	}
}
