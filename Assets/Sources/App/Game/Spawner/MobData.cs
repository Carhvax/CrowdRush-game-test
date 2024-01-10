using UnityEngine;

public class MobData {
    
    public int MaxHealth { get; private set; }
    public int Health { get; private set; }
    public int SightRadius { get; private set; }
    public MapAgent PrimaryTarget { get; private set; }
    
    public float HealthAmount => Health / (float)MaxHealth;

    public MobData(int health, int sight, MapAgent target) {
        MaxHealth = health;
        Health = health;
        SightRadius = sight;
        PrimaryTarget = target;
    }
    
    public bool ApplyDamage(int damage) {
        Health = Mathf.Clamp(Health - Mathf.Abs(damage), 0, int.MaxValue);

        return Health == 0;
    }

    public void ChangeTarget() {
        
    }
}
