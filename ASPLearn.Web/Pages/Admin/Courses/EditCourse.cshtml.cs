using ASPLearn.Core.Services.Interfaces;
using ASPLearn.DataLayer.Entities.Course;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASPLearn.Web.Pages.Admin.Courses
{
	public class EditCourseModel : PageModel
	{
		private ICourseService _courseService;

		public EditCourseModel(ICourseService courseService)
		{
			_courseService = courseService;
		}

		[BindProperty]
		public Course Course { get; set; }
		public void OnGet(int id)
		{
			Course = _courseService.GetCourseById(id);

			var groups = _courseService.GetGroupForManageCourse();
			ViewData["Groups"] = new SelectList(groups, "Value", "Text", Course.GroupId);

			List<SelectListItem> subgroups = new List<SelectListItem>()
			{
				new SelectListItem(){Text = "Please Select", Value = ""}
			};
			subgroups.AddRange(_courseService.GetSubGroupForManageCourse(Course.GroupId));
			string? selectedSubGroup = Course.SubGroup?.ToString();
			ViewData["SubGroups"] = new SelectList(subgroups, "Value", "Text", selectedSubGroup);

			var teachers = _courseService.GetTeachers();
			ViewData["Teachers"] = new SelectList(teachers, "Value", "Text", Course.TeacherId);

			var levels = _courseService.GetLevels();
			ViewData["Levels"] = new SelectList(levels, "Value", "Text", Course.LevelId);

			var statues = _courseService.GetStatuses();
			ViewData["Statues"] = new SelectList(statues, "Value", "Text", Course.StatusId);

		}

		public IActionResult OnPost(IFormFile imgCourseUp, IFormFile demoUp)
		{
			if (!ModelState.IsValid)
				return Page();

			_courseService.UpdateCourse(Course, imgCourseUp, demoUp);

			return RedirectToPage("Index");
		}
	}
}