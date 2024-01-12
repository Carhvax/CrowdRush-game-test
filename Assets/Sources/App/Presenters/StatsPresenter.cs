public class StatsPresenter : IStatePresenter {
    private readonly IStatsProvider _stats;
    
    private readonly IMenuCommand _upgradeCommand;
    
    private StatsPresenterView _view;

    public StatsPresenter(CommandFactory factory, IStatsProvider stats) {
        _stats = stats;
        _upgradeCommand = factory.CreateRoute<UpgradeScreenState>();
    }

    public void EnterState(ScreenState state) {
        if (state.OnResolvePresenterView<StatsPresenterView>(out var view)) {
            _view = view;
            
            _stats.ConsoleHealth.Changed += OnConsoleHealthChanged;    
            _stats.PlayerHealth.Changed += OnPlayerHealthChanged;
            _stats.MobsCount.Changed += OnMobsCountChanged;
            _stats.Level.Changed += OnPlayerLevelChanged;
        }
    }
    
    private void OnPlayerLevelChanged(int obj) => _upgradeCommand.Execute();

    private void OnMobsCountChanged(int amount) => _view.UpdateMobsRemain(amount);

    private void OnPlayerHealthChanged(float amount) => _view.UpdatePlayerHealth(amount);
    
    private void OnConsoleHealthChanged(float amount) => _view.UpdateConsoleHealth(amount);

    public void ExitState(ScreenState state) {
        if (state.OnResolvePresenterView<StatsPresenterView>(out var view)) {
            _stats.ConsoleHealth.Changed -= OnConsoleHealthChanged;    
            _stats.PlayerHealth.Changed -= OnPlayerHealthChanged;    
            _stats.MobsCount.Changed -= OnMobsCountChanged;    
            _stats.Level.Changed -= OnPlayerLevelChanged;
        }
    }
}