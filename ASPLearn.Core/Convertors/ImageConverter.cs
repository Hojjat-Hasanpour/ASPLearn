using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.IO;
using System.Threading.Tasks;

namespace ASPLearn.Core.Convertors
{
	public static class ImageConverter
	{
		public static void GenerateThumbnail(IFormFile file, string thumbPath)
		{
			if (file == null || file.Length == 0)
				return;

			using var inputStream = file.OpenReadStream();
			using var image = Image.Load(inputStream);

			// Resize image to thumbnail size
			image.Mutate(x => x.Resize(new ResizeOptions
			{
				Size = new Size(120, 120),
				Mode = ResizeMode.Crop
			}));

			//using var outputStream = new FileStream(thumbPath, FileMode.Create);
			//file.CopyToAsync(outputStream);
			image.SaveAsJpeg(thumbPath);

			//outputStream.Position = 0;
			//return File(outputStream, "image/jpeg");
		}
	}

}
