using System;
using UnityEngine;

public interface IAbilityPanel {

    void SetIcon(Sprite icon);
    void SetDescription(string description);

    void AddListener(Action complete);
}

public interface IAbility {
    void Execute(MobData data);

    void ApplyPanel(IAbilityPanel panel);
}

public abstract class DataAbility : ScriptableObject {
    [SerializeField] private Sprite _icon;
    [SerializeField] private string _description;
    
    public void ApplyPanel(IAbilityPanel panel) {
        panel.SetIcon(_icon);
        panel.SetDescription(_description);
    }
    
}
