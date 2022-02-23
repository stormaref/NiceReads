using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NiceReads.Infrastructure.Entities;
using NiceReads.Infrastructure.Persistence;

namespace NiceReads.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public ArticlesController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticles()
        {
            return await _context.Articles.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Article>> GetArticle(Guid id)
        {
            var article = await _context.Articles.Include(a => a.User).SingleOrDefaultAsync(a => a.Id == id);

            if (article == null)
            {
                return NotFound();
            }

            return article;
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutArticle(Guid id, PostArticleRequest request)
        {

            var article = await _context.Articles.SingleOrDefaultAsync(a => a.Id == id);

            if (article is null)
            {
                return NotFound();
            }

            article.Body = request.Body;
            article.Title = request.Title;
            article.UserId = Guid.Parse(HttpContext.User.Claims.First(c => c.Type == "UserId").Value);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        public class PostArticleRequest
        {
            public string Title { get; set; }
            public string Body { get; set; }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Article>> PostArticle(PostArticleRequest request)
        {
            var userId = Guid.Parse(HttpContext.User.Claims.First(c => c.Type == "UserId").Value);
            var article = new Article(request.Title, request.Body, userId);
            _context.Articles.Add(article);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArticle", new { id = article.Id }, article);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteArticle(Guid id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArticleExists(Guid id)
        {
            return _context.Articles.Any(e => e.Id == id);
        }
    }
}
