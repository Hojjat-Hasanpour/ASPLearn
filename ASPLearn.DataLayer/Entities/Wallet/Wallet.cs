using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPLearn.DataLayer.Entities.Wallet
{
	public class Wallet
	{
		public Wallet()
		{
			
		}

		[Key]
		public int WalletId { get; set; }

		[Display(Name = "نوع تراکنش")]
		[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
		public required int WalletTypeId { get; set; }

		[Display(Name = "کاربر")]
		[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
		public required int UserId { get; set; }

		[Display(Name = "مبلغ")]
		[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
		public required int Amount { get; set; }

		[Display(Name = "شرح")]
		[MaxLength(500, ErrorMessage = "حداکثر طول {0}، {1} کاراکتر می باشد")]
		public string? Description { get; set; }

		[Display(Name = "پرداخت شده")]
		public bool IsPay { get; set; } = false;

		[Display(Name = "تاریخ و ساعت")]
		public DateTime CreatedAt { get; set; }

		#region Relations
		public virtual User.User User { get; set; }
		public virtual WalletType WalletType { get; set; }
		#endregion
	}
}
