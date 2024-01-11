using UnityEngine;

public class PlayerEventHandler : MonoBehaviour, IAgentEventsHandler {

    [SerializeField] private MapAgent _player;
    [SerializeField] private int _health = 50;
    
    private void Awake() {
        _player.SetAgentHandler(this);
    }
    
    public void ApplyDamage(MapAgent mapAgent, int damage) {
        Debug.Log($"Player damaged: {damage}");
        _health = Mathf.Clamp(_health - damage, 0, int.MaxValue);

        if (_health == 0) {
            mapAgent.KillAgent();
        }
    }
    
    public MapAgent NearestTarget(MapAgent agent) {
        return agent;
    }
}