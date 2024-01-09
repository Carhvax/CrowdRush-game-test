using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface ILayoutElement {
    void OnShowLayout();
    void OnHideLayout();
}

public abstract class ScreenLayout : MonoBehaviour {
    private readonly HashSet<ILayoutElement> _elements = new();
    
    protected void Initialize() {
        GetComponentsInChildren<ILayoutElement>(true)
            .Each((e) => {
                _elements.Add(e);
            });
    }

    protected void ShowLayout() {
        _elements.Each(e => e.OnShowLayout());
    }

    protected void HideLayout() {
        _elements.Each(e => e.OnHideLayout());
    }

    protected bool TryGetElement<T>(out T[] elements) where T : class, ILayoutElement {
        elements = _elements.OfType<T>().ToArray();

        return elements.Length > 0;
    }
}
