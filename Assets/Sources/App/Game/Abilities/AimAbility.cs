using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/AimDecrease", fileName = "AimAbility", order = 0)]
public class AimAbility : DataAbility, IAbility {
    [SerializeField] private float _ratio;

    public void Execute(MobData data) => data.DecreaseAimTime(_ratio);
}
