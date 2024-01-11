using UnityEngine;

public class ConsoleEventHandler : MonoBehaviour, IAgentEventsHandler {

    [SerializeField] private MapAgent _console;

    private void Awake() {
        _console.SetAgentHandler(this);
    }

    public void ApplyDamage(MapAgent mapAgent, int damage) {
        
    }
    
    public MapAgent NearestTarget(MapAgent agent) {
        return agent;
    }
}
