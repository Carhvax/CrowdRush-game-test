using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenState : ScreenState {
    [SerializeField] private Image _loader;
    private Sequence _sequence;

    private void OnEnable() {
        _sequence = DOTween
            .Sequence()
            .Append(_loader.DOFillAmount(1, .5f).SetEase(Ease.Linear))
            .SetLoops(-1, LoopType.Yoyo)
            .OnStepComplete(() => _loader.fillClockwise = !_loader.fillClockwise)
            .Play();
    }

    private void OnDisable() {
        _sequence.Kill();
        _sequence = null;
    }

}