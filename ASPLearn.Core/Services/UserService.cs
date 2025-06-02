using ASPLearn.Core.Conventors;
using ASPLearn.Core.DTOs;
using ASPLearn.Core.Generator;
using ASPLearn.Core.Security;
using ASPLearn.Core.Services.Interfaces;
using ASPLearn.DataLayer.Context;
using ASPLearn.DataLayer.Entities.User;

namespace ASPLearn.Core.Services
{
    public class UserService : IUserService
    {
        private ASPLearnContext _context;
        private IWalletService _wallet_service;
        private IAdminService _admin_service;
        public UserService(ASPLearnContext context, IWalletService walletService, IAdminService adminService)
        {
            _context = context;
            _wallet_service = walletService;
            _admin_service = adminService;
        }

        public bool ActiveAccount(string activeCode)
        {
            var user = _context.Users.SingleOrDefault(u => u.ActiveCode == activeCode);
            if (user == null || user.IsActive)
                return false;
            user.IsActive = true;
            user.ActiveCode = NameGenerator.GenerateUniqCode();
            _context.SaveChanges();
            return true;
        }

        public int AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user.UserId;
        }

        public void ChangeUserPassword(string userName, string password)
        {
            var user = GetUserByUserName(userName);
            user.Password = PasswordHelper.EncodePasswordMd5(password);
            UpdateUser(user);
        }

        public bool CompareOldPassword(string userName, string oldPassword)
        {
            string hashPassword = PasswordHelper.EncodePasswordMd5(oldPassword);
            return _context.Users.Any(u => u.UserName == userName && u.Password == hashPassword);
        }

        public void EditProfile(string userName, EditProfileViewModel profile)
        {
            if (profile.UserAvatar != null)
            {
                string imagePath;
                if (profile!.AvatarName != "UserAvatarPic.png")
                {
                    imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", profile!.AvatarName);
                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }
                }
                profile.AvatarName = NameGenerator.GenerateUniqCode() + Path.GetExtension(profile.UserAvatar.FileName);
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", profile!.AvatarName);
                using var stream = new FileStream(imagePath, FileMode.Create);
                profile.UserAvatar.CopyTo(stream);
            }
            User user = GetUserByUserName(userName);
            user.UserName = profile.UserName;
            user.Email = profile.Email;
            user.UserAvatar = profile.AvatarName;

            UpdateUser(user);
        }

        public EditProfileViewModel GetDataEditProfile(string userName)
        {
            return _context.Users.Where(u => u.UserName == userName).Select(u => new EditProfileViewModel()
            {
                UserName = u.UserName,
                Email = u.Email,
                AvatarName = u.UserAvatar!,
            }).Single();
        }

        public SideBarUserPanelViewModel GetSideBarUserPanelData(string userName)
        {
            return _context.Users.Where(u => u.UserName == userName).Select(u => new SideBarUserPanelViewModel()
            {
                UserName = u.UserName,
                RegisterDate = u.RegisterDate.HasValue ? u.RegisterDate.Value.ToDateTime(TimeOnly.MinValue) : DateTime.MinValue,
                ImageName = u.UserAvatar!,
                IsAdmin = _admin_service.CheckPermission(1, u.UserName)

            }).Single();
        }

        public User GetUserByActiveCode(string activeCode)
        {
            return _context.Users.SingleOrDefault(u => u.Email == activeCode)!;
        }

        public User GetUserByEmail(string email)
        {
            //string fixedEmail = FixedText.FixedEmail(email);
            return _context.Users.SingleOrDefault(u => u.Email == email)!;
        }

        public User GetUserByUserName(string userName)
        {
            return _context.Users.SingleOrDefault(u => u.UserName == userName)!;
        }

        public int GetUserIdByUserName(string userName)
        {
            return _context.Users.Single(u => u.UserName == userName).UserId;
        }

        public InformationUserViewModel GetUserInformation(string userName)
        {
            var user = GetUserByUserName(userName);
            var info = new InformationUserViewModel()
            {
                UserName = user.UserName,
                Email = user.Email,
                RegisterDate = user.RegisterDate.HasValue ? user.RegisterDate.Value.ToDateTime(TimeOnly.MinValue) : DateTime.MinValue,
                Wallet = _wallet_service.BalanceUserWallet(user.UserId)
            };
            return info;
        }

        public bool IsExistEmail(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public bool IsExistUserName(string userName)
        {
            return _context.Users.Any(u => u.UserName == userName);
        }

        public User LoginUser(LoginViewModel login)
        {
            string hashPassword = PasswordHelper.EncodePasswordMd5(login.Password);
            string FixedEmail = FixedText.FixedEmail(login.Email);
            return _context.Users.SingleOrDefault(u => u.Email == FixedEmail && u.Password == hashPassword)!;
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }
    }
}
