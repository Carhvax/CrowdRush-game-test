using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class ScreenButton : MonoBehaviour, ILayoutButton {
    [SerializeField] private Button _button;
    private IMenuCommand _command;

    private void OnValidate() {
        if (_button == null) 
            _button = GetComponent<Button>();

        name = $"{GetType().Name}";
    }
    
    public void OnShowLayout() {}
    
    public void AddListener(IMenuCommand command) {
        _command = command;
        _command.State.Changed += OnCommandStateChanged;
        
        _button.onClick.AddListener(() => command.Execute());
    }
    
    private void OnCommandStateChanged(bool state) {
        _button.interactable = state;
    }

    public void OnHideLayout() {
        _button.onClick.RemoveAllListeners();
        
        if (_command == null) return;
        
        _command.State.Changed -= OnCommandStateChanged;
    }
    
}
