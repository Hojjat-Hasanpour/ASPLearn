using ASPLearn.Core.DTOs;
using ASPLearn.Core.Services.Interfaces;
using ASPLearn.DataLayer.Context;
using ASPLearn.DataLayer.Entities.Wallet;


namespace ASPLearn.Core.Services
{
	public class WalletService : IWalletService
	{
		private ASPLearnContext _context;
		//private IUserService _user_service;
		public WalletService(ASPLearnContext context)
		{
			_context = context;
		}

		public int AddWallet(Wallet wallet)
		{
			_context.Wallets.Add(wallet);
			_context.SaveChanges();
			return wallet.WalletId;
		}

		public int BalanceUserWallet(int userId)
		{
			//int userId = _user_service.GetUserIdByUserName(userName);
			var deposit = _context.Wallets.Where(w => w.UserId == userId && w.WalletTypeId == 1 && w.IsPay == true).Select(w => w.Amount).ToList();
			var withdraw = _context.Wallets.Where(w => w.UserId == userId && w.WalletTypeId == 2).Select(w => w.Amount).ToList();
			int depositAmount = deposit.Count > 0 ? deposit.Sum() : 0;
			int withdrawAmount = withdraw.Count > 0 ? withdraw.Sum() : 0;
			return depositAmount - withdrawAmount;
		}

		public int BalanceUserWallet(string userName)
		{
			var userId = _context.Users.First(u=>u.UserName == userName).UserId;
			return BalanceUserWallet(userId);
		}

		public int ChargeWallet(int userId, int amount, string description, bool isPay = false)
		{
			Wallet? wallet = GetCurrentUserWallet(userId);
			if (wallet == null)
			{
				wallet = new Wallet
				{
					Amount = amount,
					CreatedAt = DateTime.Now,
					Description = description,
					IsPay = isPay,
					WalletTypeId = isPay ? 2 : 1,
					UserId = userId
				};
				return AddWallet(wallet);
			}
			wallet.Amount = amount;
			return UpdateWallet(wallet);
		}

		public ConfirmWalletViewModel? GetConfirmUserForCharge(int walletId)
		{
			var wallet = _context.Wallets.Find(walletId);
			if (wallet == null)
				return null;
			return new ConfirmWalletViewModel() { Amount = wallet.Amount, WalletId = wallet.WalletId };
		}

		public Wallet? GetCurrentUserWallet(int userId)
		{
			return _context.Wallets.FirstOrDefault(w => w.UserId == userId && w.WalletTypeId == 1 && w.IsPay == false);
		}

		public List<WalletViewModel> GetUserWallet(int userId)
		{
			return _context.Wallets.Where(w => w.UserId == userId && w.IsPay == true)
				.Select(w => new WalletViewModel
				{
					Amount = w.Amount,
					Type = w.WalletTypeId,
					Description = w.Description,
					Date = w.CreatedAt
				}).ToList();
		}

		public Wallet GetWalletByWalletId(int walletId)
		{
			return _context.Wallets.Find(walletId)!;
		}

		public int UpdateWallet(Wallet wallet)
		{
			_context.Wallets.Update(wallet);
			_context.SaveChanges();
			return wallet.WalletId;
		}
	}
}
