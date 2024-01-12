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
    private MapAgent _console;

    public void InitHandler(IStatsProvider stats) {
        _console = _targets.OfType<CommandConsole>().FirstOrDefault();
        _stats = stats;
        _tree = new BehaviourTree(this);
        
        _spawners.Each(s => {
            s.AgentSpawned += OnAgentWasSpawn;
            s.AgentKilled += OnAgentWasKilled;
            
            s.Init();
        });
        
        OnAgentCountChanged();
    }
    
    private void OnAgentWasKilled(MapAgent agent) {
        _instances.Remove(agent);
        
        OnAgentCountChanged();
    }

    public void DisposeHandler() => _spawners.Each(s => {
        s.ReturnPool();
        
        _instances.Clear();
        
        s.AgentSpawned -= OnAgentWasSpawn;
        s.AgentKilled -= OnAgentWasKilled;
    });
    
    private void OnAgentCountChanged() {
        var mobs = _spawners.Sum(s => s.Remain);
        
        _stats.MobsCount.Value = mobs;
        
        if(mobs == 0) {
            // TODO: Simple end game
            _stats.CompleteGame(true);
        }
    }

    private void OnAgentWasSpawn(MapAgent agent) {
        if (agent is NightWalker walker) {
            walker.SetAgentHandler(this);
            walker.UpdateHealth(1);
            // TODO: Replace with mob factory
            _instances.Add(walker, new MobData(health: 10, sight: 5,  damage: 5, speed:1, target: _console));
        }
    }

    public void Tick() {
        _spawners.Each(s => s.Tick());
        _instances.Each(i => _tree.Execute(i.Key, i.Value));
    }


    public MapAgent NearestTarget(MapAgent agent) {
        var data = _instances[agent];

        var closestTarget = _targets
            .Where(t => t.IsActive && GetDistance(t) <= data.SightRadius)
            .OrderBy(GetDistance)
            .FirstOrDefault();
        
        return closestTarget != null ? closestTarget : _console;
        
        float GetDistance(MapAgent target) => target.GetDirectionToContact(agent.transform.position).magnitude; 
    }

    public bool ApplyDamage(MapAgent mapAgent, int damage) {
        if (_instances.TryGetValue(mapAgent, out var data)) {
            var died = data.ApplyDamage(damage);

            if (died) {
                
                
                mapAgent.KillAgent();
            }
            
            mapAgent.UpdateHealth(data.HealthAmount);

            return died;
        }

        return false;
    }
}
