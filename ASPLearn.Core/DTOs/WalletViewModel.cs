using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPLearn.Core.DTOs
{
	public class ChargeWalletViewModel
	{
		[Display(Name = "مبلغ")] // To Toman
		[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
		public int Amount { get; set; }
	}

	public class WalletViewModel
	{
		[Display(Name = "موجودی کیف پول")]
		public required int Amount { get; set; }
		public required int Type { get; set; }
		public string? Description { get; set; }
		public required DateTime Date { get; set; }
	}

	public class ConfirmWalletViewModel
	{
		public int WalletId { get; set; }

		[Display(Name = "مبلغ")] // To Toman
		[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
		public required int Amount { get; set; }
	}
}