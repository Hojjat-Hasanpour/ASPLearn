using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPLearn.DataLayer.Entities.Course
{
	public class CourseGroup
	{
		[Key]
		public int GroupId { get; set; }

		[Display(Name = "نام گروه")]
		[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
		[MaxLength(200, ErrorMessage = "حداکثر {0}،{1} کاراکتر مجاز است")]
		public required string GroupName { get; set; }

		[Display(Name = "حذف شده")]
		public bool IsDelete { get; set; } = false;

		[Display(Name ="نام سرگروه")]
		public int? ParentId { get; set; }

		[ForeignKey(nameof(ParentId))]
		public List<CourseGroup>? CourseGroups { get; set; }

		[InverseProperty("CourseGroup")]
		public List<Course> Courses { get; set; }

		[InverseProperty("Group")]
		public List<Course> SubGroups { get; set; }
	}
}
