using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;
using MVCTemplate.Sources.Repository;

namespace MVCTemplate.Data {
    public class SystemDbContext: DbContext {
        public SystemDbContext(DbContextOptions<SystemDbContext> options) : base(options) {

        }
        public DbSet<TbExceptions> TbExceptions { get; set; }
        public DbSet<TbLogs> TbLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<TbExceptions>()
                .ToTable("tbexceptions", schema: "system");

            modelBuilder.Entity<TbLogs>()
                .ToTable("tblogs", schema: "system");
        }

    }

    public class TbExceptions{
        [Key]
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Exception { get; set; }
    }

    public class TbLogs{
        [Key]
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
    }

    public enum LogType {
        Warning,
        Info,
        Error
    }

    public interface ISystemMonitor {
        Task SaveExceptionAsync(string Exception);
        Task LogMessageAsync(LogType logType, string Message);
    }

    public class SystemMonitor: ISystemMonitor {

        private readonly SystemDbContext _context;
        public SystemMonitor(SystemDbContext context) {
            _context = context;
        }
        public async Task LogMessageAsync(LogType logType, string message) {
            await _context.Set<TbLogs>().AddAsync(new TbLogs {
                CreatedAt = DateTime.UtcNow,
                Type = logType.ToString(),
                Message = message
            });
            await _context.SaveChangesAsync();
        }

        public async Task SaveExceptionAsync(string exception) {
            await _context.Set<TbExceptions>().AddAsync(new TbExceptions {
                CreatedAt = DateTime.UtcNow,
                Exception = exception
            });
            await _context.SaveChangesAsync();
        }
    }


}
