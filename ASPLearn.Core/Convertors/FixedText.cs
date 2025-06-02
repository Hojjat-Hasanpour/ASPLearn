using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPLearn.Core.Conventors
{
	public class FixedText
	{
		public static string FixedEmail(string email)
		{
			return email.Trim().ToLower();
		}
	}
}
