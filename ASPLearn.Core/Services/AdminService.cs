using ASPLearn.Core.DTOs;
using ASPLearn.Core.Generator;
using ASPLearn.Core.Security;
using ASPLearn.Core.Services.Interfaces;
using ASPLearn.DataLayer.Context;
using ASPLearn.DataLayer.Entities.Permissions;
using ASPLearn.DataLayer.Entities.User;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ASPLearn.Core.Services
{
	public class AdminService : IAdminService
	{
		private readonly ASPLearnContext _context;
		private readonly IWalletService _wallet_service;
		public AdminService(ASPLearnContext context, IWalletService wallet_service)
		{
			_context = context;
			_wallet_service = wallet_service;
		}

		public void AddRolesToUser(List<int> roleIds, int userId)
		{
			foreach (int roleId in roleIds)
			{
				UserRole userRole = new()
				{
					UserId = userId,
					RoleId = roleId
				};
				_context.UserRoles.Add(userRole);
			}
			_context.SaveChanges();
		}

		public int AddUserFormAdmin(CreateUserViewModel user)
		{
			User newUser = new()
			{
				UserName = user.UserName,
				Email = user.Email,
				Password = PasswordHelper.EncodePasswordMd5(user.Password),
				RegisterDate = DateOnly.FromDateTime(DateTime.Now),
				IsActive = true,
				ActiveCode = NameGenerator.GenerateUniqCode()
				//UserAvatar = user.UserAvatar,
			};

			// User Avatar
			if (user.UserAvatar != null)
				newUser.UserAvatar = SaveUserAvatar(user.UserAvatar);
			
			// Check if the user already exists ?
			// Add the new user to the context and save changes
			_context.Users.Add(newUser);
			_context.SaveChanges();

			return newUser.UserId;
		}

		public void DeleteUser(int userId)
		{
			User user = _context.Users.Find(userId)!;
			if (user is null)
				throw new Exception("User not found");
			user.IsDelete = true;
			_context.Users.Update(user);
			_context.SaveChanges();
		}

		public UserForAdminViewModel GetDeletedUsers(int pageId = 1, string filterEmail = "", string filterUserName = "")
		{
			// The only difference with GetUsers
			IQueryable<User> users = _context.Users.IgnoreQueryFilters().Where(u=> u.IsDelete);

			if (!string.IsNullOrEmpty(filterEmail))
			{
				users = users.Where(u => u.Email.Contains(filterEmail));
			}

			if (!string.IsNullOrEmpty(filterUserName))
			{
				users = users.Where(u => u.UserName.Contains(filterUserName));
			}

			int pageSize = 20;
			int skip = (pageId - 1) * pageSize;
			UserForAdminViewModel list = new UserForAdminViewModel
			{
				Users = users.OrderBy(u => u.RegisterDate).Skip(skip).Take(pageSize).ToList(),
				CurrentPage = pageId,
				PageCount = (int)Math.Ceiling(users.Count() / (double)pageSize)
			};
			return list;
		}

		public List<Role> GetRoles() => _context.Roles.ToList();


		public UserForAdminViewModel GetUsers(int pageId = 1, string filterEmail = "", string filterUserName = "")
		{
			IQueryable<User> users = _context.Users;

			if (!string.IsNullOrEmpty(filterEmail))
			{
				users = users.Where(u => u.Email.Contains(filterEmail));
			}

			if (!string.IsNullOrEmpty(filterUserName))
			{
				users = users.Where(u => u.UserName.Contains(filterUserName));
			}

			int pageSize = 20;
			int skip = (pageId - 1) * pageSize;
			UserForAdminViewModel list = new UserForAdminViewModel
			{
				Users = users.OrderBy(u => u.RegisterDate).Skip(skip).Take(pageSize).ToList(),
				CurrentPage = pageId,
				PageCount = (int)Math.Ceiling(users.Count() / (double)pageSize)
			};
			return list;
		}

		public InformationUserViewModel InformationUserViewModel(int userId)
		{
			var user = _context.Users.Find(userId);
			if (user is null) throw new Exception("User Not Found");

			var info = new InformationUserViewModel()
			{
				UserName = user.UserName,
				Email = user.Email,
				RegisterDate = user.RegisterDate.HasValue ? user.RegisterDate.Value.ToDateTime(TimeOnly.MinValue) : DateTime.MinValue,
				Wallet = _wallet_service.BalanceUserWallet(user.UserId)
			};
			return info;
		}

		public void RestoreDeletedUser(int userId)
		{
			User? user = _context.Users.IgnoreQueryFilters().SingleOrDefault(u=> u.UserId == userId);
			if (user is null) throw new Exception("User Not Found");
			user.IsDelete = false;
			_context.Users.Update(user);
			_context.SaveChanges();
		}

		private string SaveUserAvatar(IFormFile userAvatar)
		{
			string avatarFileName = NameGenerator.GenerateUniqCode() + Path.GetExtension(userAvatar.FileName);
			string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", avatarFileName);
			using var stream = new FileStream(imagePath, FileMode.Create);
			userAvatar.CopyTo(stream);
			return avatarFileName;
		}

		public UpdateUserViewModel ShowUserInAdmin(int userId)
		{
			return _context.Users
				.Where(u => u.UserId == userId)
				.Select(u => new UpdateUserViewModel
				{
					UserId = u.UserId,
					UserName = u.UserName,
					Email = u.Email,
					//Password = PasswordHelper.EncodePasswordMd5(u.Password),
					//UserAvatar = u.UserAvatar,
					AvatarName = u.UserAvatar!,
					UserRoles = _context.UserRoles.Select(r => r.RoleId).ToList()
				}).Single();
		}

		public void UpdateUserInAdmin(UpdateUserViewModel updateUser)
		{
			User user = _context.Users.Single(u => u.UserId == updateUser.UserId && u.UserName == updateUser.UserName);
			if (user is null)
			{
				throw new Exception("User not found");
			}

			user.Email = updateUser.Email;

			if (!string.IsNullOrEmpty(updateUser.Password))
			{
				user.Password = PasswordHelper.EncodePasswordMd5(updateUser.Password);
			}

			if (updateUser.UserAvatar != null)
			{
				if (updateUser.AvatarName != "UserAvatarPic.png")
				{
					string oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", updateUser.AvatarName);
					if (File.Exists(oldImagePath))
						File.Delete(oldImagePath);
				}

				user.UserAvatar = SaveUserAvatar(updateUser.UserAvatar);
				/*user.UserAvatar = NameGenerator.GenerateUniqCode() + Path.GetExtension(updateUser.UserAvatar.FileName);
				string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", user.UserAvatar);
				using var stream = new FileStream(imagePath, FileMode.Create);
				updateUser.UserAvatar.CopyTo(stream);*/
			}

			_context.Users.Update(user);
			_context.SaveChanges();
		}

		public void UpdateUserRoles(int userId, List<int> roleIds)
		{
			// Delete all roles of the user
			_context.UserRoles.Where(ur => ur.UserId == userId).ToList().ForEach(r => _context.UserRoles.Remove(r));
			// Add new roles to the user
			AddRolesToUser(roleIds, userId);
		}

		public int AddRole(Role role)
		{
			_context.Add(role);
			_context.SaveChanges();
			return role.RoleId;
		}

		public Role GetRoleById(int roleId)
		{
			return _context.Roles.Find(roleId);
		}

		public void UpdateRole(Role role)
		{
			_context.Roles.Update(role);
			_context.SaveChanges();
		}

		public void DeleteRole(Role role)
		{
			role.IsDelete = true;
			//_context.Roles.Remove(role);
			_context.Roles.Update(role);
			_context.SaveChanges();
		}

		public List<Permission> GetAllPermissions()
		{
			return _context.Permission.ToList();
		}

		public void AddPermissionsToRole(List<int> permissionIds, int roleId)
		{
			foreach (int permissionId in permissionIds)
			{
				RolePermission rolePermission = new()
				{
					RoleId = roleId,
					PermissionId = permissionId
				};
				_context.RolePermission.Add(rolePermission);
			}
			_context.SaveChanges();
		}

		public List<int> GetPermissionsRole(int roleId)
		{
			return _context.RolePermission
				.Where(rp => rp.RoleId == roleId)
				.Select(rp => rp.PermissionId)
				.ToList();
		}

		public void UpdatePermissionRoles(List<int> permissionIds, int roleId)
		{
			_context.RolePermission
				.Where(rp => rp.RoleId == roleId)
				.ToList()
				.ForEach(rp => _context.RolePermission.Remove(rp));
			AddPermissionsToRole(permissionIds, roleId); //Insert after removing
		}

		public bool CheckPermission(int permissionId, string userName)
		{
			int userId = _context.Users.Single(u => u.UserName == userName).UserId;
			List<int> UserRoles = _context.UserRoles
				.Where(ur => ur.UserId == userId)
				.Select(ur => ur.RoleId)
				.ToList();
			if (!UserRoles.Any())
				return false;

			List<int> RolePermissions = _context.RolePermission
				.Where(p => p.PermissionId == permissionId)
				.Select(p => p.RoleId)
				.ToList();

			return RolePermissions.Any(rp => UserRoles.Contains(rp));
		}

		public List<int> GetUserRoles(int userId)
		{
			return _context.UserRoles
				.Where(ur => ur.UserId == userId)
				.Select(ur => ur.RoleId)
				.ToList();
		}
	}

}
