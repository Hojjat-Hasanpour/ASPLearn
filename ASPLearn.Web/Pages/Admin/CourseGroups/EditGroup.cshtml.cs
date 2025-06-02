using ASPLearn.Core.Services.Interfaces;
using ASPLearn.DataLayer.Entities.Course;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASPLearn.Web.Pages.Admin.CourseGroups
{
    public class EditGroupModel(ICourseService courseService) : PageModel
    {
        private readonly ICourseService _courseService = courseService;
        [BindProperty]
        public CourseGroup CourseGroup { get; set; }
        public void OnGet(int id)
        {
            CourseGroup = _courseService.GetGroupById(id)!;
        }

        public IActionResult OnPost(CourseGroup group)
        {
            //ModelState.Remove("GroupName");
            if (!ModelState.IsValid)
                return Page();

            _courseService.UpdateGroup(group);

            return RedirectToPage("Index");
        }
    }
}
