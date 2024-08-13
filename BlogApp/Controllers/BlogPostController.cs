using BlogApp.Data;
using BlogApp.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostController : ControllerBase
    {
        private readonly DataContext _context;

        public BlogPostController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<BlogPost>>> GetAllPosts()
        {
            List<BlogPost> posts = await _context.BlogPosts.ToListAsync();
            return Ok(posts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BlogPost>> GetPost(int id)
        {
            BlogPost post = await _context.BlogPosts.FindAsync(id);
            if (post == null)
            {
                return NotFound("Post not found.");
            }
            return Ok(post);
        }

        [HttpPost]
        public async Task<IActionResult> AddPost([FromBody] BlogPost post)
        {
            post.CreatedAt = DateTime.Now;
            post.UpdateAt = DateTime.Now;
            _context.BlogPosts.Add(post);
            await _context.SaveChangesAsync();
            return Ok(post);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpgradePost(int id, [FromBody] BlogPost post)
        {
            if (post.Id != 0)
            {
                return BadRequest("Don't change ID.");
            }
            BlogPost dbPost = await _context.BlogPosts.FindAsync(id);
            if (dbPost == null)
            {
                return NotFound("Post not found.");
            }
            dbPost.UpdateAt = DateTime.Now;
            if (dbPost.Content != "")
            {
                dbPost.Content = post.Content;
            }
            if (dbPost.Title != "")
            {
                dbPost.Title = post.Title;
            }
            await _context.SaveChangesAsync();
            return Ok(post);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            BlogPost post = await _context.BlogPosts.FindAsync(id);
            if (post == null)
            {
                return NotFound("Post not found.");
            }
            _context.BlogPosts.Remove(post);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
