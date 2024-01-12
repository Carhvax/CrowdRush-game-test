using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup), typeof(HorizontalLayoutGroup))]
public class AbilitiesPresenterView : MonoBehaviour, ILayoutPresenter {
    
    [SerializeField] private CanvasGroup _group;
    [SerializeField] private HorizontalLayoutGroup _layout;
    [SerializeField] private AbilitySlot[] _slots;
    
    private void OnValidate() {
        _group ??= GetComponent<CanvasGroup>();
        _layout ??= GetComponent<HorizontalLayoutGroup>();
        _slots = GetComponentsInChildren<AbilitySlot>();
    }

    public void OnShowLayout() => Appear();

    public void RequestAbilities(Action<IAbilityPanel[]> onRequest) => onRequest?.Invoke(_slots);

    public void OnHideLayout() {}

    private Sequence Appear() => DOTween
        .Sequence()
        .Append(_group.DOFade(0, 0))
        .Join(_layout.DOSpacing(500, 0))
        .Append(_group.DOFade(1, .25f))
        .Join(_layout.DOSpacing(25, 0))
        .Play();
}
