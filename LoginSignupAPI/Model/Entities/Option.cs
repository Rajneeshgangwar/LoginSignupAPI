using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Text.Json.Serialization;

namespace LoginSignupAPI.Model.Entities
{
    public class Option
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsCorrect { get; set; }

        public Guid QuestionId { get; set; }
        [JsonIgnore]
        [ValidateNever]
        public Question Question { get; set; }
    }
}
