using ASPLearn.Core.DTOs;
using ASPLearn.Core.Services.Interfaces;
using ASPLearn.DataLayer.Entities.Wallet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPLearn.Web.Areas.UserPanel.Controllers
{
	[Area("UserPanel")]
	[Authorize]
	public class WalletController : Controller
	{
		private IUserService _user_service;
		private IWalletService _wallet_service;

		public WalletController(IUserService userService, IWalletService walletService)
		{
			_user_service = userService;
			_wallet_service = walletService;
		}

		[Route("UserPanel/Wallet")]
		public IActionResult Index()
		{
			ViewBag.ListWallet = _wallet_service.GetUserWallet(_user_service.GetUserByUserName(User.Identity!.Name!).UserId);
			return View();
		}

		[HttpPost]
		[Route("UserPanel/Wallet")]
		public IActionResult Index(ChargeWalletViewModel charge)
		{
			int userId = _user_service.GetUserByUserName(User.Identity!.Name!).UserId;
			if (!ModelState.IsValid)
			{
				ViewBag.ListWallet = _wallet_service.GetUserWallet(userId);
				return View(charge);
			}
			int walletId = _wallet_service.ChargeWallet(userId, charge.Amount, "شارژ حساب");

			#region Online Payment

			//var payment = new ZarinpalSandbox.Payment(charge.Amount);
			//var res = payment.PaymentRequest("شارژ حساب کاربری", "https://localhost:7007/OnlinePayment/" + walletId);
			//if (res.Result.Status == 100)
			//{
			//	return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + res.Result.Authority);
			//}

			return Redirect("OnlinePayment/" + walletId);
			#endregion
			//return null!;
		}

		[HttpGet("UserPanel/OnlinePayment/{id}")]
		public IActionResult OnlinePayment(int id)
		{
			var CurrentUserWallet = _wallet_service.GetConfirmUserForCharge(id);
			if (CurrentUserWallet == null)
				return NotFound();
			return View(CurrentUserWallet);
		}

		[HttpPost("UserPanel/OnlinePayment/{id}")]
		public IActionResult OnlinePayment(ConfirmWalletViewModel wallet)
		{
			if (!ModelState.IsValid)
			{
				ViewBag.CurrentUserWallet = _wallet_service.GetWalletByWalletId(wallet.WalletId);
				return View(wallet);
			}
			Wallet dbWallet = _wallet_service.GetWalletByWalletId(wallet.WalletId);
			dbWallet.IsPay = true;
			_wallet_service.UpdateWallet(dbWallet);
			return RedirectToAction("Index");
		}
	}
}
