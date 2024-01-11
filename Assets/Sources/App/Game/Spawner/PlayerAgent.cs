using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerAgent : MapAgent {
    
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Weapon _weapon;
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private int _weaponDamage = 5;
    [SerializeField] private float _weaponReload = .5f;
    
    [Space]
    [SerializeField] private LayerMask _sightMask;
    [SerializeField] private int _sightRadius;
    
    private readonly Collider[] _sight = new Collider[32];
    private MapAgent _target;
    private float _aimTimer;

    private void Update() {
        if(!IsActive) return;
        
        var direction = MoveAgent();
        
        Move(direction);
        
        SelectTarget(direction);

        ShootTarget();
    }
    
    private void ShootTarget() {
        if (_target == null || !_target.IsActive || (_aimTimer -= Time.deltaTime) > 0) return;

        _aimTimer = _weaponReload;
        
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

        return velocity;
    }

    public override void Move(Vector3 velocity) {
        MoveAnimation(velocity);
        
        _agent.Move(velocity * (Time.deltaTime * _movementSpeed));
    }

    private void SelectTarget(Vector3 direction) {
        var position = transform.position;
        
        if (TryGetTarget<NightWalker>(position, out var agent)) {
            if (_target != agent) {
                _target = agent;
                _aimTimer = _weaponReload;    
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

    private bool TryGetTarget<TAgent>(Vector3 position, out TAgent agent) where TAgent : MapAgent {
        agent = null;
        
        return Physics.OverlapSphereNonAlloc(position, _sightRadius, _sight, _sightMask) > 0 && TryApplyFilter(position, out agent);
    }

    private bool TryApplyFilter<TAgent>(Vector3 position, out TAgent target) where TAgent: MapAgent {
        target = null;
        
        var filter = FromFullSight(position, _sight)
            .OrderBy(s => (s.transform.position - position).magnitude)
            .FirstOrDefault();

        return filter != null && filter.TryGetComponent(out target);
    }

    private IEnumerable<Collider> FromFullSight(Vector3 position, IEnumerable<Collider> colliders) {
        var result = new List<Collider>();
        
        colliders.Where(c => c != null).Each(target => {
            var direction = (target.transform.position - position) + Vector3.up * .5f;
            var ray = new Ray(position, direction);
            
            if(Physics.Raycast(ray)) result.Add(target);
        });

        return result;
    }
}