﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPLearn.DataLayer.Entities.User
{
	public class UserRole
	{
		public UserRole()
		{

		}
		[Key]
		public int UR_Id { get; set; }
		public int UserId { get; set; }
		public int RoleId { get; set; }

		#region Relations
		public virtual User? User { get; set; }
		public virtual Role? Role { get; set; }
		#endregion
	}
}
