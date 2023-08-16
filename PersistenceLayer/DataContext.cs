using DomainLayer;
using Microsoft.EntityFrameworkCore;
using System;

namespace PersistenceLayer
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Activity> Activities { get; set; }
    }
}
