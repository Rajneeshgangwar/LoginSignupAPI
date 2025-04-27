using LoginSignupAPI.Model.Entities;

namespace LoginSignupAPI.Model
{
    public class QuestionListResponse
    {
        public int TotalRecords { get; set; }
        public List<QuestionResponse> Data { get; set; }
    }
    public class QuestionResponse
    {
        public Guid Id { get; set; }
        public string QuestionName { get; set; }
        public List<Option> Option { get; set; }
    }
}
