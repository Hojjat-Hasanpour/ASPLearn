using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPLearn.DataLayer.Entities.Order
{
	public class OrderDetail
	{
		[Key]
		public int OrderDetailId { get; set; }
		[Required]
		public int OrderId { get; set; }
		[Required]
		public int CourseId { get; set; }
		[Required]
		public int Count { get; set; } = 1;
		[Required]
		public int Price { get; set; }

		[ForeignKey(nameof(OrderId))]
		public virtual Order Order { get; set; }
		[ForeignKey(nameof(CourseId))]
		public virtual Course.Course Course { get; set; }
	}
}
