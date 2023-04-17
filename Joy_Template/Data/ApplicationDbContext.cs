using BCrypt.Net;
using Joy_Template.Data.Tables;
using Microsoft.EntityFrameworkCore;
using MVCTemplate.Sources.Repository;
using System.Reflection.Metadata;

namespace MVCTemplate.Data {
    public class ApplicationDbContext : DbContext {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        // System Tables
        public DbSet<TbExceptions> TbExceptions { get; set; }
        public DbSet<TbLogs> TbLogs { get; set; }

        // Application Tables
        public DbSet<TbUser> TbUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<TbExceptions>()
                .ToTable("tbexceptions", schema: "system");

            modelBuilder.Entity<TbLogs>()
                .ToTable("tblogs", schema: "system");

            modelBuilder.Entity<TbRole>()
                .ToTable("tbrole", schema: "user");
            modelBuilder.Entity<TbRole>().HasData(
                new TbRole { Id = 1, Role = "User", CreatedAt = DateTime.UtcNow, RowVersion = 1, UpdatedAt = null},
                new TbRole { Id = 2, Role = "Moderator", CreatedAt = DateTime.UtcNow, RowVersion = 1, UpdatedAt = null},
                new TbRole { Id = 3, Role = "Admin", CreatedAt = DateTime.UtcNow, RowVersion = 1, UpdatedAt = null}
            );

            modelBuilder.Entity<TbUser>()
                .ToTable("tbusers", schema: "user");
            modelBuilder.Entity<TbUser>().HasData(
                new TbUser { Id = 1, Firstname = "Nursat", Lastname = "Zeinolla", Fathername = "Erzatuly", BirthDate = new DateTime(2004, 4, 24), Iin = "040524501037", Email = "kznursat@gmail.com",Password = BCrypt.Net.BCrypt.HashPassword("nursat"), CreatedAt = DateTime.UtcNow, Roles = "Moderator, Admin, User", RowVersion = 1, UpdatedAt = null }
            );
        }
    }

}
