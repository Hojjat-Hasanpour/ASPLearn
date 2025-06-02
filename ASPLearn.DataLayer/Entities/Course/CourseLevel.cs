using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPLearn.DataLayer.Entities.Course
{
	public class CourseLevel
	{
		[Key]
		public int LevelId { get; set; }

		[Display(Name = "نام سطح")]
		[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
		[MaxLength(150, ErrorMessage = "حداکثر {0}،{1} کاراکتر مجاز است")]
		public required string LevelName { get; set; }

		List<Course> Courses { get; set; }
	}
}
