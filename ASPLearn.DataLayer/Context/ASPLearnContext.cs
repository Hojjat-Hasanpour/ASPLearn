using ASPLearn.DataLayer.Entities.Course;
using ASPLearn.DataLayer.Entities.Order;
using ASPLearn.DataLayer.Entities.Permissions;
using ASPLearn.DataLayer.Entities.Question;
using ASPLearn.DataLayer.Entities.User;
using ASPLearn.DataLayer.Entities.Wallet;
using Microsoft.EntityFrameworkCore;


namespace ASPLearn.DataLayer.Context
{
	public class ASPLearnContext(DbContextOptions options) : DbContext(options) //Primary Constructor
	{
		public DbSet<Role> Roles { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<UserRole> UserRoles { get; set; }
		public DbSet<UserDiscountCode> UserDiscountCodes { get; set; }

		public DbSet<Wallet> Wallets { get; set; }
		public DbSet<WalletType> WalletTypes { get; set; }

		public DbSet<Permission> Permission { get; set; }
		public DbSet<RolePermission> RolePermission { get; set; }

		public DbSet<CourseGroup> CourseGroups { get; set; }
		public DbSet<CourseLevel> CourseLevels { get; set; }
		public DbSet<CourseStatus> CourseStatuses { get; set; }
		public DbSet<Course> Courses { get; set; }
		public DbSet<CourseEpisode> CourseEpisodes { get; set; }
		public DbSet<UserCourse> UserCourses { get; set; }
		public DbSet<CourseComment> CourseComments { get; set; }
		public DbSet<CourseVote> CourseVotes { get; set; }

		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderDetail> OrderDetails { get; set; }
		public DbSet<Discount> Discounts { get; set; }

		public DbSet<Question> Questions { get; set; }
		public DbSet<Answer> Answers { get; set; }


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>().HasQueryFilter(user => !user.IsDelete); // Filter for soft delete
			modelBuilder.Entity<Role>().HasQueryFilter(role => !role.IsDelete);
			modelBuilder.Entity<CourseGroup>().HasQueryFilter(group => !group.IsDelete);

			modelBuilder.Entity<Course>()
				.HasOne(c => c.CourseGroup)
				.WithMany(g => g.Courses)
				.HasForeignKey(f => f.GroupId);

			modelBuilder.Entity<Course>()
				.HasOne(c => c.Group)
				.WithMany(g => g.SubGroups)
				.HasForeignKey(f => f.SubGroup);

			modelBuilder.Entity<Order>()
				.HasMany(o => o.OrderDetails)
				.WithOne(order => order.Order).HasForeignKey(o => o.OrderId).OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<OrderDetail>()
				.HasOne(o => o.Order)
				.WithMany(o => o.OrderDetails).HasForeignKey(o => o.OrderId).OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<UserCourse>()
				.HasOne(u => u.Course)
				.WithMany(uc => uc.UserCourses).HasForeignKey(o => o.CourseId).OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<UserCourse>()
				.HasOne(uc => uc.User)
				.WithMany(uc => uc.UserCourses).HasForeignKey(uc => uc.UserId).OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<CourseComment>()
				.HasOne(cc => cc.User)
				.WithMany(cc => cc.CourseComments).HasForeignKey(cc => cc.UserId).OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<CourseComment>()
				.HasOne(cc => cc.Course)
				.WithMany(cc => cc.CourseComments).HasForeignKey(cc => cc.CourseId).OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<CourseVote>()
				.HasOne(cv => cv.User)
				.WithMany(cc => cc.CourseVotes).HasForeignKey(cc => cc.UserId).OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<CourseVote>()
				.HasOne(cv => cv.Course)
				.WithMany(cv => cv.CourseVotes).HasForeignKey(cc => cc.CourseId).OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Question>()
				.HasOne(q => q.User)
				.WithMany(u => u.Questions)
				.HasForeignKey(q => q.UserId).OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Answer>()
				.HasOne(a => a.User)
				.WithMany(u => u.Answers)
				.HasForeignKey(a => a.UserId).OnDelete(DeleteBehavior.Restrict);

			base.OnModelCreating(modelBuilder);
		}
	}
}
