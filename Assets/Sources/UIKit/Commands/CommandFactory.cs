using System;

public class CommandFactory {

    private readonly IStateProvider _provider;

    public CommandFactory(IStateProvider provider) {
        _provider = provider;
    }
    
    public IMenuCommand CreateRoute<TButton, TState>(params Action[] actions) where TButton : class, ILayoutButton where TState : class, IScreenState {
        return CreateTrigger<TButton>(() => _provider.ChangeState<TState>());
    }
    
    public IMenuCommand CreateRoute<TState>(params Action[] actions) where TState : class, IScreenState {
        return CreateCommand(() => {
            actions.Each(a => a?.Invoke());
            _provider.ChangeState<TState>();
        });
    }
    
    public IMenuCommand CreateRouteBack<TButton>(params Action[] actions) where TButton : class, ILayoutButton {
        return CreateTrigger<TButton>(() => _provider.Back());
    }
    
    public IMenuCommand CreateTrigger<TButton>(params Action[] actions) where TButton : class, ILayoutButton {
        return new RouteCommand<TButton>().OnExecute(() => {
            actions.Each(a => a?.Invoke());
        });
    }
    
    public IMenuCommand CreateCommand(params Action[] actions) {
        return new MenuCommand().OnExecute(() => {
            actions.Each(a => a?.Invoke());
        });
    }
}

public static class CommandsExtensions {

    public static void AddRouteMap<TButton, TState>(this CommandFactory factory, IStateMap map) where TButton : class, ILayoutButton where TState : class, IScreenState {
        map.AddObserver(factory.CreateRoute<TButton, TState>(null) as IStateObserver);
    }
    
    public static void AddRouteBack<TButton>(this CommandFactory factory, IStateMap map) where TButton : class, ILayoutButton {
        map.AddObserver(factory.CreateRouteBack<TButton>(null)  as IStateObserver);
    }
    
    public static void AddRouteTrigger<TButton>(this CommandFactory factory, IStateMap map, params Action[] actions) where TButton : class, ILayoutButton {
        map.AddObserver(factory.CreateTrigger<TButton>(actions)  as IStateObserver);
    }
    
}