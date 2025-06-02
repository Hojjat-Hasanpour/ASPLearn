using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPLearn.DataLayer.Entities.Course
{
	public class CourseVote
	{
		[Key]
		public int VoteId { get; set; }

		public int CourseId { get; set; }
		public int UserId { get; set; }
		public bool Vote { get; set; } // True=like False=dislike
		public DateTime DateVote { get; set; } = DateTime.Now;

		//Navigation Properties
		[ForeignKey(nameof(UserId))]
		[ValidateNever]
		public User.User User { get; set; }

		[ForeignKey(nameof(CourseId))]
		[ValidateNever]
		public Course Course { get; set; }

	}
}
