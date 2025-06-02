using ASPLearn.Core.Services.Interfaces;
using ASPLearn.DataLayer.Entities.Course;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASPLearn.Web.Pages.Admin.Courses
{
	public class CreateCourseModel(ICourseService courseService) : PageModel
	{
		private readonly ICourseService _courseService = courseService;

		[BindProperty]
		public Course Course { get; set; }
		public void OnGet()
		{
			var groups = _courseService.GetGroupForManageCourse();
			ViewData["Groups"] = new SelectList(groups, "Value", "Text");

			var subGroups = new List<SelectListItem>()
			{
				new SelectListItem()
				{
					Text = "انتخاب کنید",
					Value = ""
				}
			};
			subGroups.AddRange(_courseService.GetSubGroupForManageCourse(int.Parse(groups.First().Value)));
			ViewData["SubGroups"] = new SelectList(subGroups, "Value", "Text");

			var teachers = _courseService.GetTeachers();
			ViewData["Teachers"] = new SelectList(teachers, "Value", "Text");

			var levels = _courseService.GetLevels();
			ViewData["Levels"] = new SelectList(levels, "Value", "Text");
			
			var statuses = _courseService.GetStatuses();
			ViewData["Statuses"] = new SelectList(statuses, "Value", "Text");
			//TempData.Keep();
		}

		public IActionResult OnPost(IFormFile imgCourseUp, IFormFile demoUp)
		{
			if (!ModelState.IsValid)
			{
				var groups = _courseService.GetGroupForManageCourse();
				ViewData["Groups"] = new SelectList(groups, "Value", "Text");

				var subGroups = new List<SelectListItem>()
			{
				new SelectListItem()
				{
					Text = "انتخاب کنید",
					Value = ""
				}
			};
				subGroups.AddRange(_courseService.GetSubGroupForManageCourse(int.Parse(groups.First().Value)));
				ViewData["SubGroups"] = new SelectList(subGroups, "Value", "Text");

				var teachers = _courseService.GetTeachers();
				ViewData["Teachers"] = new SelectList(teachers, "Value", "Text");

				var levels = _courseService.GetLevels();
				ViewData["Levels"] = new SelectList(levels, "Value", "Text");

				var statuses = _courseService.GetStatuses();
				ViewData["Statuses"] = new SelectList(statuses, "Value", "Text");
				return Page();
			}
			
			_courseService.AddCourse(Course, imgCourseUp, demoUp);

			return RedirectToPage("Index");
		}
	}
}
