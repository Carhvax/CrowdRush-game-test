using System;
using UnityEngine;

public abstract class MapAgent : MonoBehaviour {

    public event Action<MapAgent> Die;

}
