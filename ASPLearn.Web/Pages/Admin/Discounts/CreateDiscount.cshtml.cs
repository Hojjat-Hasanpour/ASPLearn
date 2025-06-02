using ASPLearn.Core.Services.Interfaces;
using ASPLearn.DataLayer.Entities.Order;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;

namespace ASPLearn.Web.Pages.Admin.Discounts
{
	public class CreateDiscountModel(IOrderService orderService) : PageModel
	{
		private readonly IOrderService _orderService = orderService;

		[BindProperty]
		public Discount Discount { get; set; }

		public void OnGet()
		{
		}

		public IActionResult OnPost(string stDate = "", string edDate = "")
		{
			if (stDate != "")
			{
				string[] std = stDate.Split('/');
				Discount.StartDate = new DateTime(int.Parse(std[0]), int.Parse(std[1]), int.Parse(std[2]), new PersianCalendar());
			}
			if (edDate != "")
			{
				string[] edd = edDate.Split('/');
				Discount.EndDate = new DateTime(int.Parse(edd[0]), int.Parse(edd[1]), int.Parse(edd[2]), new PersianCalendar());
			}

			if (!ModelState.IsValid && _orderService.IsExistCode(Discount.DiscountCode))
				return Page();
			Discount.DiscountCode = Discount.DiscountCode.Trim();
			_orderService.AddDiscount(Discount);
			return RedirectToPage("Index");
		}

		// /Admin/Discounts/CreateDiscount?handler=CheckCode this is default url
		// /Admin/Discounts/CreateDiscount/CheckCode         We Want to show this url // we use {handler?} to set this
		public IActionResult OnGetCheckCode(string code)
		{
			return Content(_orderService.IsExistCode(code).ToString());
		}
	}
}
