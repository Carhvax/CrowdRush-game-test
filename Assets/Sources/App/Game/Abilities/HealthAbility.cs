using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/HealthIncrease", fileName = "HealthAbility", order = 0)]
public class HealthAbility : DataAbility, IAbility {
    [SerializeField] private float _ratio;

    public void Execute(MobData data) => data.AddHealth(_ratio);
}
