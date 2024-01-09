using System;
using System.Collections.Generic;
using System.Linq;

public interface IStateObserver {
    void EnterState(ScreenState state);
    void ExitState(ScreenState state);
}

public interface IStateMap {
    IStateMap AddObserver(IStateObserver observer);
    void Complete();
}

public class StateChangeHandler : IDisposable {
    private readonly Dictionary<Type, IStateObserver[]> _observers = new();
    private readonly IStateObservable _observable;
    private readonly HashSet<Action> _maps = new();
    
    public StateChangeHandler(IStateObservable observable) {
        _observable = observable;
        
        _observable.StateExit += OnStateExited;
        _observable.StateEnter += OnStateEntered;
    }

    public StateChangeHandler AddMap<TState>(Action<IStateMap> onMap) where TState : IScreenState {
        var stateMap = new StateMap<TState>(this);
        _maps.Add(() => stateMap.Complete());
        onMap?.Invoke(stateMap);
        return this;
    }
    
    private void OnStateEntered(IScreenState state) {
        if (_observers.TryGetValue(state.GetType(), out var observers))
            observers.Each(o => o.EnterState(state as ScreenState));
    }
    
    private void OnStateExited(IScreenState state) {
        if (_observers.TryGetValue(state.GetType(), out var observers))
            observers.Each(o => o.ExitState(state as ScreenState));
    }

    private void AddMapItems(Type type, IStateObserver[] observers) {
        _observers.TryAdd(type, observers);
    }

    public void Complete() => _maps.Each(m => m?.Invoke());

    public void Dispose() {
        _observable.StateExit -= OnStateExited;
        _observable.StateEnter -= OnStateEntered;
    }

    private class StateMap<T> : IStateMap where T : IScreenState {
        
        private readonly HashSet<IStateObserver> _observers = new();
        private readonly StateChangeHandler _handler;
        public StateMap(StateChangeHandler handler) {
            _handler = handler;
        }

        public IStateMap AddObserver(IStateObserver observer) {
            _observers.Add(observer);
            return this;
        }
        
        public void Complete() {
            _handler.AddMapItems(typeof(T), _observers.ToArray());
        }
    }
}
