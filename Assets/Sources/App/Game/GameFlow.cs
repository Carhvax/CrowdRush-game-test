using System.Linq;
using Zenject;

public class GameFlow : IStatsProvider, ITickable {

    public IObservableValue<float> PlayerHealth { get; } = new ObservableValue<float>(1);
    public IObservableValue<float> ConsoleHealth { get; } = new ObservableValue<float>(1);
    public IObservableValue<int> MobsCount { get; }  = new ObservableValue<int>(50);
    public IObservableValue<int> Level { get; }  = new ObservableValue<int>(notify: false);
    
    private readonly IAgentEventsHandler[] _handlers;
    private bool _isActive;

    public GameFlow(IAgentEventsHandler[] handlers) {
        _handlers = handlers;
    }

    public void Tick() {
        if (!_isActive) return;

        _handlers.Each(h => h.Tick());
    }

    public void Create() {
        _isActive = true;
        
        PlayerHealth.Value = 1f;
        ConsoleHealth.Value = 1f;
        MobsCount.Value = 0;
        Level.Value = 0;
        
        _handlers.Each(h => h.InitHandler(this));
    }

    public void Dispose() {
        _isActive = false;
        _handlers.Each(h => h.DisposeHandler());
    }

    public void Pause(bool state) => _isActive = !state;

    public void ApplyAbility(IAbility ability) {
        var handler = _handlers.OfType<PlayerEventHandler>().FirstOrDefault();
        
        if(handler != null)
            handler.Use(ability);
    }
}
