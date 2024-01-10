using UnityEngine;
using Zenject;

public class AppEntry : MonoBehaviour {
    
    private IStateProvider _states;

    [Inject]
    private void Construct(IStateProvider states, CommandFactory commandFactory, PresenterFactory presenterFactory, StateChangeHandler handler) {
        _states = states;
        
        handler
            .AddMap<BootScreenState>(presenterFactory.AddPresenter<BootStatePresenter>)
            .AddMap<MenuScreenState>((map) => {
                commandFactory.AddRouteMap<PlayGameButton, LoadingScreenState>(map);
                commandFactory.AddRouteMap<ShowSettingsButton, SettingsScreenState>(map);
            })
            .AddMap<SettingsScreenState>(commandFactory.AddRouteBack<ReturnBackButton>)
            .AddMap<LoadingScreenState>((map) => {
                
            })
            .AddMap<GameScreenState>((map) => {
                commandFactory.AddRouteMap<ShowPauseButton, PauseScreenState>(map);
            })
            .AddMap<UnLoadingScreenState>((map) => {
                
            })
            .AddMap<PauseScreenState>((map) => {
                commandFactory.AddRouteMap<ReturnMenuButton, UnLoadingScreenState>(map);
                commandFactory.AddRouteMap<RestartMenuButton, LoadingScreenState>(map);
            })
            .AddMap<CompleteScreenState>((map) => {
                commandFactory.AddRouteMap<ReturnMenuButton, UnLoadingScreenState>(map);
                commandFactory.AddRouteMap<NextGameButton, LoadingScreenState>(map);
            })
            .Complete();
    }
    
    private void Start() {
        _states.ChangeState<BootScreenState>();
    }
}
