using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class SpawnPool<T> : MonoBehaviour where T : MapAgent {
    
    [SerializeField] private T _prefab;
    [SerializeField] private int _preloadCount = 50;
    [SerializeField] private Transform _content;

    private readonly HashSet<T> _active = new();
    private readonly Queue<T> _passive = new();

    public int ActiveInstances => _active.Count;
    
    protected void Initialize() {
        Enumerable
            .Repeat(0, _preloadCount)
            .Each(e => AddInstance());
    }

    private void AddInstance() {
        var instance = Instantiate(_prefab, _content);
        instance.gameObject.SetActive(false);
        _passive.Enqueue(instance);
    }

    private void RemoveInstance(T instance) {
        Destroy(instance);
    }
    
    protected T GetFromPool(Vector3 position) {
        if(_passive.Count == 0) AddInstance();

        var instance = _passive.Dequeue();

        _active.Add(instance);
        
        instance.transform.position = position;
        instance.gameObject.SetActive(true);
        
        return instance;
    }

    protected void Return(T instance) {
        instance.gameObject.SetActive(false);
        
        _active.Remove(instance);
        _passive.Enqueue(instance);
    }

    public void DestroyPool() {
        _active.Each(Return);
        _passive.Each(RemoveInstance);
    }
    
}
