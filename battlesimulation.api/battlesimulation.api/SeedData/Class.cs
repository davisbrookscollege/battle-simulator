using battlesimulation.api.Interfaces;
using battlesimulation.api.Models;

namespace battlesimulation.api.SeedData;

public static class SeedData
{
    //Initialize more fighters here if necessary 
    public static void Initialize(IFighterService service)
    {
        Transformer[] intialTransformers =
            {
                // Autobots
                new Transformer("Optimus Prime", Faction.Autobot, "Semi‑truck", 9, 10),
                new Transformer("Bumblebee", Faction.Autobot, "Volkswagen Beetle", 4, 8),
                new Transformer("Ironhide", Faction.Autobot, "Van", 8, 6),
                new Transformer("Ratchet", Faction.Autobot, "Ambulance", 5, 9),
                new Transformer("Wheeljack", Faction.Autobot, "Sports Car", 6, 9),

                // Decepticons
                new Transformer("Megatron", Faction.Decepticon, "Handgun", 10, 7),
                new Transformer("Starscream", Faction.Decepticon, "Jet", 7, 8),
                new Transformer("Soundwave", Faction.Decepticon, "Cassette Player", 6, 9),
                new Transformer("Shockwave", Faction.Decepticon, "Laser Cannon", 8, 10),
                new Transformer("Thundercracker", Faction.Decepticon, "Jet", 7, 6)
            };
        foreach (Transformer t in intialTransformers)
        {
            service.Store(t);
        }
    }
}