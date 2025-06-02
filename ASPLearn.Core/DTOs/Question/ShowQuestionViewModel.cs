using ASPLearn.DataLayer.Entities.Question;

namespace ASPLearn.Core.DTOs.Question
{
	public class ShowQuestionViewModel
	{
		public DataLayer.Entities.Question.Question Question { get; set; }
		public List<Answer> Answers { get; set; }
	}
}
