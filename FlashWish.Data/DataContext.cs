using FlashWish.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashWish.Data
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<GreetingCard> GreetingCards { get; set; }
        public DbSet<GreetingMessage> GreetingMessages { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=FlashWish1_db;TrustServerCertificate=True;Trusted_Connection=True;");
            //base.OnConfiguring(optionsBuilder);//----
        }
    }
}
