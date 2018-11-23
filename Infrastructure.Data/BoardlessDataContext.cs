using Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Tmpps.Infrastructure.Data.Interfaces;
using Tmpps.Infrastructure.Npgsql.Entity;

namespace Infrastructure.Data
{
    public class BoardlessDataContext : NpgsqlDbContext
    {
        public BoardlessDataContext(DbContextOptions options) : base(options) { }
        public BoardlessDataContext(DbContextOptions<BoardlessDataContext> options) : base(options) { }
        public BoardlessDataContext(DbContextOptions options, IDbQueryCache queryPool) : base(options, queryPool) { }
        public BoardlessDataContext(DbContextOptions<BoardlessDataContext> options, IDbQueryCache queryPool) : base(options, queryPool) { }

        public DbSet<User> Users { get; set; }
    }
}