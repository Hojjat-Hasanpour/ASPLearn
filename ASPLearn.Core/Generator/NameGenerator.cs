﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPLearn.Core.Generator
{
	public class NameGenerator
	{
		public static string GenerateUniqCode()
		{
			return Guid.NewGuid().ToString().Replace("-", "");
		}
	}
}
