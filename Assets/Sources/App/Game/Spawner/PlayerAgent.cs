using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerAgent : MapAgent {
    [SerializeField] private NavMeshAgent _agent;

    private void OnValidate() {
        _agent = GetComponent<NavMeshAgent>();
    }
    
}