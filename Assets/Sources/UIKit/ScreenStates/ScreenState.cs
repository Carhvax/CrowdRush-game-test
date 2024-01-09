using System;
using System.Linq;

public interface ILayoutButton : ILayoutElement {
    void AddListener(IMenuCommand command);
}

public interface ILayoutPresenter : ILayoutElement {
    
}

public interface IObservableValue<T> {
    event Action<T> Changed;
    T Value { get; set; }
}

public interface IMenuCommand {
    IObservableValue<bool> State { get; }
    void Execute();
}

public abstract class ScreenState : ScreenLayout, IScreenState {

    private void OnValidate() {
        name = $"{GetType().Name}";
    }

    public void Init() {
        gameObject.SetActive(false);
        Initialize();
    }

    public void OnButtonClick<TButton>(IMenuCommand command) where TButton : class, ILayoutButton {
        if (TryGetElement<TButton>(out var buttons)) {
            buttons.Each(b => b.AddListener(command));
        }
    }

    public bool OnResolvePresenterView<TPresenter>(out TPresenter presenter) where TPresenter : class, ILayoutPresenter {
        var result = TryGetElement<TPresenter>(out var presenters);
        presenter = presenters.FirstOrDefault();

        return result;
    }
    
    public void Show() {
        gameObject.SetActive(true);
        ShowLayout();
    }
    
    public void Hide() {
        HideLayout();
        gameObject.SetActive(false);
    }
}
