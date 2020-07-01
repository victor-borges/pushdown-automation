namespace PushdownAutomation
{
    public struct AutomataContext
    {
        public int State { get; set; }
        public char? InputSymbol { get; set; }
        public char? StackSymbol { get; set; }
    }
}
