using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Classroom.SimpleCRM.SqlDbServices
{
    public class CrmIdentityDbContext : IdentityDbContext<CrmIdentityUser, IdentityRole<string>, string>
    {
        public CrmIdentityDbContext(DbContextOptions<CrmIdentityDbContext> options) : base(options) { }
        protected CrmIdentityDbContext() : base() { }
    }
}
