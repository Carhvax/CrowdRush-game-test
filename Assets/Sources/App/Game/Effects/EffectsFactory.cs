using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class VisualEffect : MonoBehaviour {
    public abstract void Execute(Action complete);
}

public class EffectsFactory : MonoBehaviour {
    [SerializeField] private VisualEffect[] _effects;
    [SerializeField] private Transform _content;

    private readonly HashSet<VisualEffect> _instances = new();
    
    private bool TryGetPrefab<TEffect>(out TEffect effect) where TEffect : VisualEffect {
        effect = _effects.OfType<TEffect>().FirstOrDefault();

        return effect != null;
    }
    
    public void Create<TEffect>(Vector3 position, Action<VisualEffect> complete = null) where TEffect : VisualEffect {
        if (TryGetPrefab<TEffect>(out var prefab)) {
            var instance = Instantiate(prefab, position, Quaternion.identity, _content);
            instance.Execute(() => {
                complete?.Invoke(instance);
                DisposeInstance(instance);
            });
            _instances.Add(instance);
        }
        else {
            throw new NullReferenceException($"Effect {typeof(TEffect)} not found, halt!");    
        }
    }

    public void Dispose() {
        _instances.Each(DisposeInstance);
    }

    private void DisposeInstance(VisualEffect effect) {
        _instances.Remove(effect);
        Destroy(effect.gameObject);
    }
}
