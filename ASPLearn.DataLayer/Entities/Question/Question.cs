using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace ASPLearn.DataLayer.Entities.Question
{
	public class Question
	{
		[Key]
		public int QuestionId { get; set; }

		[Required]
		public int CourseId { get; set; }

		[Required]
		public int UserId { get; set; }

		[Display(Name = "عنوان سوال")]
		[Required(ErrorMessage = "{0} را وارد کنید")]
		[MaxLength(300)]
		public required string Title { get; set; }

		[Display(Name = "متن سوال")]
		[Required(ErrorMessage = "{0} را وارد کنید")]
		public required string Body { get; set; }

		[Required] public DateTime CreateDate { get; set; } = DateTime.Now;
		[Required] public DateTime ModifiedDate { get; set; } = DateTime.Now;

		//Navigation Properties
		[ValidateNever]
		public User.User User { get; set; }

		[ValidateNever]
		public Course.Course Course { get; set; }

		[ValidateNever]
		public List<Answer> Answers { get; set; }
	}
}
