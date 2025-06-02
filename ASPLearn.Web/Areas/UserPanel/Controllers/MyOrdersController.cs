using ASPLearn.Core.DTOs.Order;
using ASPLearn.Core.Services.Interfaces;
using ASPLearn.DataLayer.Entities.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPLearn.Web.Areas.UserPanel.Controllers
{
	[Area("UserPanel")]
	[Authorize]
	public class MyOrdersController : Controller
	{
		private readonly IOrderService _orderService;

		public MyOrdersController(IOrderService orderService)
		{
			_orderService = orderService;
		}

		public IActionResult Index()
		{
			return View(_orderService.GetUserOrders(User.Identity!.Name!));
		}

		public IActionResult ShowOrder(int id, bool finaly = false, string type = "")
		{
			Order? order = _orderService.GetOrderForUserPanel(User.Identity!.Name!, id);
			if (order == null)
				NotFound();
			ViewBag.TypeDiscount = type;
			ViewBag.Finaly = finaly;
			return View(order);
		}

		public IActionResult FinallyOrder(int id)
		{
			if (_orderService.FinallyOrder(User.Identity!.Name!, id))
			{
				return Redirect("/UserPanel/MyOrders/ShowOrder/" + id + "?finally=true");
			}

			return BadRequest();
		}

		public IActionResult UseDiscount(int orderId, string code)
		{
			DiscountUseType type = _orderService.UseDiscount(orderId, code);
			return Redirect($"/UserPanel/MyOrders/ShowOrder/{orderId}?type={type.ToString()}");
		}
	}
}
