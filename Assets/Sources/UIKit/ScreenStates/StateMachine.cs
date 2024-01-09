using System;
using System.Collections.Generic;
using System.Linq;

public interface IScreenState {
    void Show();
    void Hide();
}

public interface IStateProvider {
    bool HasHistory { get; }
    bool CurrentStateIs<T>() where T : class, IScreenState;
    void ChangeState<T>() where T : class, IScreenState;
    void Back();
}

public interface IStateObservable {
    event Action<IScreenState> StateExit;
    event Action<IScreenState> StateEnter;
}

namespace ScreenStates {
    
    public class StateMachine<TState> : IStateProvider, IStateObservable where TState : class, IScreenState {

        private readonly HashSet<TState> _states = new();
        private readonly Stack<TState> _history = new();

        private TState _current;
        public event Action<IScreenState> StateExit;
        public event Action<IScreenState> StateEnter;
        public bool HasHistory => _history.Count > 0;
        public bool CurrentStateIs<T>() where T : class, IScreenState => _current is T;

        protected void AddState(TState state) {
            _states.Add(state);
        }

        public void ChangeState<T>() where T : class, IScreenState {
            var state = _states.OfType<T>().FirstOrDefault();
        
            if(_current != null) _history.Push(_current);

            ChangeState(state as TState);
        }

        public void Back() {
            if (!HasHistory) return;
        
            ChangeState(_history.Pop());
        }
    
        private void ChangeState(TState state) {
            if (_current != null) {
                _current.Hide();
                StateExit?.Invoke(_current);
            }
            
            _current = state;
            
            if (_current != null) {
                _current.Show();
                StateEnter?.Invoke(_current);
            }
        }
    }

    internal static class ScreenStatesExtensions {}
}


