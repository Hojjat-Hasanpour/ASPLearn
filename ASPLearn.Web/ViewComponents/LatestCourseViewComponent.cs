using ASPLearn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ASPLearn.Web.ViewComponents
{
	public class LatestCourseViewComponent(ICourseService courseService) : ViewComponent
	{
		private readonly ICourseService _courseService = courseService;

		public async Task<IViewComponentResult> InvokeAsync()
		{
			return await Task.FromResult((IViewComponentResult)View("LatestCourse", _courseService.GetCourse().Model)); //Tuple.Item1
		}
	}
}
