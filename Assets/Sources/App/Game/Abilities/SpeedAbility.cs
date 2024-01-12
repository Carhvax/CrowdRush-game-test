using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/SpeedIncrease", fileName = "SpeedAbility", order = 0)]
public class SpeedAbility : DataAbility, IAbility {
    [SerializeField] private float _ratio;

    public void Execute(MobData data) => data.AddMovementSpeed(_ratio);
}
