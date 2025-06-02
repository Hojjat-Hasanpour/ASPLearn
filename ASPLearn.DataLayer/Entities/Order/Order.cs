using ASPLearn.DataLayer.Entities.User;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPLearn.DataLayer.Entities.Order
{
	public class Order
	{
		[Key]
		public int OrderId { get; set; }
		[Display(Name = "شناسه کاربر")]
		[Required(ErrorMessage ="لطفا {0} را وارد کنید")]
		public int UserId { get; set; }

		[Display(Name = "جمع فاکتور")]
		[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
		public int OrderSum { get; set; }

		[Display(Name ="نهایی شده")]
		public bool IsFinal { get; set; } = false;
		[Required]

		[Display(Name ="")]
		public DateTime CreateDate { get; set; }

		[ValidateNever]
		public virtual User.User User { get; set; }

		[ValidateNever]
		public virtual List<OrderDetail> OrderDetails { get; set; }
	}
}
