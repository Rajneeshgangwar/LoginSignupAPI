namespace LoginSignupAPI.Model
{
    public class QuestionListBody
    {
        public int PageIndex { get; set; } = 1;
        public int ItemCount { get; set; } = 10;
        public string SortBy { get; set; } = "questionName";
        public string SortDirection { get; set; } = "asc"; // asc or desc
        public string Search { get; set; } = "";
        public List<Filter> Filter { get; set; }
    }
    public class Filter
    {
        public string Oid { get; set; }
        public string Value { get; set; }
    }
}
