using ASPLearn.Core.Services.Interfaces;
using ASPLearn.DataLayer.Entities.Course;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharpCompress.Archives;

namespace ASPLearn.Web.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;

        public CourseController(ICourseService courseService, IOrderService orderService, IUserService userService)
        {
            _courseService = courseService;
            _orderService = orderService;
            _userService = userService;
        }

        public IActionResult Index(int pageId = 1, string filter = "",
            string getType = "all", string orderByType = "date", int startPrice = 0, int endPrice = 0,
            List<int> selectedGroups = null!)
        {
            ViewBag.selectedGroups = selectedGroups;
            ViewBag.Groups = _courseService.GetAllGroups();
            ViewBag.PageId = pageId;
            return View(_courseService.GetCourse(pageId, filter, getType, orderByType, startPrice, endPrice, selectedGroups, 9));
        }

        [Route("ShowCourse/{id}")]
        public IActionResult ShowCourse(int id, int episode = 0)
        {
            var course = _courseService.GetCourseForShow(id);
            if (course is null)
                return NotFound();
            if (episode != 0 && User.Identity!.IsAuthenticated)
            {
                if (course.Episodes.All(e => e.EpisodeId != episode))
                {
                    return NotFound();
                }

                if (!course.Episodes.First(e => e.EpisodeId == episode).IsFree)
                {
                    if (_orderService.IsUserInCourse(User.Identity!.Name!, id))
                    {
                        return NotFound();
                    }
                }
                var ep = course.Episodes.First(e => e.EpisodeId == episode);
                ViewBag.Episode = ep;
                //string filePath = "";
                string filePath = Path.Combine(ep.IsFree ? "/courseOnline" : "/courseFilesOnline", ep.EpisodeFileName.Replace(".rar", ".mp4"));
                string checkFilePath = Path.Combine(Directory.GetCurrentDirectory(), ep.IsFree ? "wwwroot/courseOnline" : "wwwroot/courseFilesOnline",
                    ep.EpisodeFileName.Replace(".rar", ".mp4"));
                if (!System.IO.File.Exists(checkFilePath))
                {
                    string targetPath = Path.Combine(filePath,
                        ep.IsFree ? "wwwroot/courseOnline" : "/courseFilesOnline");
                    string rarPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/courseFiles",
                        ep.EpisodeFileName);
                    var archive = ArchiveFactory.Open(rarPath);
                    var entries = archive.Entries.OrderBy(x => x.Key.Length);
                    foreach (var entry in entries)
                    {
                        if (Path.GetExtension(entry.Key) == ".mp4")
                            entry.WriteTo(System.IO.File.Create(Path.Combine(targetPath, ep.EpisodeFileName.Replace(".rar", ".mp4"))));
                    }
                }
                ViewBag.FilePath = filePath;
            }
            return View(course);
        }

        [Authorize]
        public ActionResult BuyCourse(int id)
        {
            int orderId = _orderService.AddOrder(User.Identity!.Name!, id);
            return Redirect("/UserPanel/MyOrders/ShowOrder/" + orderId);
        }

        [Route("/DownloadFile/{episodeId:int}")]
        public IActionResult DownloadFile(int episodeId)
        {
            var episode = _courseService.GetEpisodeById(episodeId);
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/courseFiles",
                episode.EpisodeFileName);
            string fileName = episode.EpisodeFileName;

            if (episode.IsFree)
                return File(System.IO.File.ReadAllBytes(filePath), "application/force-download", fileName);

            if (!User.Identity!.IsAuthenticated)
                return Forbid();

            if (!_orderService.IsUserInCourse(User.Identity!.Name!, episode.CourseId))
                return Forbid();

            byte[] file = System.IO.File.ReadAllBytes(filePath);
            return File(file, "application/force-download", fileName);
        }

        [HttpPost]
        public IActionResult CreateComment(CourseComment courseComment)
        {
            courseComment.IsDelete = false;
            courseComment.CreateDate = DateTime.Now;
            courseComment.UserId = _userService.GetUserByUserName(User.Identity!.Name!).UserId;
            _courseService.AddComment(courseComment);
            return View(nameof(ShowComment), _courseService.GetCourseComments(courseComment.CourseId));
        }

        public IActionResult ShowComment(int id, int pageId = 1)
        {
            return View(_courseService.GetCourseComments(id, pageId));
        }

        public IActionResult CourseVote(int id)
        {
            if (_courseService.IsFree(id))
                return PartialView(_courseService.GetCourseVotes(id));
            if (User.Identity.IsAuthenticated && !_orderService.IsUserInCourse(User.Identity!.Name!, id))
            {
                ViewBag.AccessDenied = true;
            }
            return PartialView(_courseService.GetCourseVotes(id));
        }

        [Authorize]
        public IActionResult AddVote(int id, bool vote)
        {
            int userId = _userService.GetUserIdByUserName(User.Identity!.Name!);
            _courseService.AddVote(userId, id, vote);
            return PartialView(nameof(CourseVote), _courseService.GetCourseVotes(id));

        }
    }
}
