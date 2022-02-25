using System;
namespace NiceReads.Infrastructure.Entities
{
	public class Article : BaseEntity
	{
        public Article(string title, string body, Guid userId)
        {
            Title = title;
            Body = body;
            AuthorId = userId;
        }

        public string Title { get; set; }
        public string Body { get; set; }

        public Guid AuthorId { get; set; }
        public virtual ApplicationUser Author { get; set; }

        public Guid CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}

