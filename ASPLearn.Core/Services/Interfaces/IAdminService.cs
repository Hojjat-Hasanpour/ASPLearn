using ASPLearn.Core.DTOs;
using ASPLearn.DataLayer.Entities.Permissions;
using ASPLearn.DataLayer.Entities.User;

namespace ASPLearn.Core.Services.Interfaces
{
	public interface IAdminService
	{
		public UserForAdminViewModel GetUsers(int pageId = 1, string filterEmail = "", string filterUserName = "");
		public UserForAdminViewModel GetDeletedUsers(int pageId = 1, string filterEmail = "", string filterUserName = "");
		List<Role> GetRoles();
		int AddUserFormAdmin(CreateUserViewModel user);
		void AddRolesToUser(List<int> roleIds,int userId);
		UpdateUserViewModel ShowUserInAdmin(int userId);
		void UpdateUserInAdmin(UpdateUserViewModel updateUser);
		void UpdateUserRoles(int userId,List<int> roleIds);
		void DeleteUser(int userId);

		InformationUserViewModel InformationUserViewModel(int userId);
		void RestoreDeletedUser(int userId);

		int AddRole(Role role);
		void UpdateRole(Role role);
		void DeleteRole(Role role);
		//Role EditRole(int roleId);
		Role GetRoleById(int roleId);
		List<int> GetUserRoles(int userId);

		List<Permission> GetAllPermissions();
		void AddPermissionsToRole(List<int> permissionIds, int roleId);
		List<int> GetPermissionsRole(int roleId);
		void UpdatePermissionRoles(List<int> permissionIds, int roleId);
		bool CheckPermission(int permissionId,string userName);
	}
}
