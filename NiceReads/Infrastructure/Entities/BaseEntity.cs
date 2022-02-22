using System;
namespace NiceReads.Infrastructure.Entities
{
	public class BaseEntity
	{
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}

