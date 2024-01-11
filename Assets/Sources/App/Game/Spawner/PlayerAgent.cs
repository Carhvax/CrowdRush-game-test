using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerAgent : MapAgent {
    
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Weapon _weapon;
    
    [Space]
    [SerializeField] private LayerMask _sightMask;
    
    private readonly Collider[] _sight = new Collider[32];
    private MapAgent _target;
    private float _aimTimer;

    public void ShootTarget(int weaponDamage, float weaponReload) {
        if (_target == null || !_target.IsActive || (_aimTimer -= Time.deltaTime) > 0) return;

        ResetAim(weaponReload);
        
        _weapon.Shoot(_target.transform.position + Vector3.up);

        _target.ApplyDamage(weaponDamage);
        
        AttackAnimation();
    }

    public void ResetAim(float weaponReload) {
        _aimTimer = weaponReload;
    }

    public override void Move(Vector3 velocity) {
        MoveAnimation(velocity.normalized);
        
        _agent.Move(velocity );
    }
}