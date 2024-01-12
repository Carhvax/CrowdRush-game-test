using UnityEngine;



[CreateAssetMenu(menuName = "Abilities/Heal", fileName = "HealAbility", order = 0)]
public class HealAbility : DataAbility, IAbility {
    
    public void Execute(MobData data) => data.Heal();

}