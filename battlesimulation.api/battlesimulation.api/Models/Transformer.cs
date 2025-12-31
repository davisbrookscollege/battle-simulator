using battlesimulation.api.Interfaces;
using battlesimulation.api.Models;

namespace battlesimulation.api.Models
{
    public class Transformer : ITransformer
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int Wins { get; private set; }
        public int Losses { get; private set; }
        public Alignment Alignment => Faction == Faction.Autobot ? Alignment.Hero : Alignment.Villain;
        public Faction Faction { get; private set; }
        public string AltVehicle { get; private set; }
        public int Strength { get; private set; }
        public int Intelligence { get; private set; }
        public int Power => Strength * Intelligence;
        public Transformer(string name, Faction faction, string altVehicle, int strength, int intelligence) 
        {
            Wins = 0;
            Losses = 0;
            Name = name;
            Faction = faction;
            AltVehicle = altVehicle;
            Strength = strength;
            Intelligence = intelligence;
        }
        public void AssignId(int id)
        {
            Id = id; 
        }
        public void AddWin()
        {
            Wins++;
        }
        public void AddLoss()
        {
            Losses++;
        }
        public void UpdateFaction(Faction newFaction) 
        { 
            Faction = newFaction;
        }
    }
}
