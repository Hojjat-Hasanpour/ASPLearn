using ASPLearn.Core.Services.Interfaces;
using ASPLearn.DataLayer.Entities.Course;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASPLearn.Web.Pages.Admin.CourseGroups
{
	public class IndexModel(ICourseService courseService) : PageModel
	{
		private readonly ICourseService _courseService = courseService;

		public List<CourseGroup> CourseGroups { get; set; }
		public void OnGet()
		{
			CourseGroups = _courseService.GetAllGroups();
		}
	}
}
