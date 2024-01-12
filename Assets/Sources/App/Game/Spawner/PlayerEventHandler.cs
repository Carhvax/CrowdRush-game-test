using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerEventHandler : MonoBehaviour, IAgentEventsHandler {

    [SerializeField] private PlayerAgent _player;
    [SerializeField] private LayerMask _sightMask;

    [Space, Header("Player Data Setup")]
    [SerializeField] private int _health = 50;
    [SerializeField] private int _sightRadius = 5;
    [SerializeField] private int _damage = 5;
    [SerializeField] private int _speed = 5;

    private readonly Collider[] _sight = new Collider[32];
    private MobData _data;
    private MapAgent _target;
    private IStatsProvider _stats;
    
    public void InitHandler(IStatsProvider stats) {
        _stats = stats;
        _data = new MobData(_health, _sightRadius,  _damage, _speed);
        _player.SetAgentHandler(this);
        
        UpdateHealth();
    }

    public void DisposeHandler() {}
    
    public bool ApplyDamage(MapAgent mapAgent, int damage) {
        Debug.Log($"Player damaged: {damage}");
        var died = _data.ApplyDamage(damage);

        if (died) {
            mapAgent.KillAgent();
        }

        UpdateHealth();

        return died;
    }
    
    private void UpdateHealth() => _stats.PlayerHealth.Value = _data.HealthAmount;
    
    public MapAgent NearestTarget(MapAgent agent) => agent;

    public void Tick() {
        if(!_player.IsActive) return;
        
        var direction = GetInput();
        
        direction = DoMovement(direction);

        SelectTarget(direction, _data.SightRadius, _data.AimTime);

        DoDamage();
    }
    
    private void DoDamage() {
        if (_target != null && _target.IsActive && _player.ShootTarget(_target.transform.position, _data.AimTime)) {
            if (_target.ApplyDamage(_data.Damage)) {
                _data.Experience++;

                _stats.Level.Value = _data.GetLevel();
            }
        }
    }

    private Vector3 DoMovement(Vector3 direction) {
        _player.Move(direction * (Time.deltaTime * _data.MovementSpeed));

        direction = _target != null ?
            (_target.transform.position - _player.transform.position).normalized :
            direction;

        _player.Rotate(direction);
        return direction;
    }

    public Vector3 GetInput() {
        var velocity = new Vector3(
            Input.GetAxis("Horizontal"),
            0,
            Input.GetAxis("Vertical")
        ).normalized;

        return velocity;
    }

    private void SelectTarget(Vector3 direction, float sightRadius, float weaponReload) {
        var position = _player.transform.position;
        
        if (TryGetTarget<NightWalker>(position, sightRadius, out var agent)) {
            if (_target != agent) {
                _target = agent;
                _player.ResetAim(weaponReload);
            }
        }
        else {
            _target = null;
        }
    }

    private bool TryGetTarget<TAgent>(Vector3 position, float sightRadius, out TAgent agent) where TAgent : MapAgent {
        agent = null;
        
        return Physics.OverlapSphereNonAlloc(position, sightRadius, _sight, _sightMask) > 0 && TryApplyFilter(position, out agent);
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
    
    public void Use(IAbility ability) => ability.Execute(_data);
}