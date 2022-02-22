using System;
using Microsoft.EntityFrameworkCore;

namespace NiceReads.Infrastructure.Persistence
{
	public class DatabaseContext : DbContext
	{
		public DatabaseContext()
		{
		}

        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }
    }
}

