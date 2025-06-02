
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPLearn.DataLayer.Entities.Permissions
{
	public class Permission
	{
		[Key]
		public int PermissionId { get; set; }
		[Display(Name = "عنوان دسترسی")]
		[Required(ErrorMessage = "لطفا مقدار {0} را وارد نمایید")]
		[MaxLength(200, ErrorMessage = "حداکثر طول  {0}، {1} کاراکتر می باشد")]
		public required string PermissionName { get; set; }
		public int? ParentId { get; set; }

		[ForeignKey("ParentId")]
		public List<Permission> Permissions { get; set; }
		public List<RolePermission> RolePermissions { get; set; }

	}
}
