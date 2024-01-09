using System;

public interface IRouteCommand : IMenuCommand {
    IMenuCommand OnExecute(Action onExecute);
}

public class MenuCommand : IRouteCommand {
    private Action _onExecute;

    public IObservableValue<bool> State { get; } = new ObservableValue<bool>(true);
    public IMenuCommand OnExecute(Action onExecute) {
        _onExecute = onExecute;
        
        return this;
    }
    
    public void Execute() {
        if (State.Value) {
            _onExecute?.Invoke();
        }
    }
}

public class RouteCommand<TButton> : MenuCommand, IStateObserver where TButton : class, ILayoutButton {

    public void EnterState(ScreenState state) {
        state.OnButtonClick<TButton>(this);
    }
    
    public void ExitState(ScreenState state) {}
    
}
