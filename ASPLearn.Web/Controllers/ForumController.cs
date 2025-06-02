using ASPLearn.Core.Services.Interfaces;
using ASPLearn.DataLayer.Entities.Question;
using Ganss.Xss;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPLearn.Web.Controllers
{
    public class ForumController : Controller
    {
        private readonly IForumService _forumService;
        private readonly IUserService _userService;
        public ForumController(IForumService forumService, IUserService userService)
        {
            _forumService = forumService;
            _userService = userService;
        }

        public IActionResult Index(int? courseId, string filter = "")
        {
            ViewBag.CourseId = courseId;
            return View(_forumService.GetQuestions(courseId, filter));
        }

        [Authorize]
        public IActionResult CreateQuestion(int id) //courseId
        {
            Question question = new Question()
            {
                CourseId = id,
                UserId = _userService.GetUserIdByUserName(User.Identity!.Name!),
                Body = "",
                Title = "",
            };
            return View(question);
        }

        [HttpPost]
        public IActionResult CreateQuestion(Question question) //courseId
        {
            if (!ModelState.IsValid)
                return View(question);

            int questionId = _forumService.AddQuestion(question);
            return Redirect($"/Forum/ShowQuestion/{questionId}");
        }

        public IActionResult ShowQuestion(int id) => View(_forumService.ShowQuestion(id));

        [Authorize]
        public IActionResult Answer(int id, string body)
        {
            if (!string.IsNullOrEmpty(body))
            {
                var sanitizer = new HtmlSanitizer();
                var sanitized = sanitizer.Sanitize(body);
                _forumService.AddAnswer(new Answer()
                {
                    BodyAnswer = sanitized,
                    CreateDate = DateTime.Now,
                    QuestionId = id,
                    UserId = _userService.GetUserIdByUserName(User.Identity!.Name!)
                });
            }
            return RedirectToAction(nameof(ShowQuestion), new { id = id });
        }

        [Authorize]
        public IActionResult SelectIsTrueAnswer(int questionId, int answerId)
        {
            int userId = _userService.GetUserIdByUserName(User.Identity!.Name!);
            var question = _forumService.ShowQuestion(questionId);
            if (question.Question.UserId == userId)
            {
                _forumService.ChangeIsTrueAnswer(questionId, answerId);
            }
            return RedirectToAction(nameof(ShowQuestion), new { id = questionId });
        }
    }
}
