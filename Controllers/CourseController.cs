using LogixTask.Models;
using LogixTask.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LogixTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public CourseController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(string name)
        {
            if(name != null)
            {
                var course = new Course
                {
                    Name = name
                };

                await _dbContext.Courses.AddAsync(course);
                await _dbContext.SaveChangesAsync();

                return Ok();
            }

            return BadRequest();
        }
    }
}
