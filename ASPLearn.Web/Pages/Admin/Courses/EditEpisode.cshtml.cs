using ASPLearn.Core.Services.Interfaces;
using ASPLearn.DataLayer.Entities.Course;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASPLearn.Web.Pages.Admin.Courses
{
    public class EditEpisodeModel : PageModel
    {
		private ICourseService _courseService;

		public EditEpisodeModel(ICourseService courseService)
		{
			_courseService = courseService;
		}

		[BindProperty]
		public CourseEpisode CourseEpisode { get; set; }
		public void OnGet(int id)
		{
			CourseEpisode = _courseService.GetEpisodeById(id);
		}

		public IActionResult OnPost(IFormFile fileEpisode)
		{
			ModelState.Remove("fileEpisode");
			if (!ModelState.IsValid)
				return Page();

			if (fileEpisode != null)
			{
				if (_courseService.CheckExistFile(fileEpisode.FileName))
				{
					ViewData["IsExistFile"] = true;
					return Page();
				}
			}

			CourseEpisode.Course = _courseService.GetCourseById(CourseEpisode.CourseId);
			_courseService.EditEpisode(CourseEpisode, fileEpisode);

			return Redirect("/Admin/Courses/IndexEpisode/" + CourseEpisode.CourseId);
		}
	}
}
