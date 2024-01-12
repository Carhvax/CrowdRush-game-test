using System;
using UnityEngine;

public abstract class MapAgent : MonoBehaviour {

    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private AgentAnimator _animator;
    
    private IAgentEventsHandler _handler;
    private CapsuleCollider _collider;
    private Vector3 _defaultStartPosition;

    public bool IsActive { get; private set; } = true;
    
    public event Action<MapAgent> Die;

    private void OnValidate() {
        _collider = GetComponent<CapsuleCollider>();
    }

    public void SetAgentHandler(IAgentEventsHandler handler) {
        _handler = handler;
        IsActive = true;
        
        OnChangeAgentHandler();
    }

    protected virtual void OnChangeAgentHandler() {}
    
    public void UpdateHealth(float amount) {
        _healthBar?.SetAmount(amount);
    }
    
    public void KillAgent() {
        IsActive = false;
        KillAnimation(() => Die?.Invoke(this));
    }
    
    public bool ApplyDamage(int damage) => _handler.ApplyDamage(this, damage);

    public void PlayAttack() => AttackAnimation();

    public abstract void Move(Vector3 direction);
    
    protected void KillAnimation(Action complete) {
        _animator.Died(complete);
    }

    protected void MoveAnimation(Vector3 direction) {
        var angle = Vector3.Angle(Vector3.forward, transform.forward);
        
        if (direction.magnitude != 0) {
            angle = Mathf.RoundToInt(angle) != 90 ? angle : angle * Mathf.Sign(direction.x * -1);
            var rotated = Quaternion.Euler(Vector3.up * angle) * direction;
            direction = rotated.normalized;
        }
        
        _animator.Move(direction);
    }

    protected void AttackAnimation() {
        _animator.Attack();
    }
    
    public Vector3 GetDirectionToContact(Vector3 point) {
        var position = transform.position;
        var direction = (point - position).normalized;

        return position + direction * _collider.radius - point;
    }
}
