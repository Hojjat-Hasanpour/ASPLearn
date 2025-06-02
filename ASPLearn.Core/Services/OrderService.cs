using ASPLearn.Core.DTOs.Order;
using ASPLearn.Core.Services.Interfaces;
using ASPLearn.DataLayer.Context;
using ASPLearn.DataLayer.Entities.Course;
using ASPLearn.DataLayer.Entities.Order;
using ASPLearn.DataLayer.Entities.User;
using ASPLearn.DataLayer.Entities.Wallet;
using Microsoft.EntityFrameworkCore;

namespace ASPLearn.Core.Services
{
	public class OrderService(ASPLearnContext context, IWalletService walletService) : IOrderService
	{
		private readonly ASPLearnContext _context = context;
		//private readonly IUserService _userService = userService;
		private readonly IWalletService _walletService = walletService;
		public int AddOrder(string userName, int courseId)
		{
			int userId = GetUserIdByUserName(userName);
			Order? order = _context.Orders.FirstOrDefault(o => o.UserId == userId && o.IsFinal == false);

			var course = _context.Courses.Find(courseId);
			if (order is null)
			{
				order = new Order()
				{
					UserId = userId,
					IsFinal = false,
					CreateDate = DateTime.Now,
					OrderSum = course!.CoursePrice,
					OrderDetails = new List<OrderDetail>()
					{
						new OrderDetail(){
							CourseId = courseId,
							// OrderId = order.OrderId, Does not necessary
							Count = 1,
							Price = course.CoursePrice
						}
					}
				};
				_context.Orders.Add(order);
			}
			else
			{
				OrderDetail? orderDetail = _context.OrderDetails.FirstOrDefault(d => d.OrderId == order.OrderId && d.CourseId == courseId);
				if (orderDetail != null)
				{
					// orderDetail.Count++; This is unused beacause user can not add same course to order
					//_context.OrderDetails.Update(orderDetail);
				}
				else
				{
					orderDetail = new OrderDetail()
					{
						OrderId = order.OrderId,
						CourseId = courseId,
						Count = 1,
						Price = course!.CoursePrice
					};
					_context.OrderDetails.Add(orderDetail);
				}
				UpdateOrderPrice(order.OrderId);
			}
			_context.SaveChanges();
			return order.OrderId;
		}

		public bool FinallyOrder(string userName, int orderId)
		{
			int userId = GetUserIdByUserName(userName);
			var order = _context.Orders.Include(o => o.OrderDetails).ThenInclude(od => od.Course)
				.FirstOrDefault(o => o.UserId == userId && o.OrderId == orderId);

			if (order == null || order.IsFinal)
				return false;

			if (_walletService.BalanceUserWallet(userId) < order.OrderSum) return false;
			order.IsFinal = true;
			_walletService.AddWallet(new Wallet()
			{
				Amount = order.OrderSum,
				UserId = userId,
				WalletTypeId = 2,
				CreatedAt = DateTime.Now,
				IsPay = true,
				Description = "فاکتور شماره #" + order.OrderId,
			});

			_context.Orders.Update(order);

			foreach (var detail in order.OrderDetails)
			{
				_context.UserCourses.Add(new UserCourse
				{
					CourseId = detail.CourseId,
					UserId = userId
				});
			}

			_context.SaveChanges();
			return true;
		}

		public Order? GetOrderForUserPanel(string userName, int orderId)
		{
			int userId = GetUserIdByUserName(userName);
			return _context.Orders.Include(o => o.OrderDetails).ThenInclude(od => od.Course)
				.FirstOrDefault(o => o.UserId == userId && o.OrderId == orderId);
		}

		private int GetUserIdByUserName(string userName)
		{
			return _context.Users.First(user => user.UserName == userName).UserId;
		}

		public void UpdateOrderPrice(int orderId)
		{
			var order = _context.Orders.Find(orderId);
			if (order is null) return;
			order.OrderSum = _context.OrderDetails.Where(d => d.OrderId == order.OrderId).Sum(d => d.Price);
			_context.Orders.Update(order);
			//_context.SaveChanges();
		}

		public List<Order> GetUserOrders(string userName)
		{
			int userId = GetUserIdByUserName(userName);
			return _context.Orders.Where(o => o.UserId == userId).ToList();
		}

		public DiscountUseType UseDiscount(int orderId, string code)
		{
			var discount = _context.Discounts.SingleOrDefault(d => d.DiscountCode == code);
			if (discount != null)
				return DiscountUseType.NotFound;
			if (discount!.StartDate != null && discount.StartDate < DateTime.Now)
				return DiscountUseType.ExpiredDate;
			if (discount!.EndDate != null && discount.EndDate >= DateTime.Now)
				return DiscountUseType.ExpiredDate;
			if (discount.UsableCount is < 1)
				return DiscountUseType.Finished;

			var order = GetOrderById(orderId);

			if (order == null)
				return DiscountUseType.NotFound;

			if (_context.UserDiscountCodes.Any(ud => ud.DiscountId == discount.DiscountId && ud.UserId == order.UserId))
				return DiscountUseType.UserUsed;

			order.OrderSum -= (order.OrderSum * discount.DiscountPercent) / 100; //Sum = Sum - DiscountAmount
			UpdateOrder(order);

			if (discount.UsableCount != null)
				--discount.UsableCount; //Decrement

			_context.Discounts.Update(discount);
			_context.UserDiscountCodes.Add(new UserDiscountCode()
			{
				UserId = order.UserId,
				DiscountId = discount.DiscountId,
			});
			_context.SaveChanges();

			return DiscountUseType.Success;
		}

		public Order? GetOrderById(int orderId)
		{
			return _context.Orders.Find(orderId);
		}

		public void UpdateOrder(Order order)
		{
			_context.Orders.Update(order);
			_context.SaveChanges();
		}

		public void AddDiscount(Discount discount)
		{
			_context.Discounts.Add(discount);
			_context.SaveChanges();
		}

		public List<Discount> GetAllDiscounts()
		{
			return _context.Discounts.ToList();
		}

		public Discount? GetDiscountById(int discountId)
		{
			return _context.Discounts.Find(discountId);
		}

		public void UpdateDiscount(Discount discount)
		{
			_context.Discounts.Update(discount);
			_context.SaveChanges();
		}

		public bool IsExistCode(string code)
		{
			return _context.Discounts.Any(d => d.DiscountCode == code);
		}

		public bool IsUserInCourse(string userName, int courseId)
		{
			var userId = GetUserIdByUserName(userName);
			return _context.UserCourses.Any(
				userCourse => userCourse.UserId == userId && userCourse.CourseId == courseId);
		}
	}
}
