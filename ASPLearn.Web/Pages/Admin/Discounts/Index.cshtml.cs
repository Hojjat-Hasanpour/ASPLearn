using ASPLearn.Core.Services.Interfaces;
using ASPLearn.DataLayer.Entities.Order;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASPLearn.Web.Pages.Admin.Discounts
{
	public class IndexModel(IOrderService orderService) : PageModel
	{
		private readonly IOrderService _orderService = orderService;
		public List<Discount> Discounts { get; set; }
		public void OnGet()
		{
			Discounts = _orderService.GetAllDiscounts();
		}
	}
}
