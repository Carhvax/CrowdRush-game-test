using UnityEngine;

public class MobData {
    
    public int MaxHealth { get; private set; }
    public int Health { get; private set; }
    public int SightRadius { get; private set; }
    public MapAgent PrimaryTarget { get; set; }
    
    public float HealthAmount => Health / (float)MaxHealth;
    public float SpawnTimer { get; set; } = 1;
    public float MovementSpeed { get; private set; }
    public float AimTimer { get; set; } = .5f;
    public float DeathTimer { get; set; } = 1f;
    public float AimTime { get; } = .5f;
    public float DeathTime { get; } = 1f;
    public float SpawnTime { get; } = 1f;
    public int Damage { get; set; }

    public MobData(int health, int sight, int damage, float speed,  MapAgent target = null) {
        MaxHealth = health;
        Health = health;
        Damage = damage;
        SightRadius = sight;
        MovementSpeed = speed;
        PrimaryTarget = target;
    }
    
    public bool ApplyDamage(int damage) {
        Health = Mathf.Clamp(Health - Mathf.Abs(damage), 0, int.MaxValue);

        return Health == 0;
    }
}
