using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ASPLearn.Core.DTOs
{
    public class InformationUserViewModel
    {
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public DateTime RegisterDate { get; set; }
        public int Wallet { get; set; }
    }

    public class SideBarUserPanelViewModel
    {
        public required string UserName { get; set; }
        public DateTime RegisterDate { get; set; }
        public required string ImageName { get; set; }
        public required bool IsAdmin { get; set; } = false;
    }

    public class EditProfileViewModel
    {
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "حداکثر طول {0}، {1} کاراکتر می باشد")]
        public required string UserName { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "حداکثر طول {0}، {1} کاراکتر می باشد")]
        [EmailAddress(ErrorMessage = "لطفا یک ایمیل معتبر وارد کنید")]
        public required string Email { get; set; }

        public IFormFile? UserAvatar { get; set; }
        public required string AvatarName { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Display(Name = "کلمه عبور فعلی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "حداکثر طول {0}، {1} کاراکتر می باشد")]
        public required string OldPassword { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "حداکثر طول {0}، {1} کاراکتر می باشد")]
        public required string Password { get; set; }

        [Display(Name = "تکرار کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "حداکثر طول {0}، {1} کاراکتر می باشد")]
        [Compare("Password", ErrorMessage = "کلمه عبور و تکرار آن برابر نیست")]
        public required string ConfirmPassword { get; set; }
    }
}
