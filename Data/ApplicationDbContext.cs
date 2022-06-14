using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TesteFoa.Models;

namespace TesteFoa.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<TesteFoa.Models.Book> Book { get; set; }
        public DbSet<TesteFoa.ViewModels.BookViewModel> BookViewModel { get; set; }
        public DbSet<TesteFoa.Models.Author> Author { get; set; }
    }
}
