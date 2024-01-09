using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour {

    [SerializeField] private Transform[] _points;

    private readonly HashSet<MapAgent> _agents = new();

}

public abstract class MapAgent : MonoBehaviour {
    
}
