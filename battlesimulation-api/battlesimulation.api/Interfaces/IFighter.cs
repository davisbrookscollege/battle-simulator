using battlesimulation.api.Models;

namespace battlesimulation.api.Interfaces

{
    public interface IFighter
    {
        int Id { get; }
        string Name { get; }
        int Wins { get; }
        int Losses { get; }
        int Power { get; }
        Alignment Alignment { get; }
        void AssignId(int id);
        void AddWin();
        void AddLoss();
    }
}
