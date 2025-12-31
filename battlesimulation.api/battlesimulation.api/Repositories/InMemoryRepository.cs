using battlesimulation.api.Interfaces;
using battlesimulation.api.Models;

namespace battlesimulation.api.Repositories
{
    public class InMemoryRepository<T> : IRepository<T> where T : IFighter
    {
        private readonly Dictionary<int, T> _storage = new();
        private int _nextId = 0;
        public void Store(T entity)
        {
            entity.AssignId(_nextId);
            _storage[_nextId] = entity;
            _nextId++;
        }
        public void Update(int id, T entity)
        {
            _storage[id] = entity;
        }
        public T Retrieve(int id)
        {
            if (_storage.TryGetValue(id, out var entity))
                return entity;

            throw new KeyNotFoundException($"No fighter found with ID {id}");
        }
        public T[] RetrieveAll()
        {
            return _storage.Values.ToArray(); 
        }
    }
}
