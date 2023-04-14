using MVCTemplate.Data;
using MVCTemplate.Sources.Repository;

namespace Joy_Template.Sources.Repository {
    public interface IRepositoryProvider {

        IRepositoryBase<ApplicationDbContext, TbTest> GetTbTest();
    }
    public class RepositoryProvider : IRepositoryProvider {
        private readonly IRepositoryBase<ApplicationDbContext, TbTest> _tbTestRepository;
        public RepositoryProvider(
            IRepositoryBase<ApplicationDbContext, TbTest> tbTestRepository
        ) {
            _tbTestRepository = tbTestRepository;
        }
        public IRepositoryBase<ApplicationDbContext, TbTest> GetTbTest() {
            return _tbTestRepository;
        }
    }
}
