using ASPLearn.Core.DTOs.Question;
using ASPLearn.DataLayer.Entities.Question;

namespace ASPLearn.Core.Services.Interfaces
{
    public interface IForumService
    {
        #region Question

        int AddQuestion(Question question); // Explain: 
        ShowQuestionViewModel ShowQuestion(int questionId);
        IEnumerable<Question> GetQuestions(int? courseId, string filter = "");
        #endregion

        #region Answer

        void AddAnswer(Answer answer);
        void ChangeIsTrueAnswer(int questionId, int answerId);

        #endregion
    }
}
