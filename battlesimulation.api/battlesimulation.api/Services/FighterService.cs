using battlesimulation.api.Interfaces;
using battlesimulation.api.Models;
using battlesimulation.api.Repositories;

namespace battlesimulation.api.Services
{
    public class FighterService : IFighterService
    {
        //Add more repositorys if more fighter types are created
        private readonly InMemoryRepository<Transformer> _transformerRepository;
        public FighterService()
        {
            _transformerRepository = new InMemoryRepository<Transformer>();
        }
        public void Store(IFighter fighter)
        {
            if (fighter is Transformer t)
            {
                _transformerRepository.Store(t);
            }
        }
        public TFighter Get<TFighter>(int id) where TFighter : IFighter
        {
            if (typeof(TFighter) == typeof(Transformer))
            {
                return (TFighter)(IFighter)_transformerRepository.Retrieve(id);
            }

            throw new NotSupportedException($"No repository registered for fighter type {typeof(TFighter).Name}");
        }
        public IEnumerable<TFighter> GetAll<TFighter>() where TFighter : IFighter
        {
            if (typeof(TFighter) == typeof(Transformer))
            {
                return _transformerRepository
                    .RetrieveAll()
                    .Cast<TFighter>();
            }

            throw new NotSupportedException($"No repository registered for fighter type {typeof(TFighter).Name}");
        }
    }
}
