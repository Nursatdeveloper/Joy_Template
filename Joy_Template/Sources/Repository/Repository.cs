using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace MVCTemplate.Sources.Repository {
    public interface IRepositoryBase<TContext, TModel>
           where TContext : DbContext
           where TModel : TbBase {

        Task<TModel> GetAsync(Expression<Func<TModel, bool>> filter);
        Task<TModel> InsertAsync(TModel entity);
        Task UpdateAsync(TModel entity);
        Task DeleteAsync(TModel entity);
    }

    public class RepositoryBase<TContext, TModel>: IRepositoryBase<TContext, TModel>
        where TContext : DbContext
        where TModel : TbBase {

        private TContext _context;
        public RepositoryBase(TContext context) {
            _context = context;
        }
        public Task DeleteAsync(TModel entity) {
            throw new NotImplementedException();
        }

        public Task<TModel> GetAsync(Expression<Func<TModel, bool>> filter) {
            throw new NotImplementedException();
        }

        public async Task<TModel> InsertAsync(TModel entity) {
            try {
                var createdObject = await _context.Set<TModel>().AddAsync(entity);
                await _context.SaveChangesAsync();
                return createdObject.Entity;
            } catch(Exception ex) {
                await _tbExceptions.InsertAsync(new TbExceptions() {
                    CreatedAt = DateTime.UtcNow,
                    Exception = ex.Message
                });
                throw new Exception(ex.Message);
            }
        }

        public Task UpdateAsync(TModel entity) {
            throw new NotImplementedException();
        }
    }

    public class TbBase {
        [Key]
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int RowVersion { get; set; }
    }
}
