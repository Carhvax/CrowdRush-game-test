using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NightWalker : MapAgent {
    [SerializeField] private NavMeshAgent _agent;

    private void OnValidate() {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = true;
    }
    
    public override void Move(Vector3 direction) {
        if(direction.magnitude != 0) {
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            _agent.Move(direction);
        }
        
        MoveAnimation(direction.normalized);
    }
}

