using NUnit.Framework;
using System.Collections.Generic;
using DPDA = PushdownAutomation.DeterministicPushdownAutomata;

namespace PushdownAutomation.Tests
{
    [TestFixture]
    public class Tests
    {
        private const char BaseSymbol = 'β';

        /// <summary>
        /// Matches the following language: <code>{ a²ⁿbⁿc | n >= 0}</code>
        /// </summary>
        [TestCase(null, ExpectedResult = false)]
        [TestCase("", ExpectedResult = false)]
        [TestCase(" ", ExpectedResult = false)]
        [TestCase("\n", ExpectedResult = false)]
        [TestCase("aabc", ExpectedResult = true)]
        [TestCase("aaaabbc", ExpectedResult = true)]
        [TestCase("aaaaaabbbc", ExpectedResult = true)]
        [TestCase("c", ExpectedResult = true)]
        [TestCase("ab", ExpectedResult = false)]
        [TestCase("a", ExpectedResult = false)]
        [TestCase("b", ExpectedResult = false)]
        [TestCase("bbac", ExpectedResult = false)]
        [TestCase("aabbc", ExpectedResult = false)]
        [TestCase("aabcc", ExpectedResult = false)]
        [TestCase("aaaaa", ExpectedResult = false)]
        [TestCase("bbbbb", ExpectedResult = false)]
        [TestCase("ccccc", ExpectedResult = false)]
        public bool HalfAmountOfBsThanAsAndC_WithoutNull(string input)
        {
            var inputAlphabet = new HashSet<char?> { 'a', 'b', 'c' };
            var stackAlphabet = new HashSet<char?> { 'A', 'N' };
            var states = new HashSet<int> { 0, 1, 2, 3 };
            var initialState = 0;

            var transitionRules = new HashSet<TransitionRule>
                {
                    new TransitionRule(0, 'a', BaseSymbol, "A", 2),
                    new TransitionRule(0, 'c', BaseSymbol, null, 1),
                    new TransitionRule(2, 'a', 'A', "N", 2),
                    new TransitionRule(2, 'a', 'N', "NA", 2),
                    new TransitionRule(2, 'b', 'N', null, 3),
                    new TransitionRule(3, 'b', 'N', null, 3),
                    new TransitionRule(3, 'c', null, null, 1)
                };

            var automata = new DPDA(inputAlphabet, stackAlphabet, states, initialState, transitionRules, BaseSymbol);
            
            return automata.Matches(input);
        }

        /// <summary>
        /// Matches the following language: <code>{ aⁿbⁿ | n >= 0}</code>
        /// </summary>
        [TestCase(null, ExpectedResult = true)]
        [TestCase("", ExpectedResult = true)]
        [TestCase(" ", ExpectedResult = false)]
        [TestCase("\n", ExpectedResult = false)]
        [TestCase("ab", ExpectedResult = true)]
        [TestCase("aabb", ExpectedResult = true)]
        [TestCase("aaabbb", ExpectedResult = true)]
        [TestCase("a", ExpectedResult = false)]
        [TestCase("b", ExpectedResult = false)]
        [TestCase("aab", ExpectedResult = false)]
        [TestCase("abb", ExpectedResult = false)]
        [TestCase("ba", ExpectedResult = false)]
        [TestCase("baba", ExpectedResult = false)]
        [TestCase("bbb", ExpectedResult = false)]
        [TestCase("aaa", ExpectedResult = false)]
        [TestCase("qwerty", ExpectedResult = false)]
        public bool SameAmountOfAsAndBs_WithNull(string input)
        {
            var inputAlphabet = new HashSet<char?> { null, 'a', 'b' };
            var stackAlphabet = new HashSet<char?> { 'A' };
            var states = new HashSet<int> { 0, 1 };
            var initialState = 0;

            var transitionRules = new HashSet<TransitionRule>
                {
                    new TransitionRule(0, 'a', BaseSymbol, "A", 0),
                    new TransitionRule(0, null, BaseSymbol, null, 0),
                    new TransitionRule(0, 'a', 'A', "AA", 0),
                    new TransitionRule(0, 'b', 'A', null, 1),
                    new TransitionRule(1, 'b', 'A', null, 1)
                };

            var automata = new DPDA(inputAlphabet, stackAlphabet, states, initialState, transitionRules, BaseSymbol);

            return automata.Matches(input);
        }
    }
}
