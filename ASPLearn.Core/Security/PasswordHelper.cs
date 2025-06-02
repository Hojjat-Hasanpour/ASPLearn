using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ASPLearn.Core.Security
{
	public class PasswordHelper
	{
		public static string EncodePasswordMd5(string pass)
		{
			MD5 md5 = MD5.Create();
			byte[] inputBytes = Encoding.ASCII.GetBytes(pass);
			byte[] hash = md5.ComputeHash(inputBytes);
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < hash.Length; i++)
			{
				sb.Append(hash[i].ToString("X2"));
			}
			return sb.ToString();
		}
		public static bool ComparePassword(string dbPassword, string password)
		{
			if (dbPassword == EncodePasswordMd5(password))
			{
				return true;
			}
			return false;
		}
	}
}
