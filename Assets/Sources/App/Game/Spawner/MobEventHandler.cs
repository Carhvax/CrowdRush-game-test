using System.Collections.Generic;
using System.Linq;
using AI;
using UnityEngine;

public class MobEventHandler : MonoBehaviour, IAgentEventsHandler {

    [SerializeField] private MobSpawner[] _spawners;
    
    [Space]
    [SerializeField] private MapAgent[] _targets;
    
    private readonly Dictionary<MapAgent, MobData> _instances = new();
    private BehaviourTree _tree;
    private IStatsProvider _stats;


    public void InitHandler(IStatsProvider stats) {
        _stats = stats;
        _tree = new BehaviourTree(this);
        _spawners.Each(s => s.Init());
        
        OnAgentCountChanged();
    }

    public void DisposeHandler() => _spawners.Each(s => s.DestroyPool());
    
    private void OnEnable() => _spawners.Each(s => {
        s.AgentSpawned += OnAgentWasSpawn;
        s.AgentDied += OnAgentCountChanged;
    });
    
    private void OnAgentCountChanged() {
        var mobs = _spawners.Sum(s => s.Remain);
        
        _stats.MobsCount.Value = mobs;
    }

    private void OnAgentWasSpawn(MapAgent agent) {
        if (agent is NightWalker walker) {
            walker.SetAgentHandler(this);
            walker.UpdateHealth(1);
            
            _instances.Add(walker, new MobData(health: 10, sight: 5,  damage: 5, speed:1, target: _targets.OfType<CommandConsole>().FirstOrDefault()));
        }
    }

    public void Tick() {
        _spawners.Each(s => s.Tick());
        _instances.Each(i => _tree.Execute(i.Key, i.Value));
    }

    private void OnDisable() => _spawners.Each(s => {
        s.AgentSpawned -= OnAgentWasSpawn;
        s.AgentDied -= OnAgentCountChanged;
    });

    public MapAgent NearestTarget(MapAgent agent) {
        var data = _instances[agent];

        var closestTarget = _targets
            .Where(t => t.IsActive && GetDistance(t) <= data.SightRadius)
            .OrderBy(GetDistance)
            .FirstOrDefault();
        
        return closestTarget != null ? closestTarget : _targets.OfType<CommandConsole>().FirstOrDefault();
        
        float GetDistance(MapAgent target) => target.GetDirectionToContact(agent.transform.position).magnitude; 
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
