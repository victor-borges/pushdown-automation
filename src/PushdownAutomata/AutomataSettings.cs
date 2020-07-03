using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace PushdownAutomation
{
    public class AutomataSettings
    {
        [JsonPropertyName("pushdown_automata")]
        public PushdownAutomata? PushdownAutomata { get; set; }

        [JsonPropertyName("input")]
        [SuppressMessage("Usage", "CA2227", Justification = "Used by JsonSerializer.")]
        public ISet<string?>? Inputs { get; set; } = new HashSet<string?>();
    }

    public class PushdownAutomata
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [SuppressMessage("Usage", "CA2227", Justification = "Used by JsonSerializer.")]
        [JsonPropertyName("input_alphabet")]
        public ISet<char?>? InputAlphabet { get; set; }

        [SuppressMessage("Usage", "CA2227", Justification = "Used by JsonSerializer.")]
        [JsonPropertyName("stack_alphabet")]
        public ISet<char?>? StackAlphabet { get; set; }

        [SuppressMessage("Usage", "CA2227", Justification = "Used by JsonSerializer.")]
        [JsonPropertyName("states")]
        public ISet<int>? States { get; set; }

        [JsonPropertyName("initial_state")]
        public int? InitialState { get; set; }

        [JsonPropertyName("initial_stack_symbol")]
        public char? InitialStackSymbol { get; set; }

        [SuppressMessage("Usage", "CA2227", Justification = "Used by JsonSerializer.")]
        [JsonPropertyName("transition_rules")]
        public ISet<TransitionRule?>? TransitionRules { get; set; }
    }
}
