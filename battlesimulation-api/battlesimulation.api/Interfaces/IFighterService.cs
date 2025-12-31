using battlesimulation.api.Models;
using battlesimulation.api.Interfaces;

namespace battlesimulation.api.Interfaces
{
    public interface IFighterService
    {
        void Store(IFighter t);
        TFighter Get<TFighter>(int id) where TFighter : IFighter;
        IEnumerable<TFighter> GetAll<TFighter>() where TFighter : IFighter;
    }
}
