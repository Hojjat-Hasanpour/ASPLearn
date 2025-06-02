using ASPLearn.Core.DTOs.Course;
using ASPLearn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASPLearn.Web.Pages.Admin.Courses
{
	public class IndexModel(ICourseService courseService) : PageModel
	{
		private readonly ICourseService _courseService = courseService;

		public List<ShowCourseForAdminViewModel> ListCourses { get; set; } = new List<ShowCourseForAdminViewModel>();
		public void OnGet()
		{
			ListCourses = _courseService.GetCoursesForAdmin();
		}
	}
}
