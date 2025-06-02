using ASPLearn.Core.Services.Interfaces;
using ASPLearn.DataLayer.Entities.Order;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;

namespace ASPLearn.Web.Pages.Admin.Discounts
{
	public class EditDiscountModel(IOrderService orderService) : PageModel
	{
		private readonly IOrderService _orderService = orderService;

		[BindProperty]
		public Discount Discount { get; set; }

		public void OnGet(int id)
		{
			Discount = _orderService.GetDiscountById(id)!;
		}

		public IActionResult OnPost(string stDate = "", string edDate = "")
		{
			if (stDate != "")
			{
				string[] std = stDate.Split('/');
				Discount.StartDate = new DateTime(int.Parse(std[0]),
					int.Parse(std[1]),
					int.Parse(std[2]),
					new PersianCalendar()
				);
			}

			if (edDate != "")
			{
				string[] edd = edDate.Split('/');
				Discount.EndDate = new DateTime(int.Parse(edd[0]),
					int.Parse(edd[1]),
					int.Parse(edd[2]),
					new PersianCalendar()
				);
			}

			if (!ModelState.IsValid)
				return Page();

			_orderService.UpdateDiscount(Discount);

			return RedirectToPage("Index");

		}
	}
}
