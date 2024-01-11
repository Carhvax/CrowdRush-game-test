using Zenject;

public class GameFlow : ITickable {
    
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
        _handlers.Each(h => h.InitHandler());
    }

    public void Dispose() {
        _isActive = false;
        _handlers.Each(h => h.DisposeHandler());
    }

    public void Pause(bool state) => _isActive = !state;
}
