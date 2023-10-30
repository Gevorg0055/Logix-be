using LogixTask.Models;
using LogixTask.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LogixTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public UserController(AppDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        

        [HttpPost("AssignUserToCourse")]
        public async Task<IActionResult> AssignUserToCourse(UserClassAssignmentViewModel userClassAssignmentViewModel)
        {
            if (userClassAssignmentViewModel.CoursesIds != null && userClassAssignmentViewModel.UserId != null)
            {
                foreach (var courseId in userClassAssignmentViewModel.CoursesIds)
                {
                    var userCourse = new UserClass
                    {
                        CourseId = courseId,
                        WebUserId = userClassAssignmentViewModel.UserId
                    };

                    _dbContext.Add(userCourse);
                    await _dbContext.SaveChangesAsync();
                }

                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("EditAssignedCourses")]
        public async Task<IActionResult> EditAssignedCourses(UserClassAssignmentViewModel userClassAssignmentViewModel)
        {
            
            if (userClassAssignmentViewModel.CoursesIds != null && userClassAssignmentViewModel.UserId != null)
            {
                var userCourses = await _dbContext.UserClasses.Where(c => c.WebUserId == userClassAssignmentViewModel.UserId).ToListAsync();
                _dbContext.UserClasses.RemoveRange(userCourses);

                foreach (var courseId in userClassAssignmentViewModel.CoursesIds)
                {
                    var userCourse = new UserClass
                    {
                        CourseId = courseId,
                        WebUserId = userClassAssignmentViewModel.UserId
                    };

                    _dbContext.Add(userCourse);
                    await _dbContext.SaveChangesAsync();
                }

                return Ok();
            }

            return BadRequest();
        }

        [HttpDelete("RemoveUserFromCourse")]
        public async Task<IActionResult> RemoveUserFromCourse(string userId,int courseId)
        {
            if(userId != null && courseId > 0)
            {
                var course = await _dbContext.UserClasses.Where(c => c.WebUserId == userId && c.CourseId == courseId).FirstOrDefaultAsync();
                if(course != null)
                {
                    _dbContext.UserClasses.Remove(course);
                    await _dbContext.SaveChangesAsync();
                }
            }
            return NotFound();
        }
    }
}
