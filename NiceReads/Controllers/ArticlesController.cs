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
            var article = await _context.Articles
                .Include(a => a.Author)
                .Include(a => a.Category)
                .SingleOrDefaultAsync(a => a.Id == id);

            if (article == null)
            {
                return NotFound();
            }

            return article;
        }

        [HttpGet("category/{id}")]
        public async Task<ActionResult<List<Article>>> GetArticlesByCategory(Guid categoryId)
        {
            var articles = await _context.Articles
                .Include(a => a.Category)
                .Where(a => a.CategoryId == categoryId)
                .ToListAsync();

            return Ok(articles);
        }

        [HttpGet("categories")]
        public async Task<ActionResult<List<Category>>> GetCategories()
        {
            var categories = await _context.Categories
                .ToListAsync();

            return Ok(categories);
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

            var category = await _context.Categories
                .FindAsync(request.CategoryId);

            if (category is null)
            {
                return NotFound("Category not found");
            }

            article.Body = request.Body;
            article.Title = request.Title;
            article.AuthorId = Guid.Parse(HttpContext.User.Claims.First(c => c.Type == "UserId").Value);
            article.CategoryId = request.CategoryId;

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
            public Guid CategoryId { get; set; }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Article>> PostArticle(PostArticleRequest request)
        {
            var category = await _context.Categories
                .FindAsync(request.CategoryId);

            if (category is null)
            {
                return NotFound("Category not found");
            }

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
