using System;
using System.Text.Json.Serialization;

namespace ArmyOptimizer.Models
{
    public class Army
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int TownHall { get; set; }

        public List<Troop>? Troops { get; set; }
        public List<Spell>? Spells { get; set; }
        public List<string>? Heroes { get; set; }
        public List<HeroLoadout>? HeroLoadouts { get; set; }

        [JsonPropertyName("siegeMachines")]
        public List<SiegeMachine>? SiegeMachines { get; set; }

        [JsonPropertyName("aiNotes")]
        public string? AiNotes { get; set; }

    }
}
