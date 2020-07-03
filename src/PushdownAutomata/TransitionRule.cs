using System.Text.Json.Serialization;

namespace PushdownAutomation
{
    public class TransitionRule
    {
        [JsonPropertyName("from_state")]
        public int State { get; set; }

        [JsonPropertyName("input")]
        public char? InputSymbol { get; set; }

        [JsonPropertyName("stack")]
        public char? StackSymbol { get; set; }

        [JsonPropertyName("replace")]
        public string? ReplaceSymbols { get; set; }

        [JsonPropertyName("to_state")]
        public int ToState { get; set; }
    }
}
