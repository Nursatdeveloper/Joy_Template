using Joy_Template.Data.Tables;
using Microsoft.EntityFrameworkCore;
using MVCTemplate.Sources.Repository;

namespace MVCTemplate.Data {
    public class ApplicationDbContext : DbContext {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {

        }

        // System Tables
        public DbSet<TbExceptions> TbExceptions { get; set; }
        public DbSet<TbLogs> TbLogs { get; set; }

        // Application Tables
        public DbSet<TbTest> TbTest { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<TbExceptions>()
                .ToTable("tbexceptions", schema: "system");

            modelBuilder.Entity<TbLogs>()
                .ToTable("tblogs", schema: "system");

            modelBuilder.Entity<TbTest>()
                .ToTable("tbtest", schema: "application");
        }

    }

    public class TbTest : TbBase {
        public string Name { get; set; }
    }
}
