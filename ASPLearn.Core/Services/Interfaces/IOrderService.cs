using ASPLearn.Core.DTOs.Order;
using ASPLearn.DataLayer.Entities.Order;

namespace ASPLearn.Core.Services.Interfaces
{
	public interface IOrderService
	{
		int AddOrder(string userName, int courseId);
		void UpdateOrderPrice(int orderId);
		void UpdateOrder(Order order);
		Order? GetOrderForUserPanel(string userName, int orderId);
		bool FinallyOrder(string userName, int orderId);
		List<Order> GetUserOrders(string userName);
		Order? GetOrderById(int orderId);
		#region Discount
		DiscountUseType UseDiscount(int orderId, string code);
		void AddDiscount(Discount discount);
		List<Discount> GetAllDiscounts();
		Discount? GetDiscountById(int discountId);
		void UpdateDiscount(Discount discount);

		bool IsExistCode(string code);
		bool IsUserInCourse(string userName, int courseId);

		#endregion
	}
}
