using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPLearn.Core.Services.Interfaces;
using ASPLearn.DataLayer.Entities.Course;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASPLearn.Web.Pages.Admin.Courses
{
	public class IndexEpisodeModel : PageModel
	{
		private ICourseService _courseService;

		public IndexEpisodeModel(ICourseService courseService)
		{
			_courseService = courseService;
		}

		public List<CourseEpisode> CourseEpisodes { get; set; }
		public void OnGet(int id)
		{
			ViewData["CourseId"] = id;
			CourseEpisodes = _courseService.GetListEpisodeCourse(id);
		}
	}
}