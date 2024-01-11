using TMPro;
using UnityEngine;

public class StatsPresenterView : MonoBehaviour, ILayoutPresenter {
    
    [SerializeField] private HealthBar _playerHealth;
    [SerializeField] private HealthBar _consoleHealth;
    [SerializeField] private TMP_Text _mobsRemains;

    public void OnShowLayout() {}

    public void UpdatePlayerHealth(float value) => _playerHealth.SetAmount(value);

    public void UpdateConsoleHealth(float value) => _consoleHealth.SetAmount(value);
    
    public void UpdateMobsRemain(int count) => _mobsRemains.text = count.ToString();
    
    public void OnHideLayout() {}
}
