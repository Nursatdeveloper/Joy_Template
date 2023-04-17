using Joy_Template.Data.Tables;
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

    public class RepositoryBase<TContext, TModel> : IRepositoryBase<TContext, TModel>
        where TContext : DbContext
        where TModel : TbBase {

        private TContext _context;
        private readonly ISystemMonitor _monitor;
        public RepositoryBase(TContext context, ISystemMonitor monitor) {
            _context = context;
            _monitor = monitor;
        }
        public Task DeleteAsync(TModel entity) {
            try {
                var createdObject = _context.Set<TModel>().Remove(entity);
                _context.SaveChanges();
                return Task.CompletedTask;
            } catch (Exception ex) {
                _monitor.SaveExceptionAsync(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<TModel> GetAsync(Expression<Func<TModel, bool>> filter) {
            try {
                var entity = await _context.Set<TModel>().FindAsync(filter);
                return entity;
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public async Task<TModel> InsertAsync(TModel entity) {
            try {
                EnsureModelInitedProperly(entity);
                var createdObject = await _context.Set<TModel>().AddAsync(entity);
                await _context.SaveChangesAsync();
                return createdObject.Entity;
            } catch (Exception ex) {
                await _monitor.SaveExceptionAsync(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateAsync(TModel entity) {
            await EnsureRowVersionIsUpdated(entity);
            try {
                var createdObject = _context.Set<TModel>().Update(entity);
                _context.SaveChanges();
            } catch (Exception ex) {
                _monitor.SaveExceptionAsync(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        private async Task EnsureRowVersionIsUpdated(TModel entity) {
            try {
                var model = await GetAsync(f => f.Id == entity.Id);
                if (model.RowVersion + 1 != entity.RowVersion) {
                    entity.RowVersion = model.RowVersion + 1;
                }
            } catch (Exception ex) {
                await _monitor.SaveExceptionAsync(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        private void EnsureModelInitedProperly(TModel entity) {
            entity.RowVersion = 1;
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = null;
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
