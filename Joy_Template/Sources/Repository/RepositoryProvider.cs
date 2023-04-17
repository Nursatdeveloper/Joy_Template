using Joy_Template.Data.Tables;
using MVCTemplate.Data;
using MVCTemplate.Sources.Repository;

namespace Joy_Template.Sources.Repository {
    public interface IRepositoryProvider {
        IRepositoryBase<ApplicationDbContext, TbUser> Users { get; }
    }
    public class RepositoryProvider: IRepositoryProvider {
        private readonly ApplicationDbContext _context;
        private readonly ISystemMonitor _systemMonitor;
        public RepositoryProvider(ApplicationDbContext context, ISystemMonitor systemMonitor) {
            _context = context;
            _systemMonitor = systemMonitor;
        }
        private IRepositoryBase<ApplicationDbContext, TbUser> _users;
        public IRepositoryBase<ApplicationDbContext, TbUser> Users {
            get {
                if (_users == null) {
                    _users = new RepositoryBase<ApplicationDbContext, TbUser>(_context, _systemMonitor);
                }
                return _users;
            }
        }
    }
}
