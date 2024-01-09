using System;
using System.Collections.Generic;
using System.Linq;

public interface IState {
    void Enter();
    void Exit();
}

public interface IStateProvider {
    bool HasHistory { get; }
    bool CurrentStateIs<T>() where T : class, IState;
    void ChangeState<T>() where T : class, IState;
    void Back();
}

public interface IStateObservable {
    event Action<IState> StateExit;
    event Action<IState> StateEnter;
}

namespace ScreenStates {
    
    public class StateMachine<TState> : IStateProvider, IStateObservable where TState : class, IState {

        private readonly HashSet<TState> _states = new();
        private readonly Stack<TState> _history = new();

        private TState _current;
        public event Action<IState> StateExit;
        public event Action<IState> StateEnter;
        public bool HasHistory => _history.Count > 0;
        public bool CurrentStateIs<T>() where T : class, IState => _current is T;

        protected void AddState(TState state) {
            _states.Add(state);
        }

        public void ChangeState<T>() where T : class, IState {
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
                _current.Exit();
                StateExit?.Invoke(_current);
            }
            
            _current = state;
            
            if (_current != null) {
                _current.Enter();
                StateEnter?.Invoke(_current);
            }
        }
    }

    internal static class ScreenStatesExtensions {}
}


