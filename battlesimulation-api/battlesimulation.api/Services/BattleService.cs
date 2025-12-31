using battlesimulation.api.Models;
using battlesimulation.api.Interfaces;
using battlesimulation.api.Services;

namespace battlesimulation.api.Services
{
    public class BattleService : IBattleSimulator
    {
        private readonly IFighterService _service;
        public BattleService(IFighterService transformersRepo) 
        {
            _service = transformersRepo;
        }
        public BattleResults Battle<TFighter>(List<int> idsOfFighters) where TFighter : IFighter
        {
            IFighter curFighter;
            BattleResults battleResults;  
            int herosPower = 0;
            int villainsPower = 0;
            List<int> heroIds = new List<int>();
            List<int> villainIds = new List<int>();
            List<int> allIds = new List<int>();
            List<int> winners;
            List<int> losers;
            bool isTie = false;

            foreach (int fighterId in idsOfFighters) 
            {
                curFighter = _service.Get<TFighter>(fighterId);

                if (curFighter.Alignment == Alignment.Hero)
                {
                    herosPower += curFighter.Power;
                    heroIds.Add(fighterId);
                }
                else
                {
                    villainsPower += curFighter.Power;
                    villainIds.Add(fighterId);

                    //Rogue alignments join the Villains automatically
                    if (curFighter.Alignment != Alignment.Villain)
                    {
                        Console.WriteLine($"Warning: Fighter: {curFighter.Name}, of alignment: {curFighter.Alignment} enterd the battle");
                    }
                }
            }

            if (herosPower > villainsPower)
            {
                winners = heroIds;
                losers = villainIds;
            }
            else if (herosPower < villainsPower)
            {
                winners = villainIds;
                losers = heroIds;
            }
            else
            {
                isTie = true;

                allIds.AddRange(heroIds);
                allIds.AddRange(villainIds);

                winners = new List<int>();
                losers = allIds;
            }

            battleResults = new BattleResults(winners, losers, isTie);

            foreach (int winnerId in battleResults.Winners)
            {
                _service.Get<TFighter>(winnerId).AddWin();
            }
            foreach (int loserId in battleResults.Losers)
            {
                _service.Get<TFighter>(loserId).AddLoss();
            }

            return battleResults;
        }
    }
}
