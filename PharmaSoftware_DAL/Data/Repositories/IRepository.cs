using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PharmaSoftware_DAL.Data.Repositories
{
    public interface IRepository<T>
        where T: class, new()
    {
        IEnumerable<T> Get();
        void Add(T entity);
        void Edit(T entity);
        void Delete(T entity);

        //uitbreiding
        IEnumerable<T> Get(Expression<Func<T, bool>> voorwaarden);
        IEnumerable<T> Get(params Expression<Func<T, object>>[] includes);
        IEnumerable<T> Get(Expression<Func<T, bool>> voorwaarden,
            params Expression<Func<T, object>>[] includes);

        //handige functies
        T SearchWithPK<TPrimaryKey>(TPrimaryKey id);
        void AddOrEdit(T entity);
        void AddRange(IEnumerable<T> entities);
        void Delete<TPrimaryKey>(TPrimaryKey id);
        void DeleteRange(IEnumerable<T> entities);
    }
}
