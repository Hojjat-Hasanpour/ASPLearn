using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPLearn.DataLayer.Entities.Course
{
	public class CourseEpisode
	{
		[Key]
		public int EpisodeId { get; set; }

		public int CourseId { get; set; }

		[Display(Name = "نام قسمت")]
		[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
		[MaxLength(200, ErrorMessage = "حداکثر {0}،{1} کاراکتر مجاز است")]
		public required string EpisodeTitle { get; set; }

		[Display (Name ="زمان")]
		[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
		public TimeSpan EpisodeTime { get; set; }

		[Display(Name = "فایل")]
		public string EpisodeFileName { get; set; }

		[Display(Name = "رایگان")]
		public bool IsFree { get; set; }

		[ForeignKey(nameof(CourseId))]
		public virtual Course? Course { get; set; }
	}
}
