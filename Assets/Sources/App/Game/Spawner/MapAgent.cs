using System;
using NaughtyAttributes;
using UnityEngine;

public abstract class MapAgent : MonoBehaviour {

    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private AgentAnimator _animator;
    
    private IAgentEventsHandler _handler;

    public bool IsActive { get; private set; } = true;
    public event Action<MapAgent> Die;
    
    public void SetAgentHandler(IAgentEventsHandler handler) {
        _handler = handler;
        IsActive = true;
    }

    public void UpdateHealth(float amount) {
        _healthBar?.SetAmount(amount);
    }
    
    public void KillAgent() {
        IsActive = false;
        KillAnimation(() => Die?.Invoke(this));
    }
    
    public void ApplyDamage(int damage) {
        _handler.ApplyDamage(this, damage);
    }

    public void Move(Vector3 direction) {
        
    }
    
    [Button]
    private void Hurt() {
        _handler.ApplyDamage(this, 2);
    }

    protected void KillAnimation(Action complete) {
        _animator.Died(complete);
    }

    protected void MoveAnimation(Vector3 direction) {
        // TODO: Translate animation direction towards player facing;
        _animator.Move(direction);
    }

    protected void AttackAnimation() {
        _animator.Attack();
    }
    
    
}
