using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilitySlot : MonoBehaviour, IAbilityPanel, IPointerDownHandler {
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _description;
    private Action _complete;

    private void OnValidate() => name = $"{GetType().Name}";

    public void SetIcon(Sprite icon) => _icon.sprite = icon;

    public void SetDescription(string description) => _description.text = description;
    
    public void AddListener(Action complete) => _complete = complete;
    public void OnPointerDown(PointerEventData eventData) => _complete?.Invoke();
}
