using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    [SerializeField] private Image _fillBar;

    private void Awake() {
        _fillBar.fillAmount = 0;
    }

    public void SetAmount(float amount) {
        _fillBar.DOFillAmount(1 - amount, .25f);
    }
}
