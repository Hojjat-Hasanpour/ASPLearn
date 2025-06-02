using ASPLearn.Core.DTOs;
using ASPLearn.DataLayer.Entities.Wallet;

namespace ASPLearn.Core.Services.Interfaces
{
	public interface IWalletService
	{
		int BalanceUserWallet(int userId);
		int BalanceUserWallet(string userName);
		List<WalletViewModel> GetUserWallet(int userId);
		int ChargeWallet(int userId, int amount, string description, bool isPay = false);
		int AddWallet(Wallet wallet);
		Wallet GetWalletByWalletId(int walletId);
		int UpdateWallet(Wallet wallet);

		Wallet? GetCurrentUserWallet(int userId);
		ConfirmWalletViewModel? GetConfirmUserForCharge(int walletId);
	}
}
