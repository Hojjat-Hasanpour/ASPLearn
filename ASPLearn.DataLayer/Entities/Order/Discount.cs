using ASPLearn.DataLayer.Entities.User;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace ASPLearn.DataLayer.Entities.Order
{
	public class Discount
	{
		[Key]
		public int DiscountId { get; set; }

		[Display(Name = "کد تخفیف")]
		[Required(ErrorMessage = "لطفا {0} را وارد کنید ")]
		[MaxLength(150)]
		public required string DiscountCode { get; set; }

		[Display(Name = "درصد تخفیف")]
		[Required(ErrorMessage = "لطفا {0} را وارد کنید ")]
		public required int DiscountPercent { get; set; }

		public int? UsableCount { get; set; }

		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }

		[ValidateNever]
		public List<UserDiscountCode> UserDiscountCodes { get; set; }
	}
}
