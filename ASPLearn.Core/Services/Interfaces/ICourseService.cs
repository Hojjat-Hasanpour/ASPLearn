using ASPLearn.Core.DTOs.Course;
using ASPLearn.DataLayer.Entities.Course;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASPLearn.Core.Services.Interfaces
{
    public interface ICourseService
    {
        #region CourseGroup
        List<CourseGroup> GetAllGroups();
        List<SelectListItem> GetGroupForManageCourse();
        List<SelectListItem> GetSubGroupForManageCourse(int groupId);
        List<SelectListItem> GetTeachers();
        List<SelectListItem> GetLevels();
        List<SelectListItem> GetStatuses();
        void AddGroup(CourseGroup group);
        void UpdateGroup(CourseGroup group);
        CourseGroup? GetGroupById(int groupId);
        #endregion

        #region Course
        List<ShowCourseForAdminViewModel> GetCoursesForAdmin();
        int AddCourse(Course course, IFormFile imgCourse, IFormFile courseDemo);
        Course GetCourseById(int courseId);
        void UpdateCourse(Course course, IFormFile? imgCourse, IFormFile? courseDemo);

        (List<ShowCourseListItemViewModel> Model, int PageCount) GetCourse(int pageId = 1, string filter = "",
            string getType = "all", string orderByType = "date", int startPrice = 0, int endPrice = 0,
            List<int> selectedGroups = null!, int take = 8);

        Course? GetCourseForShow(int courseId);
        List<Course> GetUserCourses(int userId);
        List<ShowCourseListItemViewModel> GetPopularCourses();
        bool IsFree(int courseId);
        #endregion

        #region Episode
        bool CheckExistFile(string fileName);
        int AddEpisode(CourseEpisode episode, IFormFile file);
        List<CourseEpisode> GetListEpisodeCourse(int courseId);
        CourseEpisode GetEpisodeById(int episodeId);
        void EditEpisode(CourseEpisode episode, IFormFile file);
        #endregion

        #region Comments
        void AddComment(CourseComment comment);
        (List<CourseComment> ListComments, int PageId) GetCourseComments(int courseId, int pageId = 1); //tuple

        #endregion

        #region CourseVote

        void AddVote(int userId, int courseId, bool vote);
        (int LikeVotes, int DislikeVotes) GetCourseVotes(int courseId); //Tuple<int,int>

        #endregion
    }
}
