using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestApi.Persistence
{
    public interface IRepository<T, ID>
    {
        T GetById(ID id);

        IEnumerable<T> GetAll();

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);

        Task<bool> SaveChagesAsync();
    }
}