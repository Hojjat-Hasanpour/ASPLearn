using ASPLearn.DataLayer.Entities.Permissions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPLearn.DataLayer.Entities.User
{
	public class Role
	{
		public Role()
		{
			
		}

		[Key]
		public int RoleId { get; set; }
		[Display(Name = "نام نقش")]
		[Required(ErrorMessage = $"لطفا {nameof(RoleTitle)} را وارد نمایید")]
		[MaxLength(200, ErrorMessage = "حداکثر طول {0}، {1} کاراکتر می باشد")]
		public required string RoleTitle { get; set; }

		[Display(Name = "حذف شده")]
		public bool IsDelete { get; set; } = false;

		#region Relations
		public virtual List<UserRole>? UserRoles { get; set; }
		public virtual List<RolePermission>? RolePermissions { get; set; }
		#endregion
	}
}
