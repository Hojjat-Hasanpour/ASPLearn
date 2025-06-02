using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPLearn.DataLayer.Entities.Course
{
	public class CourseStatus
	{
		[Key]
		public int StatusId { get; set; }

		[Display(Name = "نام سطح")]
		[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
		[MaxLength(150, ErrorMessage = "حداکثر {0}،{1} کاراکتر مجاز است")]
		public required string StatusName { get; set; }

		public List<Course> Courses { get; set; }
	}
}
