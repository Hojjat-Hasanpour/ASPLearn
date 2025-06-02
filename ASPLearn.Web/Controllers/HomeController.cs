using ASPLearn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASPLearn.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWalletService _walletService;
        private readonly ICourseService _courseService;
        public HomeController(IWalletService walletService, ICourseService courseService)
        {
            _walletService = walletService;
            _courseService = courseService;
        }

        public IActionResult Index()
        {
            ViewBag.PopularCourses = _courseService.GetPopularCourses();
            return View(_courseService.GetCourse().Model);
        }

        public IActionResult Error()
        {
            return View();
        }

        //[Route("OnlinePayment/{id}")]
        //public IActionResult OnlinePayment(int id)
        //{
        //	if (HttpContext.Request.Query["Status"] != "" &&
        //		HttpContext.Request.Query["Status"].ToString().ToLower() == "ok" &&
        //		HttpContext.Request.Query["Authority"] != "")
        //	{
        //		string authrity = HttpContext.Request.Query["Authority"]!;
        //		var wallet = _walletService.GetWalletByWalletId(id);
        //		var payment = new ZarinpalSandbox.Payment(wallet.Amount);
        //		var res = payment.Verification(authrity).Result;
        //		if (res.Status == 100)
        //		{
        //			ViewBag.Code = res.RefId;
        //			ViewBag.IsSuccess = true;
        //			wallet.IsPay = true;
        //			_walletService.UpdateWallet(wallet);
        //		}

        //		return View();
        //	}

        //	ViewBag.IsSuccess = false;
        //	return View();
        //}

        //[Route("Home/GetSubGroups/{groupId}")]
        [HttpGet("Home/GetSubGroups/{groupId}")]
        public IActionResult GetSubGroups(int groupId) // JsonResult but IActionResult is same
        {
            List<SelectListItem> list = new List<SelectListItem>()
            {
                new SelectListItem(){Text = "انتخاب کنید",Value = ""}
            };
            list.AddRange(_courseService.GetSubGroupForManageCourse(groupId));
            return Json(new SelectList(list, "Value", "Text"));
        }

        [HttpPost]
        [Route("file-upload")]
        public IActionResult UploadImage(IFormFile upload, string CKEditorFuncNum, string CKEditor, string langCode)
        {
            if (upload.Length <= 0) return null!;

            var fileName = Guid.NewGuid() + Path.GetExtension(upload.FileName).ToLower();

            var path = Path.Combine(
                Directory.GetCurrentDirectory(), "wwwroot/MyImages",
                fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                upload.CopyTo(stream);
            }

            var url = $"{"/MyImages/"}{fileName}";
            return Json(new { uploaded = true, url });
        }

        public IActionResult Error404() => View();
    }
}
