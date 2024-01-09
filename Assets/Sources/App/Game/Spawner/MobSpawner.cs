using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class MobSpawner : SpawnPool<MapAgent> {
    
    [SerializeField] private float _spawnRadius = 5f;
    [SerializeField] private int _agentsLimit = 50;
    [SerializeField] private int _agentsCount = 50;
    
    [Space]
    [SerializeField] private Transform[] _points;
    
    private int _killed;
    private float _timer;

    private int AgentLimit => Mathf.Min(_agentsLimit, _agentsCount - _killed);

    public event Action<int> AgentDied;

    private void Awake() {
        Init();
    }

    private void Update() {
        if (ActiveInstances >= AgentLimit || (_timer -= Time.deltaTime) > 0) return;
        
        SpawnAgent(_points.GetRandom());

        _timer = .25f;
    }

    private void SpawnAgent(Transform spawnPoint) {
        var point = Random.insideUnitCircle * _spawnRadius;
        var position = spawnPoint.position + new Vector3(point.x, 0, point.y);
        
        var agent = GetInstance();
        agent.transform.position = position;
        agent.Die += OnAgentWasKilled;
        
        agent.gameObject.SetActive(true);
    }
    
    private void OnAgentWasKilled(MapAgent agent) {
        agent.Die -= OnAgentWasKilled;
        
        _killed++;
        
        Return(agent);
        
        AgentDied?.Invoke(_killed);
    }
}