using battlesimulation.api.Models;

namespace battlesimulation.api.Interfaces
{
    public interface ITransformer : IFighter
    {
        Faction Faction { get; }
        string AltVehicle { get; }
        int Strength { get; }   
        int Intelligence { get; } 
        void UpdateFaction(Faction newFaction);
    }
}
