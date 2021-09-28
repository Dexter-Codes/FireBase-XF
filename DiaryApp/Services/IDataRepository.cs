using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiaryApp.Services
{
    public interface IDataRepository<T> where T : IIdentifiable
    {
        Task<T> Get(string id);
        Task<IEnumerable<T>> GetAll();
        Task<string> Save(T item);
        Task<bool> Delete(T item);
        Task<bool> Update(T item);
    }
}
