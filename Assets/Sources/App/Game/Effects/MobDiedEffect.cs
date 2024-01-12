using System;
using UnityEngine;

public class MobDiedEffect : VisualEffect {
    
    [SerializeField] private ParticleSystem _particleSystem;
    
    public override void Execute(Action complete) {
        
        _particleSystem.Play(true);

        Delay.Execute(_particleSystem.main.duration, complete);
    }
}
