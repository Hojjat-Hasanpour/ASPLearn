using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPLearn.Core.DTOs.Course
{
	public class ShowCourseForAdminViewModel
	{
		public int CourseId { get; set; }
		public required string CourseTitle { get; set; }

		public required string ImageName { get; set; }

		public int EpisodeCount { get; set; }

	}
}
