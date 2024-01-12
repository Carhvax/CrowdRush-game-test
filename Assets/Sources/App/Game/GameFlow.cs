using System;
using System.Linq;
using UnityEngine;
using Zenject;

public class GameFlow : IStatsProvider, ITickable {

    public IObservableValue<float> PlayerHealth { get; } = new ObservableValue<float>(1);
    public IObservableValue<float> ConsoleHealth { get; } = new ObservableValue<float>(1);
    public IObservableValue<int> MobsCount { get; }  = new ObservableValue<int>(50);
    public IObservableValue<int> Level { get; }  = new ObservableValue<int>(notify: false);

    private readonly EffectsFactory _factory;
    private readonly IAgentEventsHandler[] _handlers;
    private bool _isActive;

    public event Action Complete;
    
    public GameFlow(EffectsFactory factory, IAgentEventsHandler[] handlers) {
        _factory = factory;
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

    public void CompleteGame(bool state) {
        Complete?.Invoke();
    }

    public void Dispose() {
        _factory.Dispose();
        _isActive = false;
        _handlers.Each(h => h.DisposeHandler());
    }

    public void Pause(bool state) {
        Time.timeScale = state? 0 : 1;
        _isActive = !state;
    }

    public void ApplyAbility(IAbility ability) {
        var handler = _handlers.OfType<PlayerEventHandler>().FirstOrDefault();
        
        if(handler != null)
            handler.Use(ability);
    }
}
