using StreamFile.Contract.Repository.Models;
using Invedia.Data.EF.Interfaces.Repository;

namespace StreamFile.Contract.Repository.Infrastructure
{
    public interface IRepository<T> : IStringEntityRepository<T> where T : Entity, new()
    {
        bool Insert(T entity);
    }
}