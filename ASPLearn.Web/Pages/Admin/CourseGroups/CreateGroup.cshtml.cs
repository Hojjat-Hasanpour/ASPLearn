using ASPLearn.Core.Services.Interfaces;
using ASPLearn.DataLayer.Entities.Course;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASPLearn.Web.Pages.Admin.CourseGroups
{
    public class CreateGroupModel(ICourseService courseService) : PageModel
    {
        private readonly ICourseService _courseService = courseService;

        [BindProperty]
        public CourseGroup CourseGroup { get; set; }
        public void OnGet(int? id)
        {
            CourseGroup = new CourseGroup
            {
                GroupName = "",
                ParentId = id
            };
        }

        public IActionResult OnPost(CourseGroup group)
        {
            ModelState.Remove("GroupName");
            if (!ModelState.IsValid)
                return Page();

            _courseService.AddGroup(CourseGroup);
            return RedirectToPage("Index");
        }
    }
}
