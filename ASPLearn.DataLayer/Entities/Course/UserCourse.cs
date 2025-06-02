using System.ComponentModel.DataAnnotations;

namespace ASPLearn.DataLayer.Entities.Course
{
	public class UserCourse
	{
		[Key]
		public int UserCourseId { get; set; }
		public int UserId { get; set; }
		public int CourseId { get; set; }

		public Course Course { get; set; }
		public User.User User { get; set; }
	}
}
