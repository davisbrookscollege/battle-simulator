using battlesimulation.api.Models;
using System.Text.Json.Serialization;

namespace battlesimulation.api.Dtos
{
    public class CreateTransformerRequest
    {
        public string? Name { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Faction Faction { get; set; }
        public string? AltVehicle { get; set; }
        public int Strength { get; set; }
        public int Intelligence { get; set; }
    }
}
