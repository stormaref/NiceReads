using System;
using Microsoft.AspNetCore.Identity;

namespace NiceReads.Infrastructure.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser(string userName, string firstName, string lastName) : base(userName)
        {
            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Name() => $"{FirstName} {LastName}";
    }
}

