using System;
namespace NiceReads.Infrastructure.Entities
{
	public class Article : BaseEntity
	{
        public string Title { get; set; }
        public string Body { get; set; }
        public Guid UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}

