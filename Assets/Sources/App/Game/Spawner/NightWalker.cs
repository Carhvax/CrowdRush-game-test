using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NightWalker : MapAgent {
    [SerializeField] private NavMeshAgent _agent;

    private void OnValidate() {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Start() {
        _agent.SetDestination(Vector3.zero);
    }

}
