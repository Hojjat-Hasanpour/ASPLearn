using ASPLearn.Core.DTOs;
using ASPLearn.DataLayer.Entities.User;

namespace ASPLearn.Core.Services.Interfaces
{
	public interface IUserService
	{
		bool IsExistUserName(string userName);
		bool IsExistEmail(string email);
		int AddUser(User user);
		User LoginUser(LoginViewModel login);
		User GetUserByUserName(string userName);
		int GetUserIdByUserName(string userName);
		User GetUserByEmail(string email);
		User GetUserByActiveCode(string activeCode);
		void UpdateUser(User user);
		bool ActiveAccount(string activeCode);

		#region User Panel
		InformationUserViewModel GetUserInformation(string userName);
		SideBarUserPanelViewModel GetSideBarUserPanelData(string userName);
		EditProfileViewModel GetDataEditProfile(string userName);
		void EditProfile(string userName, EditProfileViewModel profile);
		bool CompareOldPassword(string userName, string oldPassword);
		void ChangeUserPassword(string userName, string password);
		#endregion
	}
}
