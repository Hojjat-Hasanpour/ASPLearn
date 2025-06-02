using ASPLearn.Core.Services.Interfaces;
using ASPLearn.DataLayer.Entities.Course;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASPLearn.Web.Pages.Admin.Courses
{
    public class CreateEpisodeModel : PageModel
    {
		private ICourseService _courseService;

		public CreateEpisodeModel(ICourseService courseService)
		{
			_courseService = courseService;
		}



		[BindProperty]
		public CourseEpisode CourseEpisode { get; set; }
		public void OnGet(int id)
		{
			CourseEpisode = new CourseEpisode() { 
				EpisodeTitle="",
				CourseId = id,
				Course = _courseService.GetCourseById(id),
				EpisodeFileName = "Text"
			};
			//CourseEpisode.CourseId = id;

		}


		public IActionResult OnPost(IFormFile fileEpisode)
		{
			if (!ModelState.IsValid || fileEpisode == null)
				return Page();

			if (_courseService.CheckExistFile(fileEpisode.FileName))
			{
				ViewData["IsExistFile"] = true;
				return Page();
			}

			CourseEpisode.Course = _courseService.GetCourseById(CourseEpisode.CourseId);
			_courseService.AddEpisode(CourseEpisode, fileEpisode);

			return Redirect("/Admin/Courses/IndexEpisode/" + CourseEpisode.CourseId);
		}

	}
}
