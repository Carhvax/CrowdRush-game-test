using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/DamageIncrease", fileName = "DamageAbility", order = 0)]
public class DamageAbility : DataAbility, IAbility {
    [SerializeField] private float _ratio;

    public void Execute(MobData data) => data.AddDamage(_ratio);
}
