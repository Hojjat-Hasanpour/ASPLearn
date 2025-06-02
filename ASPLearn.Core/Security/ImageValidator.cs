using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;

namespace ASPLearn.Core.Security
{
	public static class ImageValidator
	{
		public static bool IsImage(this IFormFile file)
		{
			if (file == null || file.Length == 0)
				return false;
			//var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
			//var fileExtension = Path.GetExtension(file.FileName).ToLower();
			//return allowedExtensions.Contains(fileExtension); // Does not Safe
			try
			{
				using var inputStream = file.OpenReadStream();
				using var image = Image.Load(inputStream);
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
