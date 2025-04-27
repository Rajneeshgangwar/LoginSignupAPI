using LoginSignupAPI.Model;
using LoginSignupAPI.Model.Data;
using LoginSignupAPI.Model.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoginSignupAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public UserController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        [HttpPost]
        public IActionResult SaveUser(UserDto user)
        {
            User userEntity = new User
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                Role = user.Role,
                Phone = user.Phone
            };
            var tempuser = _dbContext.Users.Where(x => x.Email == user.Email).FirstOrDefault();
            if (tempuser != null)
            {
                return Ok("User Alredy Exist");
            }
            _dbContext.Users.Add(userEntity);
            _dbContext.SaveChanges();
            return Ok(userEntity);

        }
        [HttpGet("{id:int}")]
        public IActionResult GetUser(int id)
        {
            var user = _dbContext.Users.Find(id);
            return Ok(user);

        }
    }
}
