using ASPLearn.DataLayer.Entities.Order;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPLearn.DataLayer.Entities.User
{
	public class UserDiscountCode
	{
		[Key]
		public int UD_Id { get; set; }
		public int UserId { get; set; }
		public int DiscountId { get; set; }


		[ValidateNever]
		[ForeignKey(nameof(UserId))]
		public User User { get; set; }

		[ValidateNever]
		[ForeignKey(nameof(DiscountId))]
		public Discount Discount { get; set; }
	}
}
