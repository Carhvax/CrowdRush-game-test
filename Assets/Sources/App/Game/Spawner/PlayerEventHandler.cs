using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerEventHandler : MonoBehaviour, IAgentEventsHandler {

    [SerializeField] private PlayerAgent _player;
    [SerializeField] private LayerMask _sightMask;
    
    private MobData _data;
    
    private readonly Collider[] _sight = new Collider[32];
    private MapAgent _target;
    

    public void InitHandler() => _player.SetAgentHandler(this);

    public void DisposeHandler() {}
    
    public void ApplyDamage(MapAgent mapAgent, int damage) {
        Debug.Log($"Player damaged: {damage}");
        var died = _data.ApplyDamage(damage);

        if (died) {
            mapAgent.KillAgent();
        }
    }
    
    public MapAgent NearestTarget(MapAgent agent) => agent;

    public void Tick() {
        if(!_player.IsActive) return;
        
        var direction = MoveAgent();
        
        _player.Move(direction * (Time.deltaTime * _data.MovementSpeed));
        
        SelectTarget(direction, _data.SightRadius, _data.AimTime);

        _player.ShootTarget(_data.Damage, _data.AimTime);
    }
    
    public Vector3 MoveAgent() {
        var velocity = new Vector3(
            Input.GetAxis("Horizontal"),
            0,
            Input.GetAxis("Vertical")
        ).normalized;

        return velocity;
    }
    
    public void SelectTarget(Vector3 direction, float sightRadius, float weaponReload) {
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
        
        direction = _target != null? 
            (_target.transform.position - position).normalized :
            direction;
        
        if(direction.magnitude != 0)
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
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
}