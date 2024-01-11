public class StatsPresenter : IStatePresenter {
    private readonly IStatsProvider _stats;
    private StatsPresenterView _view;

    public StatsPresenter(IStatsProvider stats) => _stats = stats;

    public void EnterState(ScreenState state) {
        if (state.OnResolvePresenterView<StatsPresenterView>(out var view)) {
            _view = view;
            
            _stats.ConsoleHealth.Changed += OnConsoleHealthChanged;    
            _stats.PlayerHealth.Changed += OnPlayerHealthChanged;    
            _stats.MobsCount.Changed += OnMobsCountChanged;    
        }
    }
    
    private void OnMobsCountChanged(int amount) => _view.UpdateMobsRemain(amount);

    private void OnPlayerHealthChanged(float amount) => _view.UpdatePlayerHealth(amount);
    
    private void OnConsoleHealthChanged(float amount) => _view.UpdateConsoleHealth(amount);

    public void ExitState(ScreenState state) {
        if (state.OnResolvePresenterView<StatsPresenterView>(out var view)) {
            _stats.ConsoleHealth.Changed -= OnConsoleHealthChanged;    
            _stats.PlayerHealth.Changed -= OnPlayerHealthChanged;    
            _stats.MobsCount.Changed -= OnMobsCountChanged;    
        }
    }
}
