using System;
using System.Collections.Generic;
using System.Linq;

namespace PushdownAutomation
{
    public class DeterministicPushdownAutomata
    {
        private readonly ISet<char?>? _inputAlphabet;
        private readonly ISet<char?>? _stackAlphabet;
        private readonly ISet<int> _states;
        private readonly ISet<TransitionRule>? _transitionRules;
        private readonly Stack<char?> _stack;

        private int _currentState;

        public DeterministicPushdownAutomata(
            ISet<char?>? inputAlphabet,
            ISet<char?>? stackAlphabet,
            ISet<int> states,
            int initialState,
            ISet<TransitionRule>? transitionRules,
            char? initialStackSymbol)
        {
            _inputAlphabet = inputAlphabet;
            _stackAlphabet = stackAlphabet;
            _states = states;
            _currentState = initialState;
            _transitionRules = transitionRules;
            _stack = new Stack<char?>(new[] { initialStackSymbol });
        }

        public bool Matches(string? input)
        {
            input = string.IsNullOrEmpty(input) ? null : input;

            var i = 0;

            while (!(_stack.Count == 0 && i == (input?.Length ?? 0)))
            {
                var inputSymbol = i < (input?.Length ?? 0) ? input?[i++] : null;

                if ((!_inputAlphabet?.Contains(inputSymbol) ?? true) && i != (input?.Length ?? 0))
                {
                    return false;
                }

                if (!_stack.TryPeek(out var stackSymbol))
                {
                    stackSymbol = null;
                }

                var possibleTransitions =
                    from rule in _transitionRules
                    where rule.State == _currentState
                    where rule.InputSymbol == inputSymbol
                    where rule.StackSymbol == stackSymbol
                    select rule;

                var count = possibleTransitions.Count();

                if (count > 1)
                {
                    throw new NotSupportedException();
                }
                else if (count == 0)
                {
                    return false;
                }
                else if (count == 1)
                {
                    var transition = possibleTransitions.Single();

                    if (!_states.Contains(transition.ToState))
                    {
                        return false;
                    }

                    _currentState = transition.ToState;

                    if (transition.ReplaceSymbols == null)
                    {
                        if (stackSymbol != null)
                        {
                            _ = _stack.Pop();
                        }
                    }
                    else
                    {
                        _ = _stack.Pop();

                        foreach (var symbol in transition.ReplaceSymbols)
                        {
                            if (!_stackAlphabet?.Contains(symbol) ?? true)
                            {
                                return false;
                            }

                            _stack.Push(symbol);
                        }
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
    }
}
