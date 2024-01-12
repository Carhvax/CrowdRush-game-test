using System;

public class ObservableValue<T> : IObservableValue<T> {
    private event Action<T> _changed; 
    private T _value;
    private readonly bool _notify;

    public event Action<T> Changed {
        add {
            _changed += value;
            if(_notify)
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

    public ObservableValue(T defaults = default, bool notify = true) {
        _value = defaults;
        _notify = notify;
    }
}
