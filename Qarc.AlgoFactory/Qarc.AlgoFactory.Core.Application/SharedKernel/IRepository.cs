using Qarc.Algos.SharedKernel.InputModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Qarc.AlgoFactory.Core.Application.SharedKernel
{
    public interface IRepository<T> where T : IEntity
    {
        Task CreateAsync(T entity);
        Task CreateManyAsync(IEnumerable<T> entities);
        Task<IReadOnlyCollection<T>> GetAllAsync();
        Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter);
        Task<T> GetAsync(string id);
        Task<T> GetAsync(Expression<Func<T, bool>> filter);
        Task RemoveAsync(string id);
        Task UpdateAsync(T entity);
    }
}
