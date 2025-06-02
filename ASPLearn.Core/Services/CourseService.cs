using ASPLearn.Core.Convertors;
using ASPLearn.Core.DTOs.Course;
using ASPLearn.Core.Generator;
using ASPLearn.Core.Security;
using ASPLearn.Core.Services.Interfaces;
using ASPLearn.DataLayer.Context;
using ASPLearn.DataLayer.Entities.Course;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ASPLearn.Core.Services
{
	public class CourseService(ASPLearnContext context) : ICourseService
	{
		private readonly ASPLearnContext _context = context;

		public int AddCourse(Course course, IFormFile imgCourse, IFormFile courseDemo)
		{
			course.CreateDate = DateTime.Now;
			course.CourseImageName = "No_Image.jpg";
			//ToDo: Check image
			if (imgCourse != null && imgCourse.IsImage())
			{
				course.CourseImageName = NameGenerator.GenerateUniqCode() + Path.GetExtension(imgCourse.FileName);
				// Save the image to the server
				var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/course/image", course.CourseImageName);
				using var stream = new FileStream(path, FileMode.Create);
				imgCourse.CopyToAsync(stream);

				var thumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/course/thumb", course.CourseImageName);
				ImageConverter.GenerateThumbnail(imgCourse, thumbPath);

			}

			if (courseDemo is not null)
			{
				course.DemoFileName = NameGenerator.GenerateUniqCode() + Path.GetExtension(courseDemo.FileName);
				// Save the image to the server
				string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/course/demoes", course.DemoFileName);
				using var stream = new FileStream(path, FileMode.Create);
				courseDemo.CopyTo(stream);
			}
			_context.Courses.Add(course);
			_context.SaveChanges();
			return course.CourseId;
		}

		public int AddEpisode(CourseEpisode episode, IFormFile file)
		{
			episode.EpisodeFileName = file.FileName;

			// Save the image to the server
			var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/courseFiles", episode.EpisodeFileName);
			using var stream = new FileStream(path, FileMode.Create);
			file.CopyToAsync(stream);

			_context.CourseEpisodes.Add(episode);
			_context.SaveChanges();
			return episode.EpisodeId;
		}

		public List<ShowCourseListItemViewModel> GetPopularCourses()
		{
			var courses = _context.Courses.Include(c => c.OrderDetails)
				.Include(c => c.Episodes)
				.Where(c => c.OrderDetails.Any())
				.OrderByDescending(od => od.OrderDetails.Count)
				.Take(8)
				.ToList();

			var modelList = courses.Select(c => new ShowCourseListItemViewModel()
			{
				CourseId = c.CourseId,
				CourseTitle = c.CourseTitle,
				ImageName = c.CourseImageName,
				TotalTime = new TimeSpan(c.Episodes.Sum(e => e.EpisodeTime.Ticks))
			}).ToList();
			return modelList;
		}

		public bool IsFree(int courseId)
		{
			return _context.Courses.Find(courseId)?.CoursePrice == 0;
		}

		public bool CheckExistFile(string fileName)
		{
			var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/courseFiles", fileName);
			return File.Exists(path);
		}

		public void EditEpisode(CourseEpisode episode, IFormFile file)
		{
			if (file != null)
			{
				string deleteFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/courseFiles", episode.EpisodeFileName);
				File.Delete(deleteFilePath);

				episode.EpisodeFileName = file.FileName;
				string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/courseFiles", episode.EpisodeFileName);
				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					file.CopyToAsync(stream);
				}
			}

			_context.CourseEpisodes.Update(episode);
			_context.SaveChanges();
		}

		public void AddComment(CourseComment comment)
		{
			_context.CourseComments.Add(comment);
			_context.SaveChanges();
		}

		public (List<CourseComment> ListComments, int PageId) GetCourseComments(int courseId, int pageId = 1)
		{
			const int take = 7;
			int skip = (pageId - 1) * take;
			int pageCount = _context.CourseComments.Count(c => !c.IsDelete && c.CourseId == courseId) / take;

			if (pageCount % 2 != 0)
			{
				pageCount++;
			}

			return (_context.CourseComments.Include(c => c.User)
				.Where(c => !c.IsDelete && c.CourseId == courseId).Skip(skip).Take(take)
				.OrderByDescending(c => c.CreateDate).ToList(),
				pageCount);
		}

		public void AddVote(int userId, int courseId, bool vote)
		{
			var userVote = _context.CourseVotes.FirstOrDefault(cv => cv.UserId == userId && cv.CourseId == courseId);
			if (userVote is not null)
			{
				userVote.Vote = vote;
			}
			else
			{
				userVote = new CourseVote()
				{
					CourseId = courseId,
					Vote = vote,
					UserId = userId
				};
				_context.Add(userVote);
			}

			_context.SaveChanges();
		}

		public (int LikeVotes, int DislikeVotes) GetCourseVotes(int courseId)
		{
			var votes = _context.CourseVotes.Where(cv => cv.CourseId == courseId).Select(cv => cv.Vote).ToList();
			return (LikeVotes: votes.Count(v => v), DislikeVotes: votes.Count(v => !v));
		}

		public List<CourseGroup> GetAllGroups() => _context.CourseGroups.Include(c => c.CourseGroups).ToList();

		public (List<ShowCourseListItemViewModel> Model, int PageCount) GetCourse(int pageId = 1, string filter = "",
			string getType = "all", string orderByType = "date", int startPrice = 0, int endPrice = 0,
			List<int> selectedGroups = null!, int take = 8)
		{
			IQueryable<Course> result = _context.Courses;
			if (!string.IsNullOrEmpty(filter))
			{
				result = result.Where(c => c.CourseTitle.Contains(filter) || c.Tags.Contains(filter));
			}
			result = getType switch
			{
				//"all" => _,
				"buyable" => result.Where(c => c.IsActive && c.CoursePrice != 0),
				"free" => result.Where(c => c.IsActive && c.CoursePrice == 0),
				_ => result
			};
			result = orderByType switch
			{
				"date" => result.OrderByDescending(c => c.CreateDate),
				"updateDate" => result.OrderByDescending(c => c.UpdateDate),
				_ => result
			};

			if (startPrice > 0)
			{
				result = result.Where(c => c.CoursePrice > startPrice);
			}
			if (endPrice > 0)
			{
				result = result.Where(c => c.CoursePrice < endPrice);
			}
			if (selectedGroups != null && selectedGroups.Count != 0)
			{
				foreach (int groupId in selectedGroups)
				{
					result = result.Where(c => c.GroupId == groupId || c.SubGroup == groupId);
				}
			}
			int skip = take * (pageId - 1);

			int pageCount = result
				//.AsEnumerable()
				.Select(c => new ShowCourseListItemViewModel()
				{
					CourseId = c.CourseId,
					//CourseTitle = c.CourseTitle,
					//ImageName = c.CourseImageName,
					//Price = Convert.ToInt32(c.CoursePrice), //convert to int
					//TotalTime = new TimeSpan(c.Episodes.Sum(e => e.EpisodeTime.Ticks))
				}).Count() / take;

			var query = result.Include(c => c.Episodes).AsEnumerable()
				.Select(c => new ShowCourseListItemViewModel()
				{
					CourseId = c.CourseId,
					ImageName = c.CourseImageName,
					Price = c.CoursePrice,
					CourseTitle = c.CourseTitle,
					TotalTime = new TimeSpan(c.Episodes.Sum(e => e.EpisodeTime.Ticks))
				}).Skip(skip).Take(take).ToList();

			return (query, pageCount);
		}

		public Course GetCourseById(int courseId)
		{
			return _context.Courses.Find(courseId)!;
		}

		public Course? GetCourseForShow(int courseId)
		{
			return _context.Courses.Include(c => c.Episodes)
				.Include(c => c.CourseStatus)
				.Include(c => c.CourseLevel)
				.Include(c => c.User)
				.Include(c => c.UserCourses)
				.FirstOrDefault(c => c.CourseId == courseId);
		}

		public void UpdateGroup(CourseGroup group)
		{
			_context.CourseGroups.Update(group);
			_context.SaveChanges();
		}

		public CourseGroup? GetGroupById(int groupId)
		{
			return _context.CourseGroups.Find(groupId);
		}

		public List<ShowCourseForAdminViewModel> GetCoursesForAdmin()
		{
			return _context.Courses
				.Select(c => new ShowCourseForAdminViewModel()
				{
					CourseId = c.CourseId,
					CourseTitle = c.CourseTitle,
					ImageName = c.CourseImageName,
					EpisodeCount = c.Episodes.Count
				}).ToList();
		}

		public CourseEpisode GetEpisodeById(int episodeId)
		{
			return _context.CourseEpisodes.Find(episodeId)!;
		}

		public List<SelectListItem> GetGroupForManageCourse()
		{
			return _context.CourseGroups
				.Where(g => g.ParentId == null)
				.Select(g => new SelectListItem()
				{
					Text = g.GroupName,
					Value = g.GroupId.ToString()
				}).ToList();
		}

		public List<SelectListItem> GetLevels()
		{
			return _context.CourseLevels.Select(l => new SelectListItem()
			{
				Text = l.LevelName,
				Value = l.LevelId.ToString()
			}).ToList();
		}

		public List<CourseEpisode> GetListEpisodeCourse(int courseId)
		{
			return _context.CourseEpisodes
				.Where(e => e.CourseId == courseId)
				.ToList();
		}

		public List<SelectListItem> GetStatuses()
		{
			return _context.CourseStatuses.Select(s => new SelectListItem()
			{
				Text = s.StatusName,
				Value = s.StatusId.ToString()
			}).ToList();
		}

		public void AddGroup(CourseGroup group)
		{
			_context.CourseGroups.Add(group);
			_context.SaveChanges();
		}

		public List<SelectListItem> GetSubGroupForManageCourse(int groupId)
		{
			return _context.CourseGroups
				.Where(g => g.ParentId == groupId)
				.Select(g => new SelectListItem()
				{
					Text = g.GroupName,
					Value = g.GroupId.ToString()
				}).ToList();
		}

		public List<SelectListItem> GetTeachers()
		{
			return _context.UserRoles.Where(r => r.RoleId == 2).Include(r => r.User)
				.Select(u => new SelectListItem()
				{
					Text = u.User!.UserName,
					Value = u.UserId.ToString()
				}).ToList();
		}

		public List<Course> GetUserCourses(int userId)
		{
			return _context.UserCourses.Include(uc => uc.Course).Where(uc => uc.UserId == userId).Select(c => c.Course).ToList();
		}

		public void UpdateCourse(Course course, IFormFile imgCourse, IFormFile courseDemo)
		{
			course.UpdateDate = DateTime.Now;

			if (imgCourse != null && imgCourse.IsImage())
			{
				if (course.CourseImageName != "No_Image.jpg")
				{
					string oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/course/image", course.CourseImageName);
					if (File.Exists(oldPath))
					{
						File.Delete(oldPath);
					}
					string oldThumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/course/thumb", course.CourseImageName);
					if (File.Exists(oldThumbPath))
					{
						File.Delete(oldThumbPath);
					}
				}
				course.CourseImageName = NameGenerator.GenerateUniqCode() + Path.GetExtension(imgCourse.FileName);
				// Save the image to the server
				var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/course/image", course.CourseImageName);
				using var stream = new FileStream(path, FileMode.Create);
				imgCourse.CopyToAsync(stream);

				var thumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/course/thumb", course.CourseImageName);
				ImageConverter.GenerateThumbnail(imgCourse, thumbPath);

			}

			if (courseDemo is not null)
			{
				if (course.DemoFileName != null)
				{
					string oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/course/demoes", course.DemoFileName);
					if (File.Exists(oldPath))
					{
						File.Delete(oldPath);
					}
				}
				else
				{
					course.DemoFileName = "No_Image.jpg";
				}
				course.DemoFileName = NameGenerator.GenerateUniqCode() + Path.GetExtension(courseDemo.FileName);
				// Save the image to the server
				string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/course/demoes", course.DemoFileName);
				using var stream = new FileStream(path, FileMode.Create);
				courseDemo.CopyTo(stream);
			}
			_context.Courses.Update(course);
			_context.SaveChanges();

		}
	}

}
