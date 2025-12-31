using battlesimulation.api.Models;

namespace battlesimulation.api.Interfaces
{
    public interface IBattleSimulator
    {
        BattleResults Battle<TFighter>(List<int> fighters) where TFighter : IFighter; 
    }
}
