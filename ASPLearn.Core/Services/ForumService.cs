using ASPLearn.Core.DTOs.Question;
using ASPLearn.Core.Services.Interfaces;
using ASPLearn.DataLayer.Context;
using ASPLearn.DataLayer.Entities.Question;
using Microsoft.EntityFrameworkCore;

namespace ASPLearn.Core.Services
{
    public class ForumService(ASPLearnContext context) : IForumService
    {
        private readonly ASPLearnContext _context = context;
        public int AddQuestion(Question question)
        {
            _context.Add(question);
            _context.SaveChanges();
            return question.QuestionId;
        }

        public ShowQuestionViewModel ShowQuestion(int questionId)
        {
            var question = new ShowQuestionViewModel()
            {
                Question = _context.Questions.Include(q => q.User).Include(q => q.Course).First(q => q.QuestionId == questionId),
                Answers = _context.Answers.Where(a => a.QuestionId == questionId).Include(a => a.User).ToList()
            };
            return question;
        }

        public IEnumerable<Question> GetQuestions(int? courseId, string filter = "")
        {
            IQueryable<Question> result = _context.Questions.Where(q => EF.Functions.Like(q.Title, $"%{filter}%"));
            if (courseId != null)
                result = result.Where(q => q.CourseId == courseId);
            // Paging can be added (take,skip)
            return result.Include(q => q.Course).Include(q => q.User).ToList();
        }

        public void AddAnswer(Answer answer)
        {
            _context.Add(answer);
            _context.SaveChanges();
        }

        public void ChangeIsTrueAnswer(int questionId, int answerId)
        {
            var answers = _context.Answers.Where(a => a.QuestionId == questionId);
            foreach (var answer in answers)
            {
                answer.IsTrue = answer.AnswerId == answerId; // ? true : false;
            }
            _context.UpdateRange(answers);
            _context.SaveChanges();
        }
    }
}
