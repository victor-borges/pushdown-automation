namespace PushdownAutomation
{
    public class TransitionRule
    {
        public int State { get; }
        public char? InputSymbol { get; }
        public char? StackSymbol { get; }
        public string? ReplaceSymbols { get; }
        public int ToState { get; }

        public TransitionRule(int state, char? inputSymbol, char? stackSymbol, string? replaceSymbols, int toState)
        {
            State = state;
            InputSymbol = inputSymbol;
            StackSymbol = stackSymbol;
            ReplaceSymbols = replaceSymbols;
            ToState = toState;
        }
    }
}
