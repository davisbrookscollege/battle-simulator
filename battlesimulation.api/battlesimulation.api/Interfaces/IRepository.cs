namespace battlesimulation.api.Interfaces
{
    public interface IRepository<T>
    {
        void Store(T entity);
        void Update(int id, T entity);
        T Retrieve(int id);
        T[] RetrieveAll();
    }
}

