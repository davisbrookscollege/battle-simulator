namespace battlesimulation.api.Models
{
    public class BattleResults
    {
        public IReadOnlyList<int> Winners { get; }
        public IReadOnlyList<int> Losers { get; }
        public bool IsTie { get; } = false;
        public BattleResults(List<int> winnerIds, List<int> loserIds, bool isTie) 
        { 
            Winners = winnerIds;
            Losers = loserIds;
            IsTie = isTie;
        }
    }
}
