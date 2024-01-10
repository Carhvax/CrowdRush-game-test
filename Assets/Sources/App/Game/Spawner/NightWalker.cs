using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NightWalker : MapAgent {
    [SerializeField] private NavMeshAgent _agent;
    
    private MapAgent _target;

    private void OnValidate() {
        _agent = GetComponent<NavMeshAgent>();
    }

    public void SetPrimaryTarget(MapAgent target) {
        _target = target;
    }
}

