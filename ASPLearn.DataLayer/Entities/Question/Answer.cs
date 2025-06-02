using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace ASPLearn.DataLayer.Entities.Question
{
	public class Answer
	{
		[Key]
		public int AnswerId { get; set; }

		[Required]
		public int QuestionId { get; set; }

		[ValidateNever]
		public Question Question { get; set; }

		[Required]
		public int UserId { get; set; }

		[ValidateNever]
		public User.User User { get; set; }

		[Required]
		public required string BodyAnswer { get; set; }

		public bool IsTrue { get; set; } = false;

		[Required]
		public DateTime CreateDate { get; set; }
	}
}
