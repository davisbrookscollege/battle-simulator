using Microsoft.AspNetCore.Mvc;
using battlesimulation.api.Models;
using battlesimulation.api.Services;
using battlesimulation.api.Interfaces;
using battlesimulation.api.Dtos;


namespace battlesimulation.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransformersController : ControllerBase
    {
        private readonly IFighterService _service;
        private readonly IBattleSimulator _battleService;
        public TransformersController(IFighterService transformersRepo, IBattleSimulator battleService)
        {
            _service = transformersRepo;
            _battleService = battleService;

        }

        [HttpPost("battle")]
        public IActionResult runBattle([FromBody] List<int> fighterIds)
        {
            BattleResults battleResults = _battleService.Battle<Transformer>(fighterIds);

            Console.WriteLine("Winners: " + string.Join(", ", battleResults.Winners));

            return Ok(battleResults);
        }

        [HttpGet("{id}")]
        public ActionResult<Transformer> GetTransformer(int id)
        {
            return _service.Get<Transformer>(id);
        }

        [HttpDelete("{id}")]
        public ActionResult RemoveTransformer(int id)
        {
            Transformer removedTransformer = _service.Get<Transformer>(id);
            removedTransformer.UpdateFaction(Faction.Removed);
            return Ok();
        }

        [HttpGet]
        public ActionResult<Transformer[]> GetAllTransformers()
        {
            return _service.GetAll<Transformer>().ToArray();
        }
        [HttpGet("faction/{faction}")]
        public ActionResult<Transformer[]> GetByFaction(string faction)
        {
            faction = faction.ToLower();
            Faction chosenFaction;

            if (faction == "autobots")
            {
                chosenFaction = Faction.Autobot;
            }
            else if (faction == "decepticons")
            {
                chosenFaction = Faction.Decepticon;
            }
            else if (faction == "removed")
            {
                chosenFaction = Faction.Removed;
            }
            else
            {
                return BadRequest("Invalid faction. Use autobots or decepticons.");
            }
            Transformer[] allTransformers = _service.GetAll<Transformer>().ToArray();

            Transformer[] members = allTransformers
                .Where(t => t.Faction == chosenFaction)
                .ToArray();

            return members;
        }

        [HttpPost("create")]
        public IActionResult CreateTransformer([FromBody] CreateTransformerRequest request)
        {
            if (!ModelState.IsValid)
            {
                foreach (var entry in ModelState)
                {
                    foreach (var error in entry.Value.Errors)
                    {
                        Console.WriteLine($"Model binding error for {entry.Key}: {error.ErrorMessage}");
                    }
                }

                return BadRequest(ModelState);
            }

            var transformer = new Transformer(
                request.Name ?? "",
                request.Faction,
                request.AltVehicle ?? "",
                request.Strength,
                request.Intelligence
            );
            _service.Store(transformer);

            Console.WriteLine($"Transformer: {request.Name}");

            return Ok(transformer);
        }

        [HttpGet("faction/names")]
        public IActionResult GetFactionNames()
        {
            var factions = Enum.GetValues(typeof(Faction))
            .Cast<Faction>()
            .Select(f => new
            {
                id = (int)f,
                name = f.ToString()
            });

            return Ok(factions);
        }
    }
}
