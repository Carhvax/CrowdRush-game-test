using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class MobSpawner : SpawnPool<NightWalker> {
    
    [SerializeField] private float _spawnRadius = 5f;
    [SerializeField] private int _agentsAtOnce = 50;
    [SerializeField] private int _agentsTotal = 50;
    
    [Space]
    [SerializeField] private Transform[] _points;
    
    private int _killed;
    private float _timer;

    private int AgentsLimit => Mathf.Min(_agentsAtOnce, _agentsTotal - _killed);
    public int Remain => _agentsTotal - _killed;

    public event Action AgentsCountChanged;
    public event Action<MapAgent> AgentSpawned;
    public event Action<MapAgent> AgentKilled;

    public void Init() {
        Initialize();

        _killed = 0;
    }

    public void Tick() {
        if (ActiveInstances >= AgentsLimit || (_timer -= Time.deltaTime) > 0) return;
        
        SpawnAgent(_points.GetRandom());

        _timer = .25f;
    }

    private void SpawnAgent(Transform spawnPoint) {
        var point = Random.insideUnitCircle * _spawnRadius;
        var position = spawnPoint.position + new Vector3(point.x, 0, point.y);
        
        var agent = GetFromPool(position);
        agent.Die += OnAgentWasKilled;
        
        AgentSpawned?.Invoke(agent);
    }
    
    private void OnAgentWasKilled(MapAgent agent) {
        agent.Die -= OnAgentWasKilled;
        
        _killed++;
        
        Return(agent as NightWalker);
        
        AgentsCountChanged?.Invoke();
        AgentKilled?.Invoke(agent);
    }
}