using MVCTemplate.Data;
using System.ComponentModel.DataAnnotations;

namespace Joy_Template.Data.Tables {
    public class TbExceptions {
        [Key]
        public long Id { get; set; }
        public bool IsProcessed { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Exception { get; set; }
    }

    public class TbLogs {
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

    public class SystemMonitor : ISystemMonitor {

        private readonly ApplicationDbContext _context;
        public SystemMonitor(ApplicationDbContext context) {
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
                Exception = exception,
                IsProcessed = false
            });
            await _context.SaveChangesAsync();
        }
    }

}