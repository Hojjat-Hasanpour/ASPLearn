using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace ASPLearn.DataLayer.Entities.Course
{
	public class CourseComment
	{
		[Key]
		public int CommentId { get; set; }

		public int CourseId { get; set; }
		public int UserId { get; set; }

		[MaxLength(700)]
		public required string Comment { get; set; }
		public DateTime CreateDate { get; set; }
		public bool IsDelete { get; set; } = false;
		public bool IsAdminRead { get; set; }

		//Navigation Property
		[ValidateNever]
		public Course Course { get; set; }
		[ValidateNever]
		public User.User User { get; set; }

	}
}
