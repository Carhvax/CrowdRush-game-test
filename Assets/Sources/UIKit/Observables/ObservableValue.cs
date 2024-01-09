using System;

public class ObservableValue<T> : IObservableValue<T> {
    private event Action<T> _changed; 
    private T _value;
    
    public event Action<T> Changed {
        add {
            _changed += value;
            value?.Invoke(_value);
        }
        remove {
            _changed -= value;
        }
    }
    public T Value {
        get => _value;
        set {
            if (!value.Equals(_value)) {
                _value = value;
                _changed?.Invoke(_value);
            }
        }
    }

    public ObservableValue(T defaults = default) {
        _value = defaults;
    }
}
