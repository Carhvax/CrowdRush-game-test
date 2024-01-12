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
                presenterFactory.AddPresenter<LoadingPresenter>(map);
            })
            .AddMap<RestartScreenState>((map) => {
                presenterFactory.AddPresenter<RestartPresenter>(map);
            })
            .AddMap<GameScreenState>((map) => {
                commandFactory.AddRouteMap<ShowPauseButton, PauseScreenState>(map);
                
                presenterFactory.AddPresenter<StatsPresenter>(map);
                presenterFactory.AddPresenter<FlowPresenter>(map);
            })
            .AddMap<UnLoadingScreenState>((map) => {
                presenterFactory.AddPresenter<UnloadingPresenter>(map);
            })
            .AddMap<PauseScreenState>((map) => {
                commandFactory.AddRouteMap<ReturnMenuButton, UnLoadingScreenState>(map);
                commandFactory.AddRouteMap<RestartMenuButton, RestartScreenState>(map);
                
                presenterFactory.AddPresenter<PausePresenter>(map);
            })
            .AddMap<UpgradeScreenState>((map) => {
                presenterFactory.AddPresenter<AbilityPresenter>(map);
            })
            .AddMap<CompleteScreenState>((map) => {
                commandFactory.AddRouteMap<ReturnMenuButton, UnLoadingScreenState>(map);
                commandFactory.AddRouteMap<NextGameButton, RestartScreenState>(map);
                
                presenterFactory.AddPresenter<PausePresenter>(map);
            })
            .Complete();
    }
    
    private void Start() {
        _states.ChangeState<BootScreenState>();
    }
}
