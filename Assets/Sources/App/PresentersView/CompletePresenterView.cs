using UnityEngine;

public class CompletePresenterView : MonoBehaviour, ILayoutPresenter {
    
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _loosePanel;
    
    public void OnShowLayout() {}
    
    public void OnHideLayout() {}
    
    public void Win(bool state) {
        _winPanel.gameObject.SetActive(state);
        _loosePanel.gameObject.SetActive(!state);
    }
}
