using BlogApp.Data;
using BlogApp.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;

        public UserController(DataContext context)
        {
            _context = context;
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            User user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            user.CreatedAt = DateTime.Now;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Created();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpgradeUser(int id,[FromBody] User user) 
        {
            if (user.Id == 0)
            {
                return BadRequest("Don't change ID.");
            }
            User dbUser = await _context.Users.FindAsync(user.Id);
            if (dbUser == null)
            {
                return NotFound("User not found.");
            }

            if (user.Username != "")
            {
                dbUser.Username = user.Username;
            }
            if (user.Email != "")
            {
                dbUser.Email = user.Email;
            }
            if (user.PasswordHash != "")
            {
                dbUser.PasswordHash = user.PasswordHash;
            }
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            User user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
