using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MobEventHandler : MonoBehaviour, IAgentEventsHandler {

    [SerializeField] private MobSpawner[] _spawners;
    
    [Space]
    [SerializeField] private MapAgent[] _targets;
    
    private readonly Dictionary<MapAgent, MobData> _instances = new();

    private void Awake() {

    }

    private void OnEnable() {
        _spawners.Each(s => s.AgentSpawned += OnAgentWasSpawn);
    }
    
    private void OnAgentWasSpawn(MapAgent agent) {
        if (agent is NightWalker walker) {
            walker.SetAgentHandler(this);
            walker.UpdateHealth(1);
            
            _instances.Add(walker, new MobData(health: 10, sight: 5, target: _targets.OfType<CommandConsole>().FirstOrDefault()));
        }
    }

    private void FixedUpdate() {

    }

    private void OnDisable() {
        _spawners.Each(s => s.AgentSpawned -= OnAgentWasSpawn);
    }

    public MapAgent NearestTarget(MapAgent agent) {
        var data = _instances[agent];

        return _targets
            .Where(t => GetDistance(t) <= data.SightRadius)
            .OrderBy(GetDistance)
            .FirstOrDefault();

        float GetDistance(MapAgent target) => (target.transform.position - agent.transform.position).magnitude; 
    }

    public void ApplyDamage(MapAgent mapAgent, int damage) {
        if (_instances.TryGetValue(mapAgent, out var data)) {
            var died = data.ApplyDamage(damage);

            if (died) {
                _instances.Remove(mapAgent);
                mapAgent.KillAgent();
            }
            
            mapAgent.UpdateHealth(data.HealthAmount);
        }
    }
}
