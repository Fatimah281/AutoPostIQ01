csharp
using AutoPostIQ.Data;
using AutoPostIQ.Models;
using Microsoft.AspNetCore.Mvc;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoPostIQ.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public AdminController(AppDbContext context, IBackgroundJobClient backgroundJobClient)
        {
            _context = context;
            _backgroundJobClient = backgroundJobClient;
        }

        [HttpGet("pending")]
        public async Task<ActionResult<IEnumerable<PostModel>>> GetPendingPosts()
        {
            var pendingPosts = await _context.Posts
                .Where(p => p.Status == "Pending Approval")
                .ToListAsync();

            return Ok(pendingPosts);
        }

        [HttpPost("approve/{id}")]
        public async Task<IActionResult> ApprovePost(int id)
        {
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            post.Status = "Approved";
            await _context.SaveChangesAsync();

            // Schedule the LinkedIn posting job
            // You'll need to calculate the actual delay to schedule for the next Monday 2 PM
            var delay = TimeSpan.FromDays(7); // Placeholder: schedule for 7 days from now
            _backgroundJobClient.Schedule<Jobs.JobScheduler>(x => x.PublishToLinkedInJob(post.Id), delay);

            return NoContent();
        }

        [HttpPost("reject/{id}")]
        public async Task<IActionResult> RejectPost(int id)
        {
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            post.Status = "Rejected";
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("edit/{id}")]
        public async Task<IActionResult> EditPost(int id, [FromBody] PostModel updatedPost)
        {
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            post.Content = updatedPost.Content; // Assuming Content is the editable field
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}