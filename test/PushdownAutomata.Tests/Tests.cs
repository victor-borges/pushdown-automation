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
        public bool A2nBnC_NotNull(string input)
        {
            var inputAlphabet = new HashSet<char?> { 'a', 'b', 'c' };
            var stackAlphabet = new HashSet<char?> { 'A', 'N', BaseSymbol };
            var states = new HashSet<int> { 0, 1, 2, 3 };
            var initialState = 0;

            var transitionRules = new HashSet<TransitionRule>
                {
                    new TransitionRule{ State = 0, InputSymbol = 'a', StackSymbol = BaseSymbol, ReplaceSymbols = "A", ToState = 2 },
                    new TransitionRule{ State = 0, InputSymbol = 'c', StackSymbol = BaseSymbol, ReplaceSymbols = null, ToState = 1 },
                    new TransitionRule{ State = 2, InputSymbol = 'a', StackSymbol = 'A', ReplaceSymbols = "N", ToState = 2 },
                    new TransitionRule{ State = 2, InputSymbol = 'a', StackSymbol = 'N', ReplaceSymbols = "NA", ToState = 2 },
                    new TransitionRule{ State = 2, InputSymbol = 'b', StackSymbol = 'N', ReplaceSymbols = null, ToState = 3 },
                    new TransitionRule{ State = 3, InputSymbol = 'b', StackSymbol = 'N', ReplaceSymbols = null, ToState = 3 },
                    new TransitionRule{ State = 3, InputSymbol = 'c', StackSymbol = null, ReplaceSymbols =  null, ToState = 1 }
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
        public bool AnBn_Null(string input)
        {
            var inputAlphabet = new HashSet<char?> { null, 'a', 'b' };
            var stackAlphabet = new HashSet<char?> { 'A', BaseSymbol };
            var states = new HashSet<int> { 0, 1 };
            var initialState = 0;

            var transitionRules = new HashSet<TransitionRule>
                {
                    new TransitionRule{ State = 0, InputSymbol = 'a', StackSymbol = BaseSymbol, ReplaceSymbols = "A", ToState = 0 },
                    new TransitionRule{ State = 0, InputSymbol = null, StackSymbol = BaseSymbol,
                        ReplaceSymbols = null, ToState = 0 },
                    new TransitionRule{ State = 0, InputSymbol = 'a', StackSymbol = 'A', ReplaceSymbols = "AA", ToState = 0 },
                    new TransitionRule{ State = 0, InputSymbol = 'b', StackSymbol = 'A', ReplaceSymbols = null, ToState = 1},
                    new TransitionRule{ State = 1, InputSymbol = 'b', StackSymbol = 'A', ReplaceSymbols = null,
                        ToState = 1 }
                };

            var automata = new DPDA(inputAlphabet, stackAlphabet, states, initialState, transitionRules, BaseSymbol);

            return automata.Matches(input);
        }

        /// <summary>
        /// Matches the following language: <code>{x ∈ {a,e}* | |xₐ| = |xₑ|}</code>
        /// </summary>
        [TestCase(null, ExpectedResult = true)]
        [TestCase("", ExpectedResult = true)]
        [TestCase(" ", ExpectedResult = false)]
        [TestCase("\n", ExpectedResult = false)]
        [TestCase("ae", ExpectedResult = true)]
        [TestCase("aaee", ExpectedResult = true)]
        [TestCase("eeeaaa", ExpectedResult = true)]
        [TestCase("eaeaeaaaaeeeaeae", ExpectedResult = true)]
        [TestCase("ea", ExpectedResult = true)]
        [TestCase("eeeaaaeeeaaaeaeaaeaeeeeeaeaaeeaaaa", ExpectedResult = true)]
        [TestCase("eaeeeaaa", ExpectedResult = true)]
        [TestCase("eeaeeaeeaaaa", ExpectedResult = true)]
        [TestCase("aaeaaeaaeeee", ExpectedResult = true)]
        [TestCase("a", ExpectedResult = false)]
        [TestCase("e", ExpectedResult = false)]
        [TestCase("eae", ExpectedResult = false)]
        [TestCase("eae", ExpectedResult = false)]
        [TestCase("aaaa", ExpectedResult = false)]
        [TestCase("eeee", ExpectedResult = false)]
        [TestCase("aaaaaaeeee", ExpectedResult = false)]
        [TestCase("eaeeaaa", ExpectedResult = false)]
        [TestCase("oirh", ExpectedResult = false)]
        [TestCase("00124", ExpectedResult = false)]
        [TestCase("aa12312eee", ExpectedResult = false)]
        [TestCase("eaa asd", ExpectedResult = false)]
        public bool SameAmountOfAsAndEs_Null(string input)
        {
            var inputAlphabet = new HashSet<char?> { null, 'a', 'e' };
            var stackAlphabet = new HashSet<char?> { 'A', 'E', BaseSymbol };
            var states = new HashSet<int> { 0 };
            var initialState = 0;

            var transitionRules = new HashSet<TransitionRule>
                {
                    new TransitionRule { State = 0, InputSymbol = 'a', StackSymbol = BaseSymbol, ReplaceSymbols = BaseSymbol + "A", ToState = 0 },
                    new TransitionRule { State = 0, InputSymbol = 'e', StackSymbol = BaseSymbol, ReplaceSymbols = BaseSymbol + "E", ToState = 0 },
                    new TransitionRule { State = 0, InputSymbol = null, StackSymbol = BaseSymbol, ReplaceSymbols = null, ToState = 0 },
                    new TransitionRule { State = 0, InputSymbol = 'a', StackSymbol = 'A', ReplaceSymbols = "AA", ToState = 0 },
                    new TransitionRule { State = 0, InputSymbol = 'e', StackSymbol = 'A', ReplaceSymbols = null, ToState = 0 },
                    new TransitionRule { State = 0, InputSymbol = 'a', StackSymbol = 'E', ReplaceSymbols = null, ToState = 0 },
                    new TransitionRule { State = 0, InputSymbol = 'e', StackSymbol = 'E', ReplaceSymbols = "EE", ToState = 0 }
                };

            var automata = new DPDA(inputAlphabet, stackAlphabet, states, initialState, transitionRules, BaseSymbol);

            return automata.Matches(input);
        }

        /// <summary>
        /// Matches the following language: <code>{ aⁿbᵐaⁿ | n >= 0, m > 0}</code>
        /// </summary>
        [TestCase(null, ExpectedResult = false)]
        [TestCase("", ExpectedResult = false)]
        [TestCase(" ", ExpectedResult = false)]
        [TestCase("\n", ExpectedResult = false)]
        [TestCase("aba", ExpectedResult = true)]
        [TestCase("aabbaa", ExpectedResult = true)]
        [TestCase("aaabaaa", ExpectedResult = true)]
        [TestCase("aaabbaaa", ExpectedResult = true)]
        [TestCase("aaabbbbbaaa", ExpectedResult = true)]
        [TestCase("b", ExpectedResult = true)]
        [TestCase("bbb", ExpectedResult = true)]
        [TestCase("aaaabaaaa", ExpectedResult = true)]
        [TestCase("aaaaaa", ExpectedResult = false)]
        [TestCase("aaabaa", ExpectedResult = false)]
        [TestCase("aabbaaa", ExpectedResult = false)]
        [TestCase("a", ExpectedResult = false)]
        [TestCase("aa", ExpectedResult = false)]
        [TestCase("qwerty", ExpectedResult = false)]
        public bool AnBmAn_NotNull(string input)
        {
            var inputAlphabet = new HashSet<char?> { 'a', 'b' };
            var stackAlphabet = new HashSet<char?> { 'A', BaseSymbol };
            var states = new HashSet<int> { 0, 1, 2 };
            var initialState = 0;

            var transitionRules = new HashSet<TransitionRule>
                {
                    new TransitionRule { State = 0, InputSymbol = 'a', StackSymbol = BaseSymbol, ReplaceSymbols = "A", ToState = 0 },
                    new TransitionRule { State = 0, InputSymbol = 'a', StackSymbol = 'A', ReplaceSymbols = "AA", ToState = 0 },
                    new TransitionRule { State = 0, InputSymbol = 'b', StackSymbol = BaseSymbol,
                        ReplaceSymbols = $"{BaseSymbol}",
                        ToState = 1 },
                    new TransitionRule { State = 0, InputSymbol = 'b', StackSymbol = 'A',
                        ReplaceSymbols = "A",
                        ToState = 1 },
                    new TransitionRule { State = 1, InputSymbol = null, StackSymbol = BaseSymbol,
                        ReplaceSymbols = null,
                        ToState = 1 },
                    new TransitionRule { State = 1, InputSymbol = 'b', StackSymbol = BaseSymbol,
                        ReplaceSymbols = $"{BaseSymbol}",
                        ToState = 1 },
                    new TransitionRule { State = 1, InputSymbol = 'b', StackSymbol = 'A',
                        ReplaceSymbols = "A",
                        ToState = 1 },
                    new TransitionRule { State = 1, InputSymbol = 'a', StackSymbol = 'A',
                        ReplaceSymbols = null,
                        ToState = 2 },
                    new TransitionRule { State = 2, InputSymbol = 'a', StackSymbol = 'A',
                        ReplaceSymbols = null,
                        ToState = 2 },
                };

            var automata = new DPDA(inputAlphabet, stackAlphabet, states, initialState, transitionRules, BaseSymbol);

            return automata.Matches(input);
        }

        /// <summary>
        /// Matches the following language: <code>{ aⁿb²ⁿ | n >= 0}</code>
        /// </summary>
        [TestCase(null, ExpectedResult = true)]
        [TestCase("", ExpectedResult = true)]
        [TestCase(" ", ExpectedResult = false)]
        [TestCase("\n", ExpectedResult = false)]
        [TestCase("abb", ExpectedResult = true)]
        [TestCase("aabbbb", ExpectedResult = true)]
        [TestCase("aaabbbbbb", ExpectedResult = true)]
        [TestCase("a", ExpectedResult = false)]
        [TestCase("bb", ExpectedResult = false)]
        [TestCase("aab", ExpectedResult = false)]
        [TestCase("abbb", ExpectedResult = false)]
        [TestCase("ba", ExpectedResult = false)]
        [TestCase("baba", ExpectedResult = false)]
        [TestCase("bbb", ExpectedResult = false)]
        [TestCase("aaa", ExpectedResult = false)]
        [TestCase("qwerty", ExpectedResult = false)]
        public bool AnB2n_Null(string input)
        {
            var inputAlphabet = new HashSet<char?> { null, 'a', 'b' };
            var stackAlphabet = new HashSet<char?> { 'A', BaseSymbol };
            var states = new HashSet<int> { 0, 1 };
            var initialState = 0;

            var transitionRules = new HashSet<TransitionRule>
                {
                    new TransitionRule{ State = 0, InputSymbol = 'a', StackSymbol = BaseSymbol, ReplaceSymbols = "AA", ToState = 0 },
                    new TransitionRule{ State = 0, InputSymbol = 'a', StackSymbol = 'A',
                        ReplaceSymbols = "AAA",
                        ToState = 0 },
                    new TransitionRule{ State = 0, InputSymbol = null, StackSymbol = BaseSymbol,
                        ReplaceSymbols = null,
                        ToState = 0 },
                    new TransitionRule{ State = 0, InputSymbol = 'b', StackSymbol = 'A',
                        ReplaceSymbols = null,
                        ToState = 1 },
                    new TransitionRule{ State = 1, InputSymbol = 'b', StackSymbol = 'A',
                        ReplaceSymbols = null,
                        ToState = 1 }
                };

            var automata = new DPDA(inputAlphabet, stackAlphabet, states, initialState, transitionRules, BaseSymbol);

            return automata.Matches(input);
        }
    }
}
