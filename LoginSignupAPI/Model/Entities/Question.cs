namespace LoginSignupAPI.Model.Entities
{
    public class Question
    {
        public Guid Id { get; set; }
        public string QuestionName { get; set; }
        public List<Option> Option { get; set; }
    }
}
