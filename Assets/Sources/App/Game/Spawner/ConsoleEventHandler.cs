using UnityEngine;

public class ConsoleEventHandler : MonoBehaviour, IAgentEventsHandler {

    [SerializeField] private MapAgent _console;
    [SerializeField] private int _health;
    [SerializeField] private float _armorPenalty = .5f;
    
    private IStatsProvider _stats;
    private int _startHealth;

    private void Awake() {
        _startHealth = _health;
    }

    public void InitHandler(IStatsProvider stats) {
        _stats = stats;
        _console.SetAgentHandler(this);
        _health = _startHealth;

        UpdateHealth();
    }

    public void DisposeHandler() {}

    public bool ApplyDamage(MapAgent mapAgent, int damage) {
        _health -= (int)(damage * _armorPenalty);
        
        if (_health == 0) {
            mapAgent.KillAgent();
        }

        UpdateHealth();

        return _health == 0;
    }
    
    private void UpdateHealth() => _stats.ConsoleHealth.Value = _health / (float)_startHealth;
    
    public MapAgent NearestTarget(MapAgent agent) => null;
    
    public void Tick() {}
}
