using Microsoft.AspNetCore.Mvc;
using LoginSignupAPI.Model;
using Microsoft.EntityFrameworkCore;
using System;
using LoginSignupAPI.Model.Data;
using LoginSignupAPI.Model.Entities;
using System.Linq.Dynamic.Core;

namespace LoginSignupAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public QuestionsController(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        [HttpPost("bulk-save")]
        public async Task<IActionResult> BulkSaveQuestions([FromBody] QuestionDto wrapper)
        {
            foreach (var question in wrapper.Question)
            {
                // Check if question already exists
                var existingQuestion = await _dbContext.Questions
                    .Include(q => q.Option)
                    .FirstOrDefaultAsync(q => q.Id == question.Id);

                if (existingQuestion == null)
                {
                    // New question, add it
                    _dbContext.Questions.Add(question);
                }
                else
                {
                    // Update existing question
                    existingQuestion.QuestionName = question.QuestionName;

                    // Remove old options
                    _dbContext.Options.RemoveRange(existingQuestion.Option);

                    // Add new options
                    foreach (var option in question.Option)
                    {
                        option.QuestionId = existingQuestion.Id;
                    }

                    existingQuestion.Option = question.Option;
                }
            }

            await _dbContext.SaveChangesAsync();

            return Ok("Questions saved/updated successfully!");
        }
        [HttpPost("list")]
        public async Task<IActionResult> ListQuestions([FromBody] QuestionListBody request)
        {
            var query = _dbContext.Questions
                .Include(q => q.Option)
                .AsQueryable();

            // Search
            if (!string.IsNullOrEmpty(request.Search))
            {
                query = query.Where(q => q.QuestionName.Contains(request.Search));
            }

            // Filters (optional)
            if (request.Filter != null && request.Filter.Any())
            {
                foreach (var filter in request.Filter)
                {
                    if (!string.IsNullOrEmpty(filter.Oid) && !string.IsNullOrEmpty(filter.Value))
                    {
                        // You can add custom filters based on oid
                        if (filter.Oid.ToLower() == "questionname")
                        {
                            query = query.Where(q => q.QuestionName.Contains(filter.Value));
                        }
                        // You can extend this if you add more fields
                    }
                }
            }

            // Total before pagination
            var totalRecords = await query.CountAsync();

            // Sorting (dynamic)
            if (!string.IsNullOrEmpty(request.SortBy))
            {
                var sortingOrder = $"{request.SortBy} {request.SortDirection}";
                query = query.OrderBy(sortingOrder); // using System.Linq.Dynamic.Core
            }

            // Paging
            var skip = (request.PageIndex - 1) * request.ItemCount;
            var data = await query
                .Skip(skip)
                .Take(request.ItemCount)
                .Select(q => new QuestionResponse
                {
                    Id = q.Id,
                    QuestionName = q.QuestionName,
                    Option = q.Option.Select(o => new Option
                    {
                        Id = o.Id,
                        Name = o.Name,
                        IsCorrect = o.IsCorrect
                    }).ToList()
                })
                .ToListAsync();

            // Response
            var response = new QuestionListResponse
            {
                TotalRecords = totalRecords,
                Data = data
            };

            return Ok(response);
        }
    }

    
}
