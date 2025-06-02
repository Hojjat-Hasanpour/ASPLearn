using ASPLearn.Core.Services.Interfaces;
using ASPLearn.DataLayer.Entities.Course;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASPLearn.Web.Pages.Admin.CourseGroups
{
	public class EditGroupModel(ICourseService courseService) : PageModel
	{
		private readonly ICourseService _courseService = courseService;
		public CourseGroup CourseGroup { get; set; }
		public void OnGet()
		{

		}

		public IActionResult OnPost(CourseGroup group)
		{
			if (!ModelState.IsValid)
				return Page();

			_courseService.UpdateGroup(group);

			return RedirectToPage("Index");
		}
	}
}
