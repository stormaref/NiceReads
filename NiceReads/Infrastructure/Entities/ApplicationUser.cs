using System;
using Microsoft.AspNetCore.Identity;

namespace NiceReads.Infrastructure.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Name() => $"{FirstName} {LastName}";
    }
}

