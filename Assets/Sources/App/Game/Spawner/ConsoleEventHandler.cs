using UnityEngine;

public class ConsoleEventHandler : MonoBehaviour, IAgentEventsHandler {

    [SerializeField] private MapAgent _console;
    [SerializeField] private int _health;
    [SerializeField] private float _armorPenalty = .5f;
    
    public void InitHandler() => _console.SetAgentHandler(this);

    public void DisposeHandler() {}

    public void ApplyDamage(MapAgent mapAgent, int damage) {
        _health -= (int)(damage * _armorPenalty);
        
        if (_health == 0) {
            mapAgent.KillAgent();
        }
    }
    
    public MapAgent NearestTarget(MapAgent agent) => null;
    
    public void Tick() {}
}
