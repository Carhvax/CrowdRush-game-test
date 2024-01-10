using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerAgent : MapAgent {
    
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Weapon _weapon;
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private int _weaponDamage = 5;
    
    [Space]
    [SerializeField] private LayerMask _sightMask;
    [SerializeField] private int _sightRadius;
    
    private readonly Collider[] _sight = new Collider[32];
    private MapAgent _target;
    private float _aimTimer;

    private void OnValidate() {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update() {
        var direction = MoveAgent();

        SelectTarget(direction);

        ShootTarget();
    }
    private void ShootTarget() {
        if (_target == null || !_target.IsActive || (_aimTimer -= Time.deltaTime) > 0) return;

        _aimTimer = 1f;
        
        _weapon.Shoot(_target.transform.position + Vector3.up);

        _target.ApplyDamage(_weaponDamage);
        
        AttackAnimation();
    }

    private Vector3 MoveAgent() {
        var velocity = new Vector3(
            Input.GetAxis("Horizontal"),
            0,
            Input.GetAxis("Vertical")
        ).normalized;
        
        MoveAnimation(velocity);
        
        _agent.Move(velocity * (Time.deltaTime * _movementSpeed));

        return velocity;
    }

    private void SelectTarget(Vector3 direction) {
        var position = transform.position;
        
        if (Physics.OverlapSphereNonAlloc(position, _sightRadius, _sight, _sightMask) > 0) {
            var target = _sight
                .Where(s => s != null)
                .OrderBy(s => (s.transform.position - position).magnitude)
                .FirstOrDefault();

            if (target != null && target.TryGetComponent<NightWalker>(out var walker)) {
                if (_target != walker) {
                    _target = walker;
                    _aimTimer = 1f;
                }
            }
        }
        else {
            _target = null;
        }
        
        direction = _target != null? 
            (_target.transform.position - position).normalized :
            direction;
        
        if(direction.magnitude != 0)
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }
}