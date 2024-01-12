using UnityEngine;

public class MobData {
    
    public int MaxHealth { get; private set; }
    public int Health { get; private set; }
    public int SightRadius { get; private set; }
    public MapAgent PrimaryTarget { get; set; }
    
    public float HealthAmount => Health / (float)MaxHealth;
    public float MovementSpeed { get; private set; }
    public int Damage { get; private set; }
    public float AimTime { get; private set; } = .5f;

    public float SpawnTimer { get; set; } = 1;
    public float AimTimer { get; set; } = .5f;
    public float DeathTimer { get; set; } = 1f;
    public int Experience { get; set; }

    public int Level { get; private set; } = 1;

    public MobData(int health, int sight, int damage, float speed,  MapAgent target = null) {
        MaxHealth = Health = health;
        Damage = damage;
        SightRadius = sight;
        MovementSpeed = speed;
        PrimaryTarget = target;
    }
    
    public int GetLevel() {
        var experienceByLevel = Level * 4;

        if ((Experience / (float)experienceByLevel) > 1) {
            Experience = experienceByLevel - Experience;
            Level++;
        }

        return Level;
    }
    
    public bool ApplyDamage(int damage) {
        Health = Mathf.Clamp(Health - Mathf.Abs(damage), 0, int.MaxValue);

        return Health == 0;
    }
    
    public void Heal() => Health = MaxHealth;

    public void AddSightRadius(float ratio) => SightRadius = Mathf.RoundToInt(SightRadius * (1f + ratio));
    public void AddMovementSpeed(float ratio) => MovementSpeed = AddRatio(MovementSpeed, ratio);
    public void AddDamage(float ratio) => Damage = AddRatio(Damage, ratio);
    public void AddHealth(float ratio) {
        MaxHealth = AddRatio(MaxHealth, ratio);
        Health = AddRatio(Health, ratio);
    }
    public void DecreaseAimTime(float ratio) => AimTime = DecreaseRatio(AimTime, 1- ratio);

    private int AddRatio(int value, float ratio) => Mathf.RoundToInt(value * (1 + Mathf.Clamp01(ratio)));
    private float AddRatio(float value, float ratio) => value * (1 + Mathf.Clamp01(ratio));
    private float DecreaseRatio(float value, float ratio) => value * Mathf.Clamp01(ratio);
    
}
