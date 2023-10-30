using LogixTask.Models;
using LogixTask.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LogixTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public DashboardController(AppDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var users = await _dbContext.WebUsers.ToListAsync();
            if(users.Count > 0)
            {
                return Ok(users);
            }

            return NotFound();
        }

        [HttpGet("Profile")]
        public async Task<IActionResult> Profile(string userId)
        {
            var user = await _dbContext.WebUsers.Where(u => u.Id == userId).FirstOrDefaultAsync();
            if(user != null)
            {
                var model = new ProfileViewModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    FullName = user.FullName,
                    Address = user.Address,
                    PhoneNumber = user.PhoneNumber,
                    DateOfBirth = user.DateOfBirth,
                    Email = user.Email
                };
                return Ok(model);
            }

            return NotFound();
        }

        [HttpGet("Edit")]
        public async Task<IActionResult> Edit(string userId)
        {
            var user = await _dbContext.WebUsers.Where(u => u.Id == userId).FirstOrDefaultAsync();
            if (user != null)
            {
                var model = new WebUserEditViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    FullName = user.FullName,
                    Address = user.Address,
                    PhoneNumber = user.PhoneNumber,
                    DateOfBirth = user.DateOfBirth,
                    Email = user.Email
                };
                return Ok(user);
            }
            return NotFound();
        }

        [HttpPost("Edit")]
        public async Task<IActionResult> Edit(WebUserEditViewModel webUserEditViewModel)
        {
            var user = await _dbContext.WebUsers.Where(u => u.Id == webUserEditViewModel.Id).FirstOrDefaultAsync();

            if(user != null)
            {
                user.FirstName = webUserEditViewModel.FirstName;
                user.LastName = webUserEditViewModel.LastName;
                user.PhoneNumber = webUserEditViewModel.PhoneNumber;
                user.FullName = webUserEditViewModel.FullName;
                user.DateOfBirth = webUserEditViewModel.DateOfBirth;
                user.Email = webUserEditViewModel.Email;
                user.Address = webUserEditViewModel.Address;

                await _dbContext.SaveChangesAsync();
                return Ok();
            }

            return NotFound();
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _dbContext.WebUsers.Where(u => u.Id == id).FirstOrDefaultAsync();
            if(user != null)
            {
                _dbContext.Remove(user);
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            return NotFound();
        }
    }
}
