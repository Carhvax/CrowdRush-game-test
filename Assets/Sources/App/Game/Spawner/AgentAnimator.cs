using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AgentAnimator : MonoBehaviour {

    private static readonly int DieAnimation = Animator.StringToHash("Die");
    private static readonly int MovementX = Animator.StringToHash("MovementX");
    private static readonly int MovementZ = Animator.StringToHash("MovementZ");
    private static readonly int AttackAnimation = Animator.StringToHash("Attack");

    [SerializeField] private Animator _animator;


    private void OnValidate() {
        _animator = GetComponent<Animator>();
    }

    public void Died(Action complete) {
        _animator.SetTrigger(DieAnimation);
        
        Delay.Execute(1f, complete);
    }
    
    public void Move(Vector3 direction) {
        _animator.SetFloat(MovementX, direction.x);
        _animator.SetFloat(MovementZ, direction.z);
    }
    public void Attack() {
        _animator.SetTrigger(AttackAnimation);
    }
}
