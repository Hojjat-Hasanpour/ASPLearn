using ASPLearn.DataLayer.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPLearn.Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CourseApiController(ASPLearnContext context) : ControllerBase
	{
		private readonly ASPLearnContext _context = context;

		[Produces("application/json")]
		[HttpGet("search")]
		public async Task<IActionResult> Search()
		{
			try
			{
				string term = HttpContext.Request.Query["term"].ToString();
				var courseTitles = await _context.Courses.Where(c => c.CourseTitle.Contains(term))
					.Select(c => c.CourseTitle).ToListAsync();
				return Ok(courseTitles);
			}
			catch
			{
				return BadRequest();
			}
		}
	}
}
